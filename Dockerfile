# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the .csproj files and restore dependencies
COPY UrlShortner.Api/UrlShortner.Api.csproj ./UrlShortner.Api/
COPY UrlShortner.Api.Models/UrlShortner.Api.Models.csproj ./UrlShortner.Api.Models/
COPY UrlShortner.Api.Services/UrlShortner.Api.Services.csproj ./UrlShortner.Api.Services/
COPY UrlShortner.Api.Facade/UrlShortner.Api.Facade.csproj ./UrlShortner.Api.Facade/
RUN dotnet restore ./UrlShortner.Api/UrlShortner.Api.csproj

# Copy the rest of the application code
COPY UrlShortner.Api/ ./UrlShortner.Api/
COPY UrlShortner.Api.Models/ ./UrlShortner.Api.Models/
COPY UrlShortner.Api.Services/ ./UrlShortner.Api.Services/
COPY UrlShortner.Api.Facade/ ./UrlShortner.Api.Facade/

# Build the application
RUN dotnet publish ./UrlShortner.Api/UrlShortner.Api.csproj -c Release -o out

# Use the official .NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Configurar portas baseadas no launchSettings
EXPOSE 5244
EXPOSE 7095

# Variáveis de ambiente para desenvolvimento
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_URLS=http://+:5244;https://+:7095
ENV DOTNET_LAUNCH_PROFILE=https

# Set the entry point for the application
ENTRYPOINT ["dotnet", "UrlShortner.Api.dll"]