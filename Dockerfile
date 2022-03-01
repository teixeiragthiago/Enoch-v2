#Stage 1

FROM mcr.microsoft.com/dotnet/core/sdk:5.0 AS build-env 
WORKDIR /app 
COPY . /app 
RUN dotnet restore Enoch.Api/Enoch.Api.csproj
RUN dotnet publish Enoch.Api/Enoch.Api.csproj -c Release -o /app/publish

RUN apt-get update && \
    apt install -y openjdk-11-jdk && \
    dotnet tool install --global dotnet-sonarscanner && \
    export PATH="$PATH:/app/.dotnet/tools" && \
    rm -rf src/**/**/bin src/**/**/obj binaries/* src/**/**/appsettings.*.json && \
    sed -i 's/<RuntimeIdentifier>[a-zA-Z0-9\-]*<\/RuntimeIdentifier>/<RuntimeIdentifier>linux-x64<\/RuntimeIdentifier\>/g' src/**/**/*.csproj && \
    sed -i 's/<RuntimeIdentifier>[a-zA-Z0-9\-]*<\/RuntimeIdentifier>/<RuntimeIdentifier>linux-x64<\/RuntimeIdentifier\>/g' test/**/**/*.csproj && \

    #2nd step -sonar
    dotnet sonarscanner begin \
    /d: sonar.login=admin \
    /d:sonar.host.url= localhost:9000 \
    /d:sonar.cs.opencover.reportsPaths=test/**/**/TestResults/**/coverage.opencover.xml \
    /d:sonar.coverage.exclusions="**Test*.cs, **Program.cs, **Startup.cs" \
    dotnet build && \
    dotnet test --collect:"XPlat Code Coverage" --no-build  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover && \
    dotnet sonarscanner end \
    /d:sonar.login=admin && \
    sed -i 's/<RuntimeIdentifier>[a-zA-Z0-9\-]*<\/RuntimeIdentifier>/<RuntimeIdentifier>linux-musl-x64<\/RuntimeIdentifier\>/g' src/**/**/*.csproj && \
    sed -i 's/<RuntimeIdentifier>[a-zA-Z0-9\-]*<\/RuntimeIdentifier>/<RuntimeIdentifier>linux-musl-x64<\/RuntimeIdentifier\>/g' test/**/**/*.csproj && \
    dotnet publish --configuration Release --self-contained false --output binaries 

# Stage 2
FROM mcr.microsoft.com/dotnet/core/aspnet:5.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "Enoch.Api.dll"]

