import axios from "../services/_axios";
//模擬讀取等待
const sleep = (delay) => new Promise((resolve) => setTimeout(resolve, delay));

export default {
  //取得交易紀錄
  async getTxList(searchCondition) {
    let result;

    await sleep(3000);

    //測試資料
    result = [
      {
        id: 1,
        no: "V20240001",
        createDate: "2024-01-15",
        amount: 1500,
        remark: "採購辦公用品",
        status: "Unapproved", // 未審核
      },
      {
        id: 2,
        no: "V20240002",
        createDate: "2024-01-16",
        amount: 5000,
        remark: "支付廣告費用",
        status: "Approving", // 審核中
      },
      {
        id: 3,
        no: "V20240003",
        createDate: "2024-01-17",
        amount: 10000,
        remark: "收到客戶貨款",
        status: "Approved", // 已審核
      },
      {
        id: 4,
        no: "V20240004",
        createDate: "2024-01-18",
        amount: 2000,
        remark: "支付水電費",
        status: "Posted", // 已入帳
      },
      {
        id: 5,
        no: "V20240005",
        createDate: "2024-01-19",
        amount: 800,
        remark: "重複傳票作廢",
        status: "Void", // 已作廢
      },
        {
        id: 6,
        no: "V20240006",
        createDate: "2024-01-20",
        amount: 3000,
        remark: "測試傳票",
        status: "Unapproved", // 未審核
      },
        {
        id: 7,
        no: "V20240007",
        createDate: "2024-01-21",
        amount: 7000,
        remark: "另一筆測試傳票",
        status: "Approving", // 審核中
      },
    ];

    return result;
  },

  //取得交易明細
  async getTxDetail(id){
    let result = {};

    // await sleep(3000);

    result = {
      main:{

      },
      detail:[

      ]
    };

    return result;
  }

}
