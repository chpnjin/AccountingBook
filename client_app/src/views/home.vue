<template>
    <div>
        <span>{{title}}</span><br />
        <hr />
    </div>
</template>

<script>
    let selfData; //宣告容器
    module.exports = {
        data: function () {
            return {
                title: "首頁"
            }
        },
        created: function () {
            selfData = this; //將容器綁定資料
        },
        methods: {

        }
    }
</script>