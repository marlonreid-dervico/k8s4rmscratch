#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["vote/Vote.csproj", "vote/"]
RUN dotnet restore "vote/Vote.csproj"
COPY . .
WORKDIR "/src/vote"
RUN dotnet build "Vote.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vote.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app/publish
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vote.dll"]