FROM node:18-alpine AS builder
# 設定工作目錄為 /app/web，與 volumes 掛載路徑一致
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
RUN mkdir -p dist # 確保 dist 目錄存在
RUN npm run build

FROM nginx:alpine
COPY --from=builder /app/dist /usr/share/nginx/html
# 複製 Nginx 設定檔
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]