# 第一階段：構建階段
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src/api

# 複製 csproj 並還原依賴
COPY ["api.csproj", "."]
RUN dotnet restore "api.csproj"

ENV SQL_CONNECTION_STRING="Server=175.182.6.36,3306;Database=accounting;User=jim;Password=Syno[0912]logy"

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
