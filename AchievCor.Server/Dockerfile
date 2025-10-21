# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Устанавливаем Node.js для сборки клиента
RUN apt-get update -y && apt-get install -y curl && \
    curl -fsSL https://deb.nodesource.com/setup_24.x | bash - && \
    apt-get install -y nodejs

ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["AchievCor.Server/AchievCor.Server.csproj", "AchievCor.Server/"]
COPY ["achievcor.client/achievcor.client.esproj", "achievcor.client/"]
RUN dotnet restore "./AchievCor.Server/AchievCor.Server.csproj"

COPY . .


WORKDIR /src/achievcor.client
COPY achievcor.client/package*.json ./achievcor.client/
RUN npm ci --legacy-peer-deps || npm install --legacy-peer-deps
RUN npm run build


WORKDIR /src/AchievCor.Server
RUN dotnet build "./AchievCor.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AchievCor.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AchievCor.Server.dll"]