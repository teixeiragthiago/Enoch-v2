FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE_URLS=http://+:80
# ENV ASPNETCORE_URLS=http://+:80;https://+:443
ENV ASPNETCORE_ENVIRONMENT=Development
# RUN dotnet dev-certs https --trust
# ENV ASPNETCORE_Kestrel__Certificates__Default__Password: "AS78xOi3#"
# ENV ASPNETCORE_Kestrel__Certificates__Default__Path: "/https/aspnetapp.pfx"

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Enoch.Api/Enoch.Api.csproj", "src/Enoch.Api/"]
COPY ["src/Enoch.Infra/Enoch.Infra.csproj", "src/Enoch.Infra/"]
COPY ["src/Enoch.Domain/Enoch.Domain.csproj", "src/Enoch.Domain/"]
COPY ["src/Enoch.CrossCutting/Enoch.CrossCutting.csproj", "src/Enoch.CrossCutting/"]
RUN dotnet restore "src/Enoch.Api/Enoch.Api.csproj"
RUN dotnet restore "src/Enoch.Infra/Enoch.Infra.csproj"
RUN dotnet restore "src/Enoch.Domain/Enoch.Domain.csproj"
RUN dotnet restore "src/Enoch.CrossCutting/Enoch.CrossCutting.csproj"
COPY . .
WORKDIR "/src/src/Enoch.Api"
RUN dotnet build "Enoch.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Enoch.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#Método padrão .NET
ENTRYPOINT ["dotnet", "Enoch.Api.dll"]

# Opção utilizada pelo Heroku
# CMD ASPNETCORE_URLS=http://*:$PORT dotnet Enoch.Api.dll
