#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG PASSWORD_ENV_SEEDED

RUN dotnet dev-certs https -ep /https/aspnetapp.pfx -p ${PASSWORD_ENV_SEEDED}

WORKDIR /src
COPY ["TaxManager/TaxManager.csproj", "."]
COPY ["Data/EFCore.TaxDb.csproj", "Data/"]
COPY ["UnitOfWork/UnitOfWork.csproj", "Data/"]
COPY ["Services/Services.csproj", "Data/"]

RUN dotnet restore "./TaxManager.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TaxManager/TaxManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaxManager/TaxManager.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /https/* /https/
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaxManager.dll"]