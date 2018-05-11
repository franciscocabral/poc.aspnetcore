FROM microsoft/aspnetcore:2.0 AS base
ARG CONFIG=Debug
ENV CONFIG ${CONFIG}
WORKDIR /app
EXPOSE 80



FROM microsoft/aspnetcore-build:2.0 AS builder
ARG CONFIG=debug
ENV CONFIG ${CONFIG}
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet build -c ${CONFIG} -o /app



FROM builder AS publish
RUN dotnet publish -c ${CONFIG} -o /app



FROM base AS execution
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "poc.aspnetcore.dll"]