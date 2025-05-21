FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["AufgabenService.API/AufgabenService.API.csproj", "AufgabenService.API/"]
COPY ["AufgabenService.Application/AufgabenService.Application.csproj", "AufgabenService.Application/"]
COPY ["AufgabenService.Domain/AufgabenService.Domain.csproj", "AufgabenService.Domain/"]
COPY ["AufgabenService.Infrastructure/AufgabenService.Infrastructure.csproj", "AufgabenService.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "AufgabenService.API/AufgabenService.API.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/AufgabenService.API"
RUN dotnet build "AufgabenService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AufgabenService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AufgabenService.API.dll"]