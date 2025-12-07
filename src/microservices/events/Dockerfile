FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

USER root
WORKDIR /app
EXPOSE ${ASPNETCORE_HTTP_PORTS}

FROM base AS librdkafka_build

# Установка librdkafka из пакетов (быстрее чем сборка из исходников)
RUN apt-get update && \
    apt-get install -y wget gnupg2 && \
    wget -qO - https://packages.confluent.io/deb/7.5/archive.key | apt-key add - && \
    echo "deb [arch=arm64] https://packages.confluent.io/deb/7.5 stable main" >> /etc/apt/sources.list && \
    apt-get update && \
    apt-get install -y librdkafka-dev && \
    rm -rf /var/lib/apt/lists/*



FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["./Events.api/Events.api.csproj", "."]
RUN dotnet restore "Events.api.csproj"
COPY . .
RUN dotnet build "Events.sln" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Events.sln" -c Release -o /app/publish

RUN apt-get update && \
    apt-get install -y librdkafka1 && \
    rm -rf /var/lib/apt/lists/*

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Events.api.dll"]