﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["UserManagement/UserManagement.csproj", "UserManagement/"]
RUN dotnet restore "UserManagement/UserManagement.csproj"
COPY . .
WORKDIR "/src/UserManagement"
RUN dotnet build "UserManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserManagement.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserManagement.dll"]
