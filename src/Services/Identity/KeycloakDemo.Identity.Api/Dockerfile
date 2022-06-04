FROM mcr.microsoft.com/dotnet/runtime-deps:6.0-alpine-amd64 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src

COPY [".", "KeycloakDemo.Identity.Api/"]
RUN dotnet restore "KeycloakDemo.Identity.Api/KeycloakDemo.Identity.Api.csproj" -r linux-musl-x64 /p:PublishReadyToRun=true

COPY . .
WORKDIR "/src/KeycloakDemo.Identity.Api/"
#RUN dotnet build "KeycloakDemo.Identity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish --no-restore "KeycloakDemo.Identity.Api.csproj" -c Release -o /app/publish -r linux-musl-x64 --self-contained true /p:PublishTrimmed=true /p:PublishReadyToRun=true /p:PublishSingleFile=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

#Set container datetime zone
ENV TZ=America/Sao_Paulo
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

#Set user and your permissions when bash is enabled in this layer
#RUN addgroup --system --gid 1001 demousr \
    #&& adduser --system --uid 1001 --group demousr \ 
    #&& chown -R demousr:demousr /app \
    #&& chown -R demousr:demousr /home/demousr/
# USER lguilen

ENTRYPOINT ["./KeycloakDemo.Identity.Api"]
