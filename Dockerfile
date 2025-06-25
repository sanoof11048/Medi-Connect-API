# Stage 1 - Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln .
COPY Medi-Connect.Application/*.csproj ./Medi-Connect.Application/
COPY Medi-Connect.Domain/*.csproj ./Medi-Connect.Domain/
COPY Medi-Connect.Infrastructure/*.csproj ./Medi-Connect.Infrastructure/
COPY Medi-Connect-API/*.csproj ./Medi-Connect-API/

RUN dotnet restore

COPY . .
WORKDIR /app/Medi-Connect-API
RUN dotnet publish -c Release -o /app/out

# Stage 2 - Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/out ./

EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "Medi-Connect.API.dll"]