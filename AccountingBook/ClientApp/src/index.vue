<template>
    <div id="app">
        <header>
            {{mainTitle}}
        </header>
        <div id="center">
            <nav>
                <ul>
                    <li><router-link to="/query">查詢交易紀錄</router-link></li>
                    <li><router-link to="/edit">編輯交易</router-link></li>
                    <li><router-link to="/settings">設定</router-link></li>
                </ul>
            </nav>
            <main>
                <router-view>
                    <!-- 模板填入內容 -->
                </router-view>
            </main>
        </div>
        <footer>
            {{copyright}}
        </footer>
    </div>
</template>

<script>
    module.exports = {
        data: function () {
            return {
                mainTitle: "個人帳務管理工具",
                copyright: "© 2019 陳品均(Chen Jim) 版權所有"
            }
        },
        created: function () {
            selfData = this; //將容器綁定資料
        },
        methods: {

        }
    }
</script>

<style>
html, body {
    margin: 0px; /*去外距*/
    height: 100%; /*自動填滿瀏覽器高度*/
}

#app {
    height: inherit; /*自動填滿瀏覽器高度*/
    /*第一層flex--header、center、footer標籤垂直排列*/
    display: flex;
    flex-direction: column;
}

header {
    height: 50px;
    background-color: #3490C5;
}

#center {
    flex: 1; /*第一層flex--center中間包含nav與main填滿剩餘高度*/
    display: flex; /*第二層flex--將nav與main標籤水平排列*/
}

nav {
    min-width: 150px; /*第二層flex--nav固定寬度*/
    background-color: #9ED0E8;
}

main {
    flex: 1; /*第二層flex---main填滿剩餘寬度*/
    background-color: #DAE6EC;
}

footer {
    height: 50px;
    background-color: #3490C5;
}
</style>
