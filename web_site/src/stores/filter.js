
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
    tel: '',

  }),
});

//傳票結構複製資料容器
export const jeCopyData = defineStore('jeData', {
  state: () => ({
    main_summary: '',
    voucher_type:'',
    account_list: []
  })
});
