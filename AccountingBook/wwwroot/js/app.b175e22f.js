(function(t){function e(e){for(var r,u,o=e[0],c=e[1],s=e[2],l=0,d=[];l<o.length;l++)u=o[l],a[u]&&d.push(a[u][0]),a[u]=0;for(r in c)Object.prototype.hasOwnProperty.call(c,r)&&(t[r]=c[r]);f&&f(e);while(d.length)d.shift()();return i.push.apply(i,s||[]),n()}function n(){for(var t,e=0;e<i.length;e++){for(var n=i[e],r=!0,o=1;o<n.length;o++){var c=n[o];0!==a[c]&&(r=!1)}r&&(i.splice(e--,1),t=u(u.s=n[0]))}return t}var r={},a={app:0},i=[];function u(e){if(r[e])return r[e].exports;var n=r[e]={i:e,l:!1,exports:{}};return t[e].call(n.exports,n,n.exports,u),n.l=!0,n.exports}u.m=t,u.c=r,u.d=function(t,e,n){u.o(t,e)||Object.defineProperty(t,e,{enumerable:!0,get:n})},u.r=function(t){"undefined"!==typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(t,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(t,"__esModule",{value:!0})},u.t=function(t,e){if(1&e&&(t=u(t)),8&e)return t;if(4&e&&"object"===typeof t&&t&&t.__esModule)return t;var n=Object.create(null);if(u.r(n),Object.defineProperty(n,"default",{enumerable:!0,value:t}),2&e&&"string"!=typeof t)for(var r in t)u.d(n,r,function(e){return t[e]}.bind(null,r));return n},u.n=function(t){var e=t&&t.__esModule?function(){return t["default"]}:function(){return t};return u.d(e,"a",e),e},u.o=function(t,e){return Object.prototype.hasOwnProperty.call(t,e)},u.p="/";var o=window["webpackJsonp"]=window["webpackJsonp"]||[],c=o.push.bind(o);o.push=e,o=o.slice();for(var s=0;s<o.length;s++)e(o[s]);var f=c;i.push([0,"chunk-vendors"]),n()})({0:function(t,e,n){t.exports=n("56d7")},"2d50":function(t,e){var n;t.exports={data:function(){return{title:""}},created:function(){n=this,n.title="設定"},methods:{}}},"2f97":function(t,e,n){},"34f0":function(t,e,n){"use strict";var r=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",[n("span",[t._v(t._s(t.title))]),n("hr"),t._m(0)])},a=[function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",{attrs:{id:"flexBox"}},[n("fieldset",{attrs:{id:"main"}},[n("legend",[t._v("主項目")]),n("div",{staticClass:"mainItem",attrs:{id:"transactionInfo"}},[n("table",{attrs:{border:"1"}},[n("colgroup",[n("col",{attrs:{id:"info_col1"}}),n("col",{attrs:{id:"info_col2"}}),n("col",{attrs:{id:"info_col3"}}),n("col",{attrs:{id:"info_col4"}})]),n("tr",[n("td",[t._v("交易代碼")]),n("td",[n("input",{attrs:{type:"text",id:""}})]),n("td",[t._v("交易日期")]),n("td",[n("input",{attrs:{type:"date"}})])]),n("tr",[n("td",[t._v("說明")]),n("td",{attrs:{colspan:"3"}},[n("input",{attrs:{type:"text",id:"remark"}})])])])]),n("div",{staticClass:"mainItem",attrs:{id:"editButtons"}},[n("input",{attrs:{type:"button",value:"新增"}}),n("input",{attrs:{type:"button",value:"更新"}}),n("input",{attrs:{type:"button",value:"刪除"}})])]),n("fieldset",{staticClass:"detial"},[n("legend",{attrs:{align:"center"}},[t._v("借方")])]),n("fieldset",{staticClass:"detial"},[n("legend",{attrs:{align:"center"}},[t._v("貸方")])])])}];n.d(e,"a",function(){return r}),n.d(e,"b",function(){return a})},"3f6f":function(t,e,n){},"56d7":function(t,e,n){"use strict";n.r(e);n("cadf"),n("551c"),n("f751"),n("097d");var r=n("2b0e"),a=n("8c4f"),i=n("9f78"),u=n("9f3f"),o=n("bf45"),c=n("8aab");r["a"].use(a["a"]),r["a"].config.productionTip=!1;var s=new a["a"]({routes:[{path:"/query",component:u["default"]},{path:"/edit",component:o["default"]},{path:"/settings",component:c["default"]}]});new r["a"]({render:function(t){return t(i["default"])},router:s}).$mount("#app")},"5a6a":function(t,e,n){"use strict";var r=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",{attrs:{id:"app"}},[n("header",[t._v("\n        "+t._s(t.mainTitle)+"\n    ")]),n("div",{attrs:{id:"center"}},[n("nav",[n("ul",[n("li",[n("router-link",{attrs:{to:"/query"}},[t._v("查詢交易紀錄")])],1),n("li",[n("router-link",{attrs:{to:"/edit"}},[t._v("編輯交易")])],1),n("li",[n("router-link",{attrs:{to:"/settings"}},[t._v("設定")])],1)])]),n("main",[n("router-view")],1)]),n("footer",[t._v("\n        "+t._s(t.copyright)+"\n    ")])])},a=[];n.d(e,"a",function(){return r}),n.d(e,"b",function(){return a})},"60ef":function(t,e){t.exports={data:function(){return{title:"編輯交易資料"}},created:function(){this},methods:{}}},"8aab":function(t,e,n){"use strict";var r=n("bba5"),a=n("cb92"),i=n("2877"),u=Object(i["a"])(a["default"],r["a"],r["b"],!1,null,null,null);e["default"]=u.exports},9600:function(t,e,n){"use strict";var r=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",[n("span",[t._v(t._s(t.title))]),n("br"),n("hr")])},a=[];n.d(e,"a",function(){return r}),n.d(e,"b",function(){return a})},"9d1d":function(t,e){t.exports={data:function(){return{title:"查詢交易紀錄"}},created:function(){this},methods:{}}},"9f3f":function(t,e,n){"use strict";var r=n("9600"),a=n("d68d"),i=n("2877"),u=Object(i["a"])(a["default"],r["a"],r["b"],!1,null,null,null);e["default"]=u.exports},"9f78":function(t,e,n){"use strict";var r=n("5a6a"),a=n("e375"),i=(n("e00d"),n("2877")),u=Object(i["a"])(a["default"],r["a"],r["b"],!1,null,null,null);e["default"]=u.exports},a041:function(t,e,n){"use strict";var r=n("2f97"),a=n.n(r);a.a},bba5:function(t,e,n){"use strict";var r=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",[n("span",[t._v(t._s(t.title))]),n("br"),n("hr")])},a=[];n.d(e,"a",function(){return r}),n.d(e,"b",function(){return a})},be8f:function(t,e){t.exports={data:function(){return{mainTitle:"個人帳務管理工具",copyright:"© 2019 陳品均(Chen Jim) 版權所有"}},created:function(){this},methods:{}}},bf45:function(t,e,n){"use strict";var r=n("34f0"),a=n("e8bd"),i=(n("a041"),n("2877")),u=Object(i["a"])(a["default"],r["a"],r["b"],!1,null,null,null);e["default"]=u.exports},cb92:function(t,e,n){"use strict";var r=n("2d50"),a=n.n(r);e["default"]=a.a},d68d:function(t,e,n){"use strict";var r=n("9d1d"),a=n.n(r);e["default"]=a.a},e00d:function(t,e,n){"use strict";var r=n("3f6f"),a=n.n(r);a.a},e375:function(t,e,n){"use strict";var r=n("be8f"),a=n.n(r);e["default"]=a.a},e8bd:function(t,e,n){"use strict";var r=n("60ef"),a=n.n(r);e["default"]=a.a}});
//# sourceMappingURL=app.b175e22f.js.map