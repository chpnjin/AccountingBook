FROM node:18 AS builder

# 設定工作目錄為 /app/web，與 volumes 掛載路徑一致
WORKDIR /app

# 複製 package.json 和 package-lock.json
COPY package*.json ./
# 安裝依賴
RUN npm install
# 複製應用程式的其餘檔案

COPY . .

# 確保 dist 目錄存在
RUN mkdir -p dist
RUN npm run build

# 使用 Nginx 或其他伺服器來提供靜態檔案
FROM nginx:alpine
COPY --from=builder /app/dist /usr/share/nginx/html

# 複製 Nginx 設定檔
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]