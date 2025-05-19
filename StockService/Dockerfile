FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ./ ./

RUN dotnet restore StockService/StockService.csproj
RUN dotnet build StockService/StockService.csproj -c Release -o /app/build
RUN dotnet publish StockService/StockService.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "StockService.dll"]
