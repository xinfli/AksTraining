#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
ENV ASPNETCORE_URLS=http://+:80
ENV Options.FrontendApiEndpoint=http://localhost:8082/api/Message/SendMessage

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["AksTestFrontend.csproj", ""]
RUN dotnet restore "./AksTestFrontend.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "AksTestFrontend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AksTestFrontend.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AksTestFrontend.dll"]