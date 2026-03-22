import axios from "./_axios";
//模擬讀取等待
// const sleep = (delay) => new Promise((resolve) => setTimeout(resolve, delay));

export default {
  //取得傳票清單
  async getVoucherList(searchCondition) {
    let response = await axios.post("Voucher/List", searchCondition);
    let data;

    if (response.statusText == "OK") {
      data = response.data;

      data.forEach(item => {
        item.entry_date = item.entry_date.split('T')[0];
      });

      return data;
    } else {
      return [];
    }
  },//取得交易明細
  async getVoucherDetail(no) {
    let result = await axios.get(`Voucher/GetDetail?no=${no}`);

    if (result.data.master && result.data.detail) {
      //日期轉換
      let entry_date = result.data.master.entry_date.split('T')[0];
      result.data.master.entry_date = entry_date;
      return result.data;
    } else {
      console.error(result.statusText);
      return null;
    }
  },//編輯傳票
  async editVoucher(data) {
    let result;
    //新增|編輯寫入
    result = await axios.post("Voucher/Edit", data);

    return result.data;
  }, //複製傳票結構
  async copy(no) {
    let result = await axios.get(`Voucher/Copy?voucherNo=${no}`);

    if (result.data.master && result.data.detail) {
      return result.data;
    } else {
      console.error(result.statusText);
      return null;
    }
  }, //取得可結帳年份
  async getCanClosingYears() {
    let result = await axios.get(`Voucher/GetCanClosingYears`);

    if (result.data) {
      return result.data;
    } else {
      console.error(result.statusText);
      return null;
    }
  }, //取得指定年份虛帳戶結算結果
  async getGetNominalAccountBalances(year) {
    let response = await axios.get(`Voucher/GetNominalAccountBalances?year=${year}`);

    return response.data;
  }, //生成結帳傳票
  async generateClosingVoucher(year,accoundId) {
    const response = await axios.post('Voucher/Closing', null, {
      params: {
        year: year,
        equityAccountId: accoundId // 傳送隱藏的 ID
      }
    });

    if (response.status === 200) {
      return "done"
    } else {
      return "false"
    }
  }
}
