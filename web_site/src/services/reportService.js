import axios from "../services/_axios";

export default {
  //取得科目餘額(科目類型:Asset|Liability|Revenue|Expense|Equity)
  async getAccountBalance(type) {
    let response = await axios.get(`Report/GetAccountBalanceList?accountType=${type}`);

    if (response.status == 200) {
      return response.data;
    } else {
      return [];
    }
  }
}
