# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/WebApp/MVC/MVC/MVC.csproj", "src/WebApp/MVC/MVC/"]
COPY ["src/WebApp/Common/Infrastructure/Infrastructure.csproj", "src/WebApp/Common/Infrastructure/"]
COPY ["src/WebApp/Common/Application/Application.csproj", "src/WebApp/Common/Application/"]
COPY ["src/WebApp/Common/Domain/Domain.csproj", "src/WebApp/Common/Domain/"]
COPY ["src/WebApp/Common/Shared/Shared.csproj", "src/WebApp/Common/Shared/"]
RUN dotnet restore "./src/WebApp/MVC/MVC/MVC.csproj"
COPY . .
WORKDIR "/src/src/WebApp/MVC/MVC"
RUN dotnet build "./MVC.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MVC.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MVC.dll"]