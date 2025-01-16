//傳票查詢條件保留
import { defineStore } from 'pinia';

export const useFilterStore = defineStore('filter', {
  state: () => ({
    date_start: '',
    date_end: '',
    summary: '',
    account_list: []
  }),
});
