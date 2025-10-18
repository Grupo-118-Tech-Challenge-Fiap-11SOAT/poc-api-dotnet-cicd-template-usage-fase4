FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["poc-api-dotnet-cicd-template-usage-fase4/poc-api-dotnet-cicd-template-usage-fase4.csproj", "poc-api-dotnet-cicd-template-usage-fase4/"]
RUN dotnet restore "poc-api-dotnet-cicd-template-usage-fase4/poc-api-dotnet-cicd-template-usage-fase4.csproj"
COPY poc-api-dotnet-cicd-template-usage-fase4/ poc-api-dotnet-cicd-template-usage-fase4/
WORKDIR /src/poc-api-dotnet-cicd-template-usage-fase4
RUN dotnet build "poc-api-dotnet-cicd-template-usage-fase4.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "poc-api-dotnet-cicd-template-usage-fase4.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "poc-api-dotnet-cicd-template-usage-fase4.dll"]
