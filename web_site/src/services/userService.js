import axios from '../services/_axios';

export default {
  //API呼叫測試
  async getServerInfo() {
    let result = await axios.get('Test/GetServerInfo').then((response) => {
      return response;
    }).catch((error) => {
      return error;
    }).finally((obj) => {
      return obj;
    });

    return result.status == 200 ? result.data : result.statusText;
  },

  //使用者認證
  async loginVerification(parm) {
    let response = await axios.post('User/LoginVerification', parm).then((axios) => {
      return {
        "token": axios.data.token,
        "id" : axios.data.id,
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
      const response = await axios.post('User/Insert', userData);

      return response.data;
    } catch (error) {
      console.error('Failed to create user:', error);
      throw error;
    }
  }
};
