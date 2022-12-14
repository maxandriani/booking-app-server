#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/BookingApp.Server/BookingApp.Server.csproj", "src/BookingApp.Server/"]
COPY ["src/BookingApp.Infrastructure/BookingApp.Infrastructure.csproj", "src/BookingApp.Infrastructure/"]
COPY ["src/BookingApp.Core/BookingApp.Core.csproj", "src/BookingApp.Core/"]
RUN dotnet restore "src/BookingApp.Server/BookingApp.Server.csproj"
COPY . .
WORKDIR "/src/src/BookingApp.Server"
RUN dotnet build "BookingApp.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookingApp.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookingApp.Server.dll"]