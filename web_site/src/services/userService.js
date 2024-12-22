import axios from '../services/axios';

export default {
  //API呼叫測試
  async callApiTest() {
    let result = await axios.get('api/Test/NoNeedAuthorizeTest').then((response) => {
      return response;
    }).catch((error) => {
      return error;
    }).finally((obj) => {
      return obj;
    });

    return result;
  },

  //取得使用者
  async loginVerification(parm) {
    let response = await axios.post('api/User/LoginVerification', parm).then((axios) => {
      return {
        "token": axios.data.token,
        "name" : axios.data.full_name
      };
    }).catch((error) => {
      return { "msg": error.response.data };
    });

    return response;
  },

  //創建使用者
  async createUser(userData) {
    try {
      const response = await axios.post('api/user/insert', userData);

      return response.data;
    } catch (error) {
      console.error('Failed to create user:', error);
      throw error;
    }
  }
};
