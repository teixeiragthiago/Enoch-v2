#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Stage 1
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Stage 2
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Enoch.Api/Enoch.Api.csproj", "src/Enoch.Api/"]
COPY ["src/Enoch.Infra/Enoch.Infra.csproj", "src/Enoch.Infra/"]
COPY ["src/Enoch.Domain/Enoch.Domain.csproj", "src/Enoch.Domain/"]
COPY ["src/Enoch.CrossCutting/Enoch.CrossCutting.csproj", "src/Enoch.CrossCutting/"]
RUN dotnet restore "src/Enoch.Api/Enoch.Api.csproj"
COPY . .
WORKDIR "/src/src/Enoch.Api"
RUN dotnet build "Enoch.Api.csproj" -c Release -o /app/build

# Stage 3
FROM build AS publish
RUN dotnet publish "Enoch.Api.csproj" -c Release -o /app/publish

# Stage 4 - Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Enoch.Api.dll"]