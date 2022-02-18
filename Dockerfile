#Stage 1

FROM mcr.microsoft.com/dotnet/core/sdk:5.0 AS build-env 
WORKDIR /app 
COPY . /app 
RUN dotnet restore Enoch.Api/Enoch.Api.csproj
RUN dotnet publish Enoch.Api/Enoch.Api.csproj -c Release -o /app/publish

# Stage 2
FROM mcr.microsoft.com/dotnet/core/aspnet:5.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "Enoch.Api.dll"]
