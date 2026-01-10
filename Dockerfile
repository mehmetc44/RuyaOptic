# BUILD
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore RuyaOptik.API/RuyaOptik.API.csproj
RUN dotnet publish RuyaOptik.API/RuyaOptik.API.csproj -c Release -o /app/publish

# RUNTIME
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Container içinde ASP.NET default port
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_ENVIRONMENT=Development
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RuyaOptik.API.dll"]
