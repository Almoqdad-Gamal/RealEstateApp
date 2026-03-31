FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["RealEstateApp.API/RealEstateApp.API.csproj", "RealEstateApp.API/"]
COPY ["RealEstateApp.Application/RealEstateApp.Application.csproj", "RealEstateApp.Application/"]
COPY ["RealEstateApp.Domain/RealEstateApp.Domain.csproj", "RealEstateApp.Domain/"]
COPY ["RealEstateApp.Infrastructure/RealEstateApp.Infrastructure.csproj", "RealEstateApp.Infrastructure/"]

RUN dotnet restore "RealEstateApp.API/RealEstateApp.API.csproj"

COPY . .

WORKDIR "/src/RealEstateApp.API"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT [ "dotnet", "RealEstateApp.API.dll"]