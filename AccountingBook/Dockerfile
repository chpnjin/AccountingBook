FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["AccountingBook/AccountingBook.csproj", "AccountingBook/"]
RUN dotnet restore "AccountingBook/AccountingBook.csproj"
COPY . .
WORKDIR "/src/AccountingBook"
RUN dotnet build "AccountingBook.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AccountingBook.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AccountingBook.dll"]