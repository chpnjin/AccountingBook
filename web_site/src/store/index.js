import { createStore } from 'vuex';

const store = createStore({
  state: {      //存放全域狀態值
    user_id : "",
    name : ""
  },
  mutations: {  //修改狀態的同步函式(類似method)

  },
  actions: {     //呼叫mutations的入口，處理非同步邏輯，然後提交對應的 mutations

  },
  getters: { // 透過派生狀態進行計算

  },
});

export default store;
