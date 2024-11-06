#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

RUN chmod -R 755 /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
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