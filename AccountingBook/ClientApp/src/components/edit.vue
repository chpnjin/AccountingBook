﻿<template>
  <div>
    <span>{{title}}</span>
    <hr />
    <div class="flexBox">
      <fieldset>
        <legend>主項目</legend>
        <table id="masterInfo" border="1">
          <colgroup>
            <col class="itemNameCol" />
            <col style=" width:480px" />
            <col class="itemNameCol" />
            <col style="width:120px" />
            <col style="width:70px" />
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
            <td rowspan="2">
              <div class="editButtons">
                <button id="btnAddTxn" @click="addTxn">新增</button>
                <button id="btnUpdateTxn" @click="updateTxn">更新</button>
                <button id="btnDeleteTxn" @click="deleteTxn">刪除</button>
              </div>
            </td>
          </tr>
          <tr>
            <td>說明</td>
            <td colspan="3">
              <input type="text" id="remark" />
            </td>
          </tr>
        </table>
        <div id="txnMasterActions"></div>
      </fieldset>
      <fieldset class="detial">
        <legend align="center">借方</legend>
        <table border="1" class="detialInfo">
          <colgroup>
            <col class="itemNameCol" />
            <col />
            <col style="width:70px"/>
          </colgroup>
          <tr>
            <td>
              <span>科目名稱</span>
            </td>
            <td>
              <select>
                <option value>手頭現金</option>
                <option value>伙食費</option>
              </select>
              <span>{{accountType}}</span>
            </td>
            <td rowspan="2">
              <div class="editButtons">
                <button value>新增</button>
                <button value>刪除</button>
              </div>
            </td>
          </tr>
          <tr>
            <td>
              <span>金額</span>
            </td>
            <td>
              <input type="number" value="1" min="1" class="detial_input_amount" />
            </td>
          </tr>
        </table>
        <table border="1" class="detial_items_list">
          <colgroup>
            <col />
            <col />
          </colgroup>
          <thead>
            <th>科目名稱</th>
            <th>金額</th>
          </thead>
          <tbody></tbody>
        </table>
      </fieldset>
      <fieldset class="detial">
        <legend align="center">貸方</legend>
        <table border="1" class="detialInfo">
          <colgroup>
            <col class="itemNameCol" />
            <col />
            <col style="width:70px"/>
          </colgroup>
          <tr>
            <td>
              <span>科目名稱</span>
            </td>
            <td>
              <select>
                <option value>手頭現金</option>
                <option value>伙食費</option>
              </select>
              <span>{{accountType}}</span>
            </td>
            <td rowspan="2">
              <div class="detial_btn editButtons">
                <button value>新增</button>
                <button value>刪除</button>
              </div>
            </td>
          </tr>
          <tr>
            <td>
              <span>金額</span>
            </td>
            <td>
              <input type="number" value="1" min="1" class="detial_input_amount" />
            </td>
          </tr>
        </table>
        <table border="1" class="detial_items_list">
          <colgroup>
            <col />
            <col />
          </colgroup>
          <thead>
            <th>科目名稱</th>
            <th>金額</th>
          </thead>
          <tbody></tbody>
        </table>
      </fieldset>
    </div>
  </div>
</template>

<script>
module.exports = {
  data: function() {
    return {
      title: "編輯交易資料",
      accountType: "[科目類別]"
    };
  },
  created: function() {},
  methods: {
    addTxn: function() {},
    updateTxn: function() {},
    deleteTxn: function() {}
  }
};
</script>

<style>
td > input {
  width: 100%;
}

/* 借方與貸方欄位 */
.detial {
  display: flex;
  flex-wrap: wrap;
  flex: 1;
}
/* 交易明細編輯欄位 */
.detialInfo {
  flex-grow: 1;
  flex-basis: auto;
  width: 100%;
}

/* 交易明細項目清單 */
.detial_items_list {
  flex-grow: 1;
  flex-basis: auto;
  width: 100%;
}

#masterInfo {
  /* flexItem基準為10px * 10倍 = 100px */
  flex-grow: 1;
  flex-basis: 10px;
  table-layout: fixed;
  width: 100%;
}

.itemNameCol {
  width: 80px;
}

#remark {
  width: 100%;
}

/* 交易主表操作 */
#txnMasterActions {
  flex-grow: 1;
  flex-basis: 10px;
}

/* 編輯按鈕垂直排列 */
.editButtons {
  display: flex;
  flex-direction: column;
}
</style>