FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ./packages ./packages/

COPY ["src/Presentation/Hsm.Api/Hsm.Api.csproj", "src/Presentation/Hsm.Api/"]
COPY ["src/Core/Hsm.Application/Hsm.Application.csproj", "src/Core/Hsm.Application/"]
COPY ["src/Core/Hsm.Domain/Hsm.Domain.csproj", "src/Core/Hsm.Domain/"]
COPY ["src/Shared/ModelMapper/ModelMapper.csproj", "src/Shared/ModelMapper/"]
COPY ["src/Infrastructure/Hsm.Persistence/Hsm.Persistence.csproj", "src/Infrastructure/Hsm.Persistence/"]
RUN dotnet restore "./src/Presentation/Hsm.Api/Hsm.Api.csproj" --source ./packages --source https://api.nuget.org/v3/index.json
COPY . .
WORKDIR "/src/src/Presentation/Hsm.Api"
RUN dotnet build "./Hsm.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Hsm.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hsm.Api.dll"]