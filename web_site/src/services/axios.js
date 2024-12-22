import axios from 'axios';

const apiClient = axios.create({
  baseURL: 'http://localhost:300/', //設定API位置
  timeout: 10000,
});

// 請求攔截器
apiClient.interceptors.request.use(
  config => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers['Authorization'] = 'Bearer ${token}';
    }
    return config;
  },
  error => {
    return Promise.reject(error);
  }
);

// 響應攔截器
apiClient.interceptors.response.use(
  response => response,
  error => {
    if (error.response && error.response.status === 401) {
      // 檢查當前的 URL 是否已經是登錄頁面
      if (window.location.pathname !== "/login") {
        // 令牌過期或無效，但只在不在登錄頁面時才重新定向
        localStorage.removeItem('token');
        window.location.href = "/login";
      }
    }
    return Promise.reject(error);
  }
);

export default apiClient;
