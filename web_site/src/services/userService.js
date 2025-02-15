﻿import axios from '../services/_axios';

export default {
  //API呼叫測試
  async getServerInfo() {
    try {
      const response = await axios.get('Test/GetServerInfo');
      return response.status === 200 ? response.data : `API 請求失敗，狀態碼: ${response.status} ${response.statusText}`;
    } catch (error) {
      return `API 請求失敗，請確認API Server狀態(${error.message})`;
    }
  },
  //使用者認證
  async loginVerification(parm) {
    let response = await axios.post('User/LoginVerification', parm).then((axios) => {
      return {
        "token": axios.data.token,
        "id": axios.data.id,
        "name": axios.data.full_name
      };

    }).catch((error) => {
      return { "msg": error.response.data };
    });

    return response;
  },
  //使用者是否存在?
  async userExist(name, email) {
    let response = await axios.get(`User/CheckUserExist?name=${name}&email=${email}`).then((result) => {
      return result.data;
    });
    return response;
  },
  //創建使用者
  async createUser(userData) {
    try {
      const response = await axios.post('User/EditUser', userData);

      return response.data;
    } catch (error) {
      console.error('Failed to create user:', error);
      throw error;
    }
  }
};
