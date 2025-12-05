FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base

USER root
WORKDIR /app
EXPOSE ${ASPNETCORE_HTTP_PORTS}

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["./Events.api/Events.api.csproj", "."]
RUN dotnet restore "Events.api.csproj"
COPY . .
RUN dotnet build "Events.sln" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Events.sln" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Events.api.dll"]