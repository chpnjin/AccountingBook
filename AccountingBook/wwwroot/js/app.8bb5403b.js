(function(t){function e(e){for(var a,i,c=e[0],o=e[1],l=e[2],d=0,f=[];d<c.length;d++)i=c[d],r[i]&&f.push(r[i][0]),r[i]=0;for(a in o)Object.prototype.hasOwnProperty.call(o,a)&&(t[a]=o[a]);u&&u(e);while(f.length)f.shift()();return s.push.apply(s,l||[]),n()}function n(){for(var t,e=0;e<s.length;e++){for(var n=s[e],a=!0,c=1;c<n.length;c++){var o=n[c];0!==r[o]&&(a=!1)}a&&(s.splice(e--,1),t=i(i.s=n[0]))}return t}var a={},r={app:0},s=[];function i(e){if(a[e])return a[e].exports;var n=a[e]={i:e,l:!1,exports:{}};return t[e].call(n.exports,n,n.exports,i),n.l=!0,n.exports}i.m=t,i.c=a,i.d=function(t,e,n){i.o(t,e)||Object.defineProperty(t,e,{enumerable:!0,get:n})},i.r=function(t){"undefined"!==typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(t,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(t,"__esModule",{value:!0})},i.t=function(t,e){if(1&e&&(t=i(t)),8&e)return t;if(4&e&&"object"===typeof t&&t&&t.__esModule)return t;var n=Object.create(null);if(i.r(n),Object.defineProperty(n,"default",{enumerable:!0,value:t}),2&e&&"string"!=typeof t)for(var a in t)i.d(n,a,function(e){return t[e]}.bind(null,a));return n},i.n=function(t){var e=t&&t.__esModule?function(){return t["default"]}:function(){return t};return i.d(e,"a",e),e},i.o=function(t,e){return Object.prototype.hasOwnProperty.call(t,e)},i.p="/";var c=window["webpackJsonp"]=window["webpackJsonp"]||[],o=c.push.bind(c);c.push=e,c=c.slice();for(var l=0;l<c.length;l++)e(c[l]);var u=o;s.push([0,"chunk-vendors"]),n()})({0:function(t,e,n){t.exports=n("56d7")},"2c17":function(t,e,n){"use strict";var a=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",[n("span",[t._v(t._s(t.title))]),n("hr"),n("div",{staticClass:"flexBox"},[n("fieldset",[n("legend",[t._v("主項目")]),n("table",{attrs:{id:"masterInfo",border:"1"}},[t._m(0),n("tr",[n("td",[t._v("交易代碼")]),t._m(1),n("td",[t._v("交易日期")]),t._m(2),n("td",{attrs:{rowspan:"2"}},[n("div",{staticClass:"editButtons"},[n("button",{attrs:{id:"btnAddTxn"},on:{click:t.addTxn}},[t._v("新增")]),n("button",{attrs:{id:"btnUpdateTxn"},on:{click:t.updateTxn}},[t._v("更新")]),n("button",{attrs:{id:"btnDeleteTxn"},on:{click:t.deleteTxn}},[t._v("刪除")])])])]),t._m(3)]),n("div",{attrs:{id:"txnMasterActions"}})]),n("fieldset",{staticClass:"detial"},[n("legend",{attrs:{align:"center"}},[t._v("借方")]),n("table",{staticClass:"detialInfo",attrs:{border:"1"}},[t._m(4),n("tr",[t._m(5),n("td",[t._m(6),n("span",[t._v(t._s(t.accountType))])]),t._m(7)]),t._m(8)]),t._m(9)]),n("fieldset",{staticClass:"detial"},[n("legend",{attrs:{align:"center"}},[t._v("貸方")]),n("table",{staticClass:"detialInfo",attrs:{border:"1"}},[t._m(10),n("tr",[t._m(11),n("td",[t._m(12),n("span",[t._v(t._s(t.accountType))])]),t._m(13)]),t._m(14)]),t._m(15)])])])},r=[function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("colgroup",[n("col",{staticClass:"itemNameCol"}),n("col",{staticStyle:{width:"480px"}}),n("col",{staticClass:"itemNameCol"}),n("col",{staticStyle:{width:"120px"}}),n("col",{staticStyle:{width:"70px"}})])},function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("td",[n("input",{attrs:{type:"text",id:""}})])},function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("td",[n("input",{attrs:{type:"date"}})])},function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("tr",[n("td",[t._v("說明")]),n("td",{attrs:{colspan:"3"}},[n("input",{attrs:{type:"text",id:"remark"}})])])},function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("colgroup",[n("col",{staticClass:"itemNameCol"}),n("col"),n("col",{staticStyle:{width:"70px"}})])},function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("td",[n("span",[t._v("科目名稱")])])},function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("select",[n("option",{attrs:{value:""}},[t._v("手頭現金")]),n("option",{attrs:{value:""}},[t._v("伙食費")])])},function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("td",{attrs:{rowspan:"2"}},[n("div",{staticClass:"editButtons"},[n("button",{attrs:{value:""}},[t._v("新增")]),n("button",{attrs:{value:""}},[t._v("刪除")])])])},function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("tr",[n("td",[n("span",[t._v("金額")])]),n("td",[n("input",{staticClass:"detial_input_amount",attrs:{type:"number",value:"1",min:"1"}})])])},function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("table",{staticClass:"detial_items_list",attrs:{border:"1"}},[n("colgroup",[n("col"),n("col")]),n("thead",[n("th",[t._v("科目名稱")]),n("th",[t._v("金額")])]),n("tbody")])},function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("colgroup",[n("col",{staticClass:"itemNameCol"}),n("col"),n("col",{staticStyle:{width:"70px"}})])},function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("td",[n("span",[t._v("科目名稱")])])},function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("select",[n("option",{attrs:{value:""}},[t._v("手頭現金")]),n("option",{attrs:{value:""}},[t._v("伙食費")])])},function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("td",{attrs:{rowspan:"2"}},[n("div",{staticClass:"detial_btn editButtons"},[n("button",{attrs:{value:""}},[t._v("新增")]),n("button",{attrs:{value:""}},[t._v("刪除")])])])},function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("tr",[n("td",[n("span",[t._v("金額")])]),n("td",[n("input",{staticClass:"detial_input_amount",attrs:{type:"number",value:"1",min:"1"}})])])},function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("table",{staticClass:"detial_items_list",attrs:{border:"1"}},[n("colgroup",[n("col"),n("col")]),n("thead",[n("th",[t._v("科目名稱")]),n("th",[t._v("金額")])]),n("tbody")])}];n.d(e,"a",function(){return a}),n.d(e,"b",function(){return r})},"2d50":function(t,e){var n;t.exports={data:function(){return{title:""}},mounted:function(){n=this,n.title="設定",this.$refs.default.click()},methods:{openPage:function(t,e){var n,a,r,s;for(n=document.getElementsByClassName("tablink"),a=document.getElementsByClassName("tabcontent"),r=document.getElementById(e),s=0;s<a.length;s++)a[s].style.display="none";for(s=0;s<n.length;s++)n[s].style.fontWeight="";switch(r.style.display="block",t.target.style.fontWeight="1000",e){case"subject":this.getMainAccounts();break;case"sub-subject":this.getSubAccounts();break;case"commonlyUsedTrans":this.getCommonlyUsedTrans();break}},getMainAccounts:function(){console.log("call getMainAccounts"),this.axios({method:"get",url:"api/Account",responseType:"stream"}).then(function(t){})},getSubAccounts:function(){console.log("call getSubAccounts")},getCommonlyUsedTrans:function(){console.log("call getCommonlyUsedTrans")}}}},"2f97":function(t,e,n){},"3f6f":function(t,e,n){},5438:function(t,e,n){},"55a6":function(t,e,n){"use strict";var a=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",[n("div",{attrs:{id:"tabGroup"}},[n("button",{ref:"default",staticClass:"tablink",on:{click:function(e){return t.openPage(e,"subject")}}},[t._v("主科目設定")]),n("button",{staticClass:"tablink",on:{click:function(e){return t.openPage(e,"sub-subject")}}},[t._v("子科目設定")]),n("button",{staticClass:"tablink",on:{click:function(e){return t.openPage(e,"commonlyUsedTrans")}}},[t._v("常用交易設定")])]),t._m(0),t._m(1),n("div",{staticClass:"tabcontent",attrs:{id:"commonlyUsedTrans"}})])},r=[function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",{staticClass:"tabcontent",attrs:{id:"subject"}},[n("table",[n("colgroup",[n("col",{staticStyle:{width:"15px"}}),n("col",{staticStyle:{width:"180px"}}),n("col",{staticStyle:{width:"15px"}}),n("col",{staticStyle:{width:"50px"}})]),n("tr",[n("td",[t._v("名稱")]),n("td",[n("input",{attrs:{type:"text"}})]),n("td",[t._v("類型")]),n("td",[n("select",[n("option",{attrs:{value:""}},[t._v("資產")]),n("option",{attrs:{value:""}},[t._v("負債")]),n("option",{attrs:{value:""}},[t._v("權益")]),n("option",{attrs:{value:""}},[t._v("收入")]),n("option",{attrs:{value:""}},[t._v("支出")])])])]),n("tr",[n("td",[t._v("說明")]),n("td",{attrs:{colspan:"3"}},[n("input",{attrs:{type:"text"}})])])]),n("div",{staticClass:"editButtonGroup"},[n("button",[t._v("新增")]),n("button",[t._v("修改")]),n("button",[t._v("刪除")])]),n("hr"),n("table",{attrs:{border:"1"}},[n("colgroup",[n("col",{staticStyle:{width:"150px"}}),n("col",{staticStyle:{width:"80px"}}),n("col",{staticStyle:{width:"100%"}})]),n("thead",[n("tr",[n("th",[t._v("名稱")]),n("th",[t._v("類型")]),n("th",[t._v("說明")])])])])])},function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",{staticClass:"tabcontent",attrs:{id:"sub-subject"}},[n("table",[n("colgroup",[n("col",{staticStyle:{width:"50px"}}),n("col",{staticStyle:{width:"100%"}})]),n("tr",[n("td",[t._v("主科目")]),n("td",[n("select",[n("option",{attrs:{value:""}},[t._v("伙食費")]),n("option",{attrs:{value:""}},[t._v("交通費")])])])]),n("tr",[n("td",[t._v("名稱")]),n("td",[n("input",{attrs:{type:"text"}})])])]),n("div",{staticClass:"editButtonGroup"},[n("button",[t._v("新增")]),n("button",[t._v("修改")]),n("button",[t._v("刪除")])]),n("hr"),n("table",{attrs:{border:"1"}},[n("colgroup",[n("col",{staticStyle:{width:"150px"}}),n("col",{staticStyle:{width:"100%"}})]),n("thead",[n("tr",[n("th",[t._v("子科目名稱")]),n("th",[t._v("說明")])])])])])}];n.d(e,"a",function(){return a}),n.d(e,"b",function(){return r})},"56d7":function(t,e,n){"use strict";n.r(e);n("cadf"),n("551c"),n("f751"),n("097d");var a=n("2b0e"),r=n("8c4f"),s=n("bc3a"),i=n.n(s),c=n("a7fe"),o=n.n(c),l=n("9f78"),u=n("9f3f"),d=n("bf45"),f=n("8aab");a["a"].use(r["a"]),a["a"].use(o.a,i.a),a["a"].config.productionTip=!1;var v=new r["a"]({routes:[{path:"/query",name:"query",component:u["default"]},{path:"/edit",name:"edit",component:d["default"]},{path:"/settings",name:"settings",component:f["default"]}]});new a["a"]({render:function(t){return t(l["default"])},router:v}).$mount("#app")},"60ef":function(t,e){t.exports={data:function(){return{title:"編輯交易資料",accountType:"[科目類別]"}},created:function(){},methods:{addTxn:function(){},updateTxn:function(){},deleteTxn:function(){}}}},"8aab":function(t,e,n){"use strict";var a=n("55a6"),r=n("cb92"),s=(n("cbe9"),n("2877")),i=Object(s["a"])(r["default"],a["a"],a["b"],!1,null,null,null);e["default"]=i.exports},9600:function(t,e,n){"use strict";var a=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",[n("span",[t._v(t._s(t.title))]),n("br"),n("hr")])},r=[];n.d(e,"a",function(){return a}),n.d(e,"b",function(){return r})},"9d1d":function(t,e){t.exports={data:function(){return{title:"查詢交易紀錄"}},created:function(){this},methods:{}}},"9f3f":function(t,e,n){"use strict";var a=n("9600"),r=n("d68d"),s=n("2877"),i=Object(s["a"])(r["default"],a["a"],a["b"],!1,null,null,null);e["default"]=i.exports},"9f78":function(t,e,n){"use strict";var a=n("cf46"),r=n("e375"),s=(n("e00d"),n("2877")),i=Object(s["a"])(r["default"],a["a"],a["b"],!1,null,null,null);e["default"]=i.exports},a041:function(t,e,n){"use strict";var a=n("2f97"),r=n.n(a);r.a},be8f:function(t,e){t.exports={data:function(){return{mainTitle:"個人帳務管理工具",copyright:"© 2019 陳品均(Chen Jim) 版權所有"}},created:function(){this},methods:{}}},bf45:function(t,e,n){"use strict";var a=n("2c17"),r=n("e8bd"),s=(n("a041"),n("2877")),i=Object(s["a"])(r["default"],a["a"],a["b"],!1,null,null,null);e["default"]=i.exports},cb92:function(t,e,n){"use strict";var a=n("2d50"),r=n.n(a);e["default"]=r.a},cbe9:function(t,e,n){"use strict";var a=n("5438"),r=n.n(a);r.a},cf46:function(t,e,n){"use strict";var a=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",{attrs:{id:"app"}},[n("header",[t._v(t._s(t.mainTitle))]),n("div",{attrs:{id:"center"}},[n("nav",[n("ul",[n("li",[n("router-link",{attrs:{to:"/query"}},[t._v("查詢交易紀錄")])],1),n("li",[n("router-link",{attrs:{to:"/edit"}},[t._v("編輯交易")])],1),n("li",[n("router-link",{attrs:{to:"/settings"}},[t._v("設定")])],1)])]),n("content",[n("router-view")],1)]),n("footer",[t._v(t._s(t.copyright))])])},r=[];n.d(e,"a",function(){return a}),n.d(e,"b",function(){return r})},d68d:function(t,e,n){"use strict";var a=n("9d1d"),r=n.n(a);e["default"]=r.a},e00d:function(t,e,n){"use strict";var a=n("3f6f"),r=n.n(a);r.a},e375:function(t,e,n){"use strict";var a=n("be8f"),r=n.n(a);e["default"]=r.a},e8bd:function(t,e,n){"use strict";var a=n("60ef"),r=n.n(a);e["default"]=r.a}});
//# sourceMappingURL=app.8bb5403b.js.map