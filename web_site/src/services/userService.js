import axios from 'axios';

export default {
  //取得使用者
  async getUser(parm) {
    try {
      const response = await axios.get(`/api/users/${id}`);
      return response.data;
    } catch (error) {
      console.error('Failed to fetch user:', error);
      throw error;
    }
  },

  //創建使用者
  async createUser(userData) {
    try {
      const response = await axios.post('/api/user/insert', userData);
      return response.data;
    } catch (error) {
      console.error('Failed to create user:', error);
      throw error;
    }
  }
};
