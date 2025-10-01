FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Server/Workers/FOT.OutboxWorker/FOT.OutboxWorker.csproj", "src/Server/Workers/FOT.OutboxWorker/"]
RUN dotnet restore "src/Server/Workers/FOT.OutboxWorker/FOT.OutboxWorker.csproj"
COPY . .
WORKDIR "/src/src/Server/Workers/FOT.OutboxWorker"
RUN dotnet build "./FOT.OutboxWorker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FOT.OutboxWorker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FOT.OutboxWorker.dll"]
