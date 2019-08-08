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
                title: ""
            }
        },
        created: function () {
            selfData = this; //將容器綁定資料
            selfData.title = "設定";
        },
        methods: {

        }
    }
</script>

<style>
</style>