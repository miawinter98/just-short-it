#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

ARG BASE=8.0
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/aspnet:$BASE AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["JustShortIt.csproj", "."]
RUN dotnet restore "./././JustShortIt.csproj"
COPY . .
WORKDIR "/src/."
ARG TARGETPLATFORM
RUN dotnet build "./JustShortIt.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./JustShortIt.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY LICENSE .
ENTRYPOINT ["dotnet", "JustShortIt.dll"]