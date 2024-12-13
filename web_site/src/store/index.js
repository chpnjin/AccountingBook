import { createStore } from 'vuex';

const store = createStore({
  state: {      //存放全域狀態值
    user_id : null,
    name : ''
  },
  mutations: {  //修改狀態的同步函式(類似method)
    setUserId(state, userId) { // 設定 user_id
      state.user_id = userId;
    },
    setUserName(state, name) { // 設定使用者名稱
      state.name = name;
    },
  },
  actions: {     //呼叫mutations的入口，處理非同步邏輯，然後提交對應的 mutations

  },
  getters: { // 透過派生狀態進行計算
    user_id: (state) => state.user_id,
    name: (state) => state.name,
  },
});

export default store;
