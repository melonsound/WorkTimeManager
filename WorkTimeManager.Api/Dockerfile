#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["WorkTimeManager.Api.csproj", "."]
RUN dotnet restore "./WorkTimeManager.Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "WorkTimeManager.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkTimeManager.Api.csproj" -c Release -o /app/publish

# Declare ports above 1024 as an unprivileged non-root user cannot bind to > 1024
ENV ASPNETCORE_URLS http://+:8000;https://+:8443
EXPOSE 8000
EXPOSE 8443

ENV TZ=Europe/Moscow

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkTimeManager.Api.dll"]