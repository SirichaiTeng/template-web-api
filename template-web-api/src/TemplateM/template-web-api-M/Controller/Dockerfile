# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
##USER $APP_UID
WORKDIR /app
USER app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["template-web-api/src/TemplateM/template-web-api-M/Controller/template-web-api-M.csproj", "template-web-api/src/TemplateM/template-web-api-M/Controller/"]
COPY ["template-web-api/src/TemplateM/template-web-api-M/template-web-api-M.Application/template-web-api-M.Application.csproj", "template-web-api/src/TemplateM/template-web-api-M/template-web-api-M.Application/"]
COPY ["template-web-api/src/TemplateM/template-web-api-M/template-web-api-M.Domain/template-web-api-M.Domain.csproj", "template-web-api/src/TemplateM/template-web-api-M/template-web-api-M.Domain/"]
COPY ["template-web-api/src/TemplateM/template-web-api-M/template-web-api-M.Infrastructrue/template-web-api-M.Infrastructrue.csproj", "template-web-api/src/TemplateM/template-web-api-M/template-web-api-M.Infrastructrue/"]
COPY ["template-web-api/src/TemplateM/template-web-api-M/template-web-api-M.Common/template-web-api-M.Common.csproj", "template-web-api/src/TemplateM/template-web-api-M/template-web-api-M.Common/"]
RUN dotnet restore "./template-web-api/src/TemplateM/template-web-api-M/Controller/template-web-api-M.csproj"
COPY ["template-web-api/src/TemplateM/template-web-api-M/Controller/", "template-web-api/src/TemplateM/template-web-api-M/Controller/"]
COPY ["template-web-api/src/TemplateM/template-web-api-M/template-web-api-M.Application/", "template-web-api/src/TemplateM/template-web-api-M/template-web-api-M.Application/"]
COPY ["template-web-api/src/TemplateM/template-web-api-M/template-web-api-M.Domain/", "template-web-api/src/TemplateM/template-web-api-M/template-web-api-M.Domain/"]
COPY ["template-web-api/src/TemplateM/template-web-api-M/template-web-api-M.Infrastructrue/", "template-web-api/src/TemplateM/template-web-api-M/template-web-api-M.Infrastructrue/"]
COPY ["template-web-api/src/TemplateM/template-web-api-M/template-web-api-M.Common/", "template-web-api/src/TemplateM/template-web-api-M/template-web-api-M.Common/"]
WORKDIR "/src/template-web-api/src/TemplateM/template-web-api-M/Controller"
RUN dotnet build "./template-web-api-M.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./template-web-api-M.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER app
ENTRYPOINT ["dotnet", "template-web-api-M.dll"]