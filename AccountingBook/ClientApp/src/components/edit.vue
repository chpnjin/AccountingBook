<template>
  <div>
    <span>{{title}}</span>
    <hr />
    <div id="flexBox">
      <fieldset id="main">
        <legend>主項目</legend>
        <table border="1">
            <colgroup>
            <col>
            <col>
            </colgroup>
          <tr>
            <td>交易代碼</td>
            <td><input type="text" id=""></td>
          </tr>
          <tr>
            <td>交易日期</td>
            <td><input type="date"></td>
          </tr>
          <tr>
            <td>說明</td>
            <td><input type="text"></td>
          </tr>
        </table>
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
#flexBox {
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
}

#main {
  flex: 100%;
}

.detial {
  flex: 1;
}
</style>