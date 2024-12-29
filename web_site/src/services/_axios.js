import axios from "axios";

const apiClient = axios.create({
  baseURL: "http://localhost:300/", //設定API位置
  timeout: 50000, //等待Server回應時限
});

// 請求攔截器
apiClient.interceptors.request.use(
  (config) => {
    //非登入驗證均需在請求標頭帶入JWT
    if (config.url != "api/User/LoginVerification") {
      const token = localStorage.getItem("token");
      if (token) {
        config.headers["Authorization"] = `Bearer ${token}`;
      }
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

//回應攔截處裡
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response && error.response.status === 401) {
      // 檢查當前的 URL 是否已經是登錄頁面
      if (window.location.pathname !== "/login") {
        // 令牌過期或無效，但只在不在登錄頁面時才重新定向
        localStorage.removeItem("token");
        window.location.href = "/login";
      }
    }
    return Promise.reject(error);
  }
);

export default apiClient;
