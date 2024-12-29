//框架入口
import { createApp } from 'vue'

import App from './App.vue'
import router from './router' //引用路由
import store from './store'; //引用狀態管理
import './assets/main.css'


const app = createApp(App);
app.use(router);
app.use(store);
app.mount('#app'); //主框架的載入目標元件
