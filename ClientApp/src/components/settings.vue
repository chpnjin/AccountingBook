<template>
  <div>
    <div id="tabGroup">
      <button class="tablink" @click="openPage($event,'subject')" ref="default">主科目設定</button>
      <button class="tablink" @click="openPage($event,'sub-subject')">子科目設定</button>
      <button class="tablink" @click="openPage($event,'commonlyUsedTrans')">常用交易設定</button>
    </div>
    <div id="subject" class="tabcontent">
      <table>
        <colgroup>
          <col style="width:15px" />
          <col style="width:180px" />
          <col style="width:15px" />
          <col style="width:50px" />
        </colgroup>
        <tr>
          <td>名稱</td>
          <td>
            <input type="text" />
          </td>
          <td>類型</td>
          <td>
            <select>
              <option value>資產</option>
              <option value>負債</option>
              <option value>權益</option>
              <option value>收入</option>
              <option value>支出</option>
            </select>
          </td>
        </tr>
        <tr>
          <td>說明</td>
          <td colspan="3">
            <input type="text" />
          </td>
        </tr>
      </table>
      <div class="editButtonGroup">
        <button>新增</button>
        <button>修改</button>
        <button>刪除</button>
      </div>
      <hr />
      <table border="1">
        <colgroup>
          <col style="width:150px" />
          <col style="width:80px" />
          <col style="width:100%" />
        </colgroup>
        <thead>
          <tr>
            <th>名稱</th>
            <th>類型</th>
            <th>說明</th>
          </tr>
        </thead>
      </table>
    </div>
    <div id="sub-subject" class="tabcontent">
      <table>
        <colgroup>
          <col style="width:50px" />
          <col style="width:100%" />
        </colgroup>
        <tr>
          <td>主科目</td>
          <td>
            <select>
              <option value>伙食費</option>
              <option value>交通費</option>
            </select>
          </td>
        </tr>
        <tr>
          <td>名稱</td>
          <td>
            <input type="text" />
          </td>
        </tr>
      </table>
      <div class="editButtonGroup">
        <button>新增</button>
        <button>修改</button>
        <button>刪除</button>
      </div>
      <hr />
      <table border="1">
        <colgroup>
          <col style="width:150px" />
          <col style="width:100%" />
        </colgroup>
        <thead>
          <tr>
            <th>子科目名稱</th>
            <th>說明</th>
          </tr>
        </thead>
      </table>
    </div>
    <div id="commonlyUsedTrans" class="tabcontent"></div>
  </div>
</template>

<script>
let selfData; //宣告容器
module.exports = {
  data: function() {
    return {
      title: ""
    };
  },
  mounted: function() {
    selfData = this; //將容器綁定資料
    selfData.title = "設定";
    this.$refs.default.click();
  },
  methods: {
    openPage(e, pageName) {
      //切換顯示頁面
      let allTablink, allTab, tab, i;

      allTablink = document.getElementsByClassName("tablink");
      allTab = document.getElementsByClassName("tabcontent");
      tab = document.getElementById(pageName);

      //隱藏所有頁面內容
      for (i = 0; i < allTab.length; i++) {
        allTab[i].style.display = "none";
      }

      for (i = 0; i < allTablink.length; i++) {
        allTablink[i].style.fontWeight = "";
      }

      //顯示被選中的頁面
      tab.style.display = "block";
      e.target.style.fontWeight = "1000";

      //依照所選頁面取得對應資料
      switch (pageName) {
        case "subject":
          this.getMainAccounts();
          break;
        case "sub-subject":
          this.getSubAccounts();
          break;
        case "commonlyUsedTrans":
          this.getCommonlyUsedTrans();
          break;
      }
    },
    getMainAccounts() {
      console.log("call getMainAccounts");
      //取得主科目資料
      this.axios({
        method: "get",
        url: "api/Account",
        responseType: "stream"
      }).then(function(response) {});
    },
    getSubAccounts() {
      console.log("call getSubAccounts");
      //取得子科目資料
    },
    getCommonlyUsedTrans(){
      console.log("call getCommonlyUsedTrans");
      //取得常用交易
    }
  }
};
</script>

<style>
fieldset {
  padding: 5px;
}

/* 頁籤群組 */
#tabGroup {
  display: flex;
}

/* 頁籤 */
.tablink {
  flex: 1;
  border: none;
  outline: none;
  cursor: pointer;
  font-size: 17px;
  width: 25%;
}

.tablink:hover {
  background-color: #a18d74;
  color: white;
}

/* 單頁內容 */
.tabcontent {
  display: none;
  padding: 5px;
  height: 100%;
}

table {
  table-layout: fixed;
  width: 100%;
}

td > input {
  width: 100%;
}

td > select {
  width: 100%;
}

.editButtonGroup {
  display: flex;
}

.editButtonGroup button {
  flex: 1;
}
</style>