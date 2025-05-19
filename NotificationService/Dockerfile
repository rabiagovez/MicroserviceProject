FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ./ ./

RUN dotnet restore NotificationService/NotificationService.csproj
RUN dotnet build NotificationService/NotificationService.csproj -c Release -o /app/build
RUN dotnet publish NotificationService/NotificationService.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.dll"]
