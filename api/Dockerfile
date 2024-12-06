## 請參閱 https://aka.ms/customizecontainer 了解如何自訂您的偵錯容器，以及 Visual Studio 如何使用此 Dockerfile 來組建您的映像，以加快偵錯速度。
#
## 此階段用於以快速模式從 VS 執行時 (偵錯設定的預設值)
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER app
#WORKDIR /app
#EXPOSE 8080
#EXPOSE 8081
#
#
## 此階段是用來組建服務專案
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#ARG BUILD_CONFIGURATION=Release
#WORKDIR /src
#COPY ["api.csproj", "api/"]
#RUN dotnet restore "./api/api.csproj"
#COPY . .
#WORKDIR "/src/api"
#RUN dotnet build "./api.csproj" -c $BUILD_CONFIGURATION -o /app/build
#
## 此階段可用來發佈要複製到最終階段的服務專案
#FROM build AS publish
#ARG BUILD_CONFIGURATION=Release
#RUN dotnet publish "./api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
#
## 此階段用於生產環境，或以一般模式從 VS 執行時 (未使用偵錯設定時的預設值)
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "api.dll"]

# 第一階段：構建階段
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src/api

# 複製 csproj 並還原依賴
COPY ["api.csproj", "."]
RUN dotnet restore "api.csproj"

# 複製剩餘檔案並構建應用
COPY . .
RUN dotnet build "api.csproj" -c Release -o /app/build

# 第二階段：發佈階段
FROM build AS publish
RUN dotnet publish "api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 第三階段：運行階段
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# 從 publish 階段複製發佈結果
COPY --from=publish /app/publish .

# 設置容器啟動命令
ENTRYPOINT ["dotnet", "api.dll"]
