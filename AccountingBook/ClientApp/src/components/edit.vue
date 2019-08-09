<template>
  <div>
    <span>{{title}}</span>
    <hr />
    <div id="flexBox">
      <fieldset id="main">
        <legend>主項目</legend>
        <div id="transactionInfo" class="mainItem">
          <table border="1">
            <colgroup>
              <col id="info_col1" />
              <col id="info_col2" />
              <col id="info_col3" />
              <col id="info_col4" />
            </colgroup>
            <tr>
              <td>交易代碼</td>
              <td>
                <input type="text" id />
              </td>
              <td>交易日期</td>
              <td>
                <input type="date" />
              </td>
            </tr>
            <tr>
              <td>說明</td>
              <td colspan="3">
                <input type="text" id="remark" />
              </td>
            </tr>
          </table>
        </div>
        <div id="editButtons" class="mainItem">
          <input type="button" value="新增" />
          <input type="button" value="更新" />
          <input type="button" value="刪除" />
        </div>
      </fieldset>
      <fieldset class="detial">
        <legend align="center">借方</legend>
      </fieldset>
      <fieldset class="detial">
        <legend align="center">貸方</legend>
      </fieldset>
    </div>
  </div>
</template>

<script>
let selfData; //宣告容器
module.exports = {
  data: function() {
    return {
      title: "編輯交易資料"
    };
  },
  created: function() {
    selfData = this; //將容器綁定資料
  },
  methods: {}
};
</script>

<style>
* {
  -webkit-box-sizing: border-box;
  -moz-box-sizing: border-box;
  box-sizing: border-box;
}

#flexBox {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
}

td > input {
  width: 100%;
}

#main {
  display: flex;
  flex: 100%;
}

.detial {
  flex: 1;
}

#transactionInfo {
  flex-grow: 10;
  flex-shrink: 0;
  flex-basis: 10px;
}

/* 交易主項目表格 */
#transactionInfo table{
  width: 100%;
}

#info_col1 {
  width: 70px;
}

#info_col2{
  width: 480px;
}

#info_col3 {
  width: 70px;
}

#remark {
  width: 100%;
}
/* 交易操作按鈕 */
#editButtons {
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  flex-shrink: 0;
  flex-basis: 10px;
}
</style>