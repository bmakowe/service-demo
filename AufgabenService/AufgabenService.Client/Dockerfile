FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AufgabenService.Client.csproj", "./"]
RUN dotnet restore "AufgabenService.Client.csproj"
COPY . .
RUN dotnet build "AufgabenService.Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AufgabenService.Client.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
EXPOSE 443
