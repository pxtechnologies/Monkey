FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Monkey.Sql.WebApiHost/Monkey.Sql.WebApiHost.csproj Monkey.Sql.WebApiHost/
COPY Monkey.Cqrs/Monkey.Cqrs.csproj Monkey.Cqrs/
COPY Monkey.WebApi.SimpleInjector/Monkey.WebApi.SimpleInjector.csproj Monkey.WebApi.SimpleInjector/
COPY Monkey.WebApi/Monkey.WebApi.csproj Monkey.WebApi/
COPY Monkey/Monkey.csproj Monkey/
COPY Monkey.Sql.SimpleInjector/Monkey.Sql.SimpleInjector.csproj Monkey.Sql.SimpleInjector/
COPY Monkey.Sql/Monkey.Sql.csproj Monkey.Sql/
COPY Monkey.SimpleInjector/Monkey.SimpleInjector.csproj Monkey.SimpleInjector/
RUN dotnet restore Monkey.Sql.WebApiHost/Monkey.Sql.WebApiHost.csproj
COPY . .
WORKDIR /src/Monkey.Sql.WebApiHost
RUN dotnet build Monkey.Sql.WebApiHost.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Monkey.Sql.WebApiHost.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Monkey.Sql.WebApiHost.dll"]
