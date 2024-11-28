# 簡介
電子帳簿網頁板(.Net Core + Vue.js)

此專案後端使用.Net Core框架,前端使用Vue.js框架
實現前後端關注點分離的Side Project

第一階段預計將原Windows Form版的功能完全複製到此專案中

<H3>2024-11-28</H3>

後端API 改為 NET8 框架
前端網站 改為獨立執行 Vue 3.0 框架網站

兩者個別使用Dockerfile單獨打包，再用docker-compose整合打包後同時執行
此為因應未來微服務架構趨勢而改動