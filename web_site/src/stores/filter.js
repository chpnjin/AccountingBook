
import { defineStore } from 'pinia';

//傳票查詢條件保留
export const jeSearchCondition = defineStore('filter', {
  state: () => ({
    date_start: '',
    date_end: '',
    summary: '',
    account_list: []
  }),
});

//使用者編輯查詢條件
export const userSearchCondition = defineStore('filter', {
  state: () => ({
    name: '',
    email: '',
    tel:'',
    
  }),
});
