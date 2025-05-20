FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PruefungService/PruefungService.API/PruefungService.API.csproj", "PruefungService.API/"]
RUN dotnet restore "PruefungService.API/PruefungService.API.csproj"

COPY ["PruefungService/PruefungService.API/", "PruefungService.API/"]
WORKDIR "/src/PruefungService.API"
RUN dotnet build "PruefungService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PruefungService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PruefungService.API.dll"]