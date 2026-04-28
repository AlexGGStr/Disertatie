FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PropertyService/PropertyService.csproj", "PropertyService/"]
RUN dotnet restore "PropertyService/PropertyService.csproj"
COPY . .
WORKDIR "/src/PropertyService"
RUN dotnet build "PropertyService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PropertyService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PropertyService.dll"]
