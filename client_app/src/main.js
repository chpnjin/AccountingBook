import { createApp } from 'vue' //引用Vue核心套件
import App from './App.vue' 
import router from './router/index.js' //引用路由,指向router資料夾

const app = createApp(App); //建立Vue實例
app.use(router);   //引用路由
app.mount('#app'); //掛載Vue到<div id="app"></div>