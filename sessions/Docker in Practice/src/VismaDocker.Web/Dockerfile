FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build
WORKDIR /src

# copy csproj and restore as distinct layers
#COPY VismaDocker.Web.csproj ./VismaDocker.Web.csproj
#RUN dotnet restore

# copy everything else and build
COPY . .
WORKDIR /src
RUN dotnet publish -c Release -o /app

FROM build AS publish

FROM base AS final

WORKDIR /app

COPY --from=publish /app .
ENV ASPNETCORE_URLS="http://0.0.0.0:5000"
ENTRYPOINT ["dotnet", "VismaDocker.Web.dll"]