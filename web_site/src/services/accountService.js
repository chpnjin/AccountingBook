import axios from "../services/_axios";

export default {
  //取得主科目列表
  async getMainAccounts() {
    let response;

    try {
      response = await axios.post("Account/GetMainAccounts");
      return response.data;
    } catch (error) {
      console.info(error);
      return [];
    }
  },
  //取得子科目列表(主科目ID)
  async getSubAccounts(mainId) {
    let response;
    let parms = {
      main_id: mainId
    }

    try {
      response = await axios.post("Account/GetSubAccounts", parms);
      return response.data;
    } catch (error) {
      return [];
    }
  },
  //檢查ID是否已存在
  async accountIdExist(accountNo) {
    let response;

    try {
      response = await axios.get(`Account/CheckAccountExist?accountNo=${accountNo}`).then((result)=>{
        return result.data;
      });

      return response;

    } catch (error) {
      console.info(error);
      return error;
    }

  },
  //編輯主科目
  async editMainAccount(parms) {
    let response;
    parms.main_id = null;

    try {
      response = await axios.post("Account/Edit", parms);
      return response.data;
    } catch (error) {
      console.info(error);
      return error;
    }
  },
  //編輯子科目
  async editSubAccount(parms) {
    let response;

    try {
      response = await axios.post("Account/Edit", parms);
      return response.data;
    } catch (error) {
      console.info(error);
      return error;
    }
  }
};
