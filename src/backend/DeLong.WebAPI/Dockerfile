# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/backend/DeLong.WebAPI/DeLong.WebAPI.csproj", "src/backend/DeLong.WebAPI/"]
COPY ["src/backend/DeLong.Application/DeLong.Service.csproj", "src/backend/DeLong.Application/"]
COPY ["src/backend/DeLong.Domain/DeLong.Domain.csproj", "src/backend/DeLong.Domain/"]
COPY ["src/backend/DeLong.Infrastructure/DeLong.Data.csproj", "src/backend/DeLong.Infrastructure/"]
RUN dotnet restore "./src/backend/DeLong.WebAPI/DeLong.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/backend/DeLong.WebAPI"
RUN dotnet build "./DeLong.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DeLong.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeLong.WebAPI.dll"]