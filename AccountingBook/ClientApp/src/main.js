import Vue from 'vue'
import VueRouter from 'vue-router'
import index from './index.vue'
import query from './components/query.vue';
import edit from './components/edit.vue';
import settings from './components/settings.vue';

//引用Vue路由
Vue.use(VueRouter);

Vue.config.productionTip = false;

const router = new VueRouter({
  routes: [
      { path: '/query', component: query },
      { path: '/edit', component: edit },
      { path: '/settings', component: settings }
  ]
});

new Vue({
  render: h => h(index),
  router
}).$mount('#app');
