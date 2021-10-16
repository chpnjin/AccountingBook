<template>
  <div id="app">
    <header>{{mainTitle}}</header>
    <div id="center">
      <nav>
        <ul>
          <li><router-link to="/query">查詢交易紀錄</router-link></li>
          <li><router-link to="/edit">編輯交易</router-link></li>
          <li><router-link to="/settings">設定</router-link></li>
        </ul>
      </nav>
      <content>
        <router-view/>
      </content>
    </div>
    <footer>{{copyright}}</footer>
  </div>
</template>

<script>
let selfData;
module.exports = {
  data: function() {
    return {
      mainTitle: "個人帳務管理工具",
      copyright: "© 2019 陳品均(Chen Jim) 版權所有"
    };
  },
  created: function() {
    selfData = this; //將容器綁定資料
  },
  methods: {
  }
};
</script>

<style>
* {
  /* 實際寬高＝所設定的數值(已包含border和padding) */
  -webkit-box-sizing: border-box;
  -moz-box-sizing: border-box;
  box-sizing: border-box;
}

html,
body {
  margin: 0px; /*去外距*/
  height: 100%; /*自動填滿瀏覽器高度*/
}
/* 通用flexBox設定 */
.flexBox {
  display: flex;
  /*flex-wrap:當超過瀏覽器寬度時flex元素自動換行*/
  flex-wrap: wrap;
}

#app {
  height: inherit; /*自動填滿瀏覽器高度*/
  /*第一層flex--header、center、footer標籤垂直排列*/
  display: flex;
  flex-direction: column;
}

header {
  height: 50px;
  background-color: #3490c5;
}

#center {
  flex: 1; /*第一層flex--center中間包含nav與main填滿剩餘高度*/
  display: flex; /*第二層flex--將nav與main標籤水平排列*/
}

nav {
  min-width: 150px; /*第二層flex--nav固定寬度*/
  background-color: #9ed0e8;
}
/* 導覽列清單 */
nav ul {
  list-style-type: none;
  margin: 0;
  padding: 0;
}

nav ul li a {
  display: block;
  /* 清除文字特效與指定文字顏色 */
  text-decoration: none;
  color: #000;
}

/* 導覽列超連結:滑鼠移至連結 */
li a:hover {
  background-color: #A18D74;
  color: white;
}

/* 導覽列:目前所在頁面 */
.router-link-active{
  background-color: #dae6ec;
  /* color:red; */
  font-weight: 900;
}

content {
  flex: 1; /*第二層flex---main填滿剩餘寬度*/
  background-color: #dae6ec;
}

footer {
  height: 50px;
  background-color: #3490c5;
}
</style>
