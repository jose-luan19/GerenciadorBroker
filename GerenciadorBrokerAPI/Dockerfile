FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GerenciadorBrokerAPI/GerenciadorBrokerAPI.csproj", "GerenciadorBrokerAPI/"]
COPY ["Infra/Infra.csproj", "Infra/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["CrossCouting/CrossCouting.csproj", "CrossCouting/"]
RUN dotnet restore "./GerenciadorBrokerAPI/GerenciadorBrokerAPI.csproj"
COPY . .
WORKDIR "/src/GerenciadorBrokerAPI"
RUN dotnet build "./GerenciadorBrokerAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GerenciadorBrokerAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GerenciadorBrokerAPI.dll"]