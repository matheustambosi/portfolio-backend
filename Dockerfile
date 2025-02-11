FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

RUN apt-get update && \
    apt-get install -y \
    git \
    curl \
    ca-certificates \
    lsb-release \
    sudo \
    certbot python3-certbot-nginx \
    gnupg2 && \
    curl -sSL https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl -sSL https://packages.microsoft.com/config/debian/10/prod.list > /etc/apt/sources.list.d/microsoft-prod.list && \
    apt-get update && \
    apt-get install -y dotnet-sdk-8.0 && \
    curl -fsSL https://get.docker.com | sh

RUN curl -sSL https://github.com/docker/compose/releases/download/v2.32.4/docker-compose-linux-x86_64 -o /usr/local/bin/docker-compose && \
    chmod +x /usr/local/bin/docker-compose

RUN docker-compose --version

RUN chmod -R 755 /app

FROM base AS build
WORKDIR /src
COPY ["AtletiGo/AtletiGo.csproj", "AtletiGo/"]
RUN dotnet restore "AtletiGo/AtletiGo.csproj"
COPY . .
WORKDIR "/src/AtletiGo"
RUN dotnet build "AtletiGo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AtletiGo.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN mkdir -p /app/certificates
COPY certificates/aspnetapp.pfx /app/certificates

ENTRYPOINT ["dotnet", "AtletiGo.dll"]