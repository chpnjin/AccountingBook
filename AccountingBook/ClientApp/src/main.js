import Vue from 'vue';
import VueRouter from 'vue-router';
import axios from 'axios'
import VueAxios from 'vue-axios'
import index from './index.vue'; //首頁
import query from './components/query.vue'; //分頁:查詢
import edit from './components/edit.vue'; //分頁:編輯
import settings from './components/settings.vue'; //分頁:設定

Vue.use(VueRouter);      //引用Vue路由
Vue.use(VueAxios,axios); //引用AJAX套件
Vue.config.productionTip = false;

const router = new VueRouter({
  routes: [
    {
      path: "/query",
      name: "query",
      component: query
    },
    {
      path: "/edit",
      name: "edit",
      component: edit
    },
    {
      path: "/settings",
      name: "settings",
      component: settings
    }
  ]
});

new Vue({
  render: h => h(index),
  router
}).$mount('#app');
