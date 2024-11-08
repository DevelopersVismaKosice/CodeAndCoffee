FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as base
WORKDIR /app
EXPOSE 80
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
WORKDIR /src

COPY . .
WORKDIR /src/VismaDocker.ScraperWorker
RUN dotnet publish -c Release -o /app

FROM build AS publish

FROM base AS final

WORKDIR /app

COPY --from=publish /app .
# ENV ASPNETCORE_URLS="http://0.0.0.0:5001"
ENTRYPOINT ["dotnet", "VismaDocker.ScraperWorker.dll"]