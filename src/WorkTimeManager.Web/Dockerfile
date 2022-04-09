#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/WorkTimeManager.Web/WorkTimeManager.Web.csproj", "src/WorkTimeManager.Web/"]
COPY ["src/WorkTimeManager.Security/WorkTimeManager.Security.csproj", "src/WorkTimeManager.Security/"]
COPY ["src/WorkTimeManager.Core/WorkTimeManager.Core.csproj", "src/WorkTimeManager.Core/"]
COPY ["src/WorkTimeManager.Infrastructure/WorkTimeManager.Infrastructure.csproj", "src/WorkTimeManager.Infrastructure/"]
RUN dotnet restore "src/WorkTimeManager.Web/WorkTimeManager.Web.csproj"
COPY . .
WORKDIR "/src/src/WorkTimeManager.Web"
RUN dotnet build "WorkTimeManager.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WorkTimeManager.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WorkTimeManager.Web.dll"]