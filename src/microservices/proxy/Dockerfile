FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base

USER root
WORKDIR /app
EXPOSE ${ASPNETCORE_HTTP_PORTS}

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["./Proxy.Api/Proxy.Api.csproj", "."]
RUN dotnet restore "Proxy.Api.csproj"
COPY . .
RUN dotnet build "Proxy.sln" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Proxy.sln" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Proxy.Api.dll"]