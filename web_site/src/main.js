//框架入口
import { createApp } from 'vue'
import { createPinia } from 'pinia' //引用狀態管理

import App from './App.vue'
import router from './router' //引用路由
import '@/assets/main.css'
import '@/assets/btn.css'

const app = createApp(App);
const pinia = createPinia();

app.use(router);
app.use(pinia);
app.mount('#app'); //主框架的載入目標元件
