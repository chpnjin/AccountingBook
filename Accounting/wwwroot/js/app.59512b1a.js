(function(t){function e(e){for(var r,i,o=e[0],c=e[1],f=e[2],s=0,d=[];s<o.length;s++)i=o[s],u[i]&&d.push(u[i][0]),u[i]=0;for(r in c)Object.prototype.hasOwnProperty.call(c,r)&&(t[r]=c[r]);l&&l(e);while(d.length)d.shift()();return a.push.apply(a,f||[]),n()}function n(){for(var t,e=0;e<a.length;e++){for(var n=a[e],r=!0,o=1;o<n.length;o++){var c=n[o];0!==u[c]&&(r=!1)}r&&(a.splice(e--,1),t=i(i.s=n[0]))}return t}var r={},u={app:0},a=[];function i(e){if(r[e])return r[e].exports;var n=r[e]={i:e,l:!1,exports:{}};return t[e].call(n.exports,n,n.exports,i),n.l=!0,n.exports}i.m=t,i.c=r,i.d=function(t,e,n){i.o(t,e)||Object.defineProperty(t,e,{enumerable:!0,get:n})},i.r=function(t){"undefined"!==typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(t,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(t,"__esModule",{value:!0})},i.t=function(t,e){if(1&e&&(t=i(t)),8&e)return t;if(4&e&&"object"===typeof t&&t&&t.__esModule)return t;var n=Object.create(null);if(i.r(n),Object.defineProperty(n,"default",{enumerable:!0,value:t}),2&e&&"string"!=typeof t)for(var r in t)i.d(n,r,function(e){return t[e]}.bind(null,r));return n},i.n=function(t){var e=t&&t.__esModule?function(){return t["default"]}:function(){return t};return i.d(e,"a",e),e},i.o=function(t,e){return Object.prototype.hasOwnProperty.call(t,e)},i.p="/";var o=window["webpackJsonp"]=window["webpackJsonp"]||[],c=o.push.bind(o);o.push=e,o=o.slice();for(var f=0;f<o.length;f++)e(o[f]);var l=c;a.push([0,"chunk-vendors"]),n()})({0:function(t,e,n){t.exports=n("56d7")},"2d50":function(t,e){var n;t.exports={data:function(){return{title:""}},created:function(){n=this,n.title="設定"},methods:{}}},"2f97":function(t,e,n){},"3f6f":function(t,e,n){},"56d7":function(t,e,n){"use strict";n.r(e);n("cadf"),n("551c"),n("f751"),n("097d");var r=n("2b0e"),u=n("8c4f"),a=n("9f78"),i=n("9f3f"),o=n("bf45"),c=n("8aab");r["a"].use(u["a"]),r["a"].config.productionTip=!1;var f=new u["a"]({routes:[{path:"/query",component:i["default"]},{path:"/edit",component:o["default"]},{path:"/settings",component:c["default"]}]});new r["a"]({render:function(t){return t(a["default"])},router:f}).$mount("#app")},"60ef":function(t,e){t.exports={data:function(){return{title:"編輯交易資料"}},created:function(){this},methods:{}}},"8aab":function(t,e,n){"use strict";var r=n("bba5"),u=n("cb92"),a=n("2877"),i=Object(a["a"])(u["default"],r["a"],r["b"],!1,null,null,null);e["default"]=i.exports},9600:function(t,e,n){"use strict";var r=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",[n("span",[t._v(t._s(t.title))]),n("br"),n("hr")])},u=[];n.d(e,"a",function(){return r}),n.d(e,"b",function(){return u})},"9d1d":function(t,e){t.exports={data:function(){return{title:"查詢交易紀錄"}},created:function(){this},methods:{}}},"9f3f":function(t,e,n){"use strict";var r=n("9600"),u=n("d68d"),a=n("2877"),i=Object(a["a"])(u["default"],r["a"],r["b"],!1,null,null,null);e["default"]=i.exports},"9f78":function(t,e,n){"use strict";var r=n("b6a4"),u=n("e375"),a=(n("e00d"),n("2877")),i=Object(a["a"])(u["default"],r["a"],r["b"],!1,null,null,null);e["default"]=i.exports},a041:function(t,e,n){"use strict";var r=n("2f97"),u=n.n(r);u.a},b6a4:function(t,e,n){"use strict";var r=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",{attrs:{id:"app"}},[n("header",[t._v("\n        "+t._s(t.mainTitle)+"\n    ")]),n("div",{attrs:{id:"center"}},[n("nav",[n("ul",[n("li",[n("router-link",{attrs:{to:"/query"}},[t._v("查詢交易紀錄")])],1),n("li",[n("router-link",{attrs:{to:"/edit"}},[t._v("編輯交易")])],1),n("li",[n("router-link",{attrs:{to:"/settings"}},[t._v("設定")])],1)])]),n("main",[n("router-view")],1)]),n("footer",[t._v("\n        "+t._s(t.copyright)+"\n    ")])])},u=[];n.d(e,"a",function(){return r}),n.d(e,"b",function(){return u})},bba5:function(t,e,n){"use strict";var r=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",[n("span",[t._v(t._s(t.title))]),n("br"),n("hr")])},u=[];n.d(e,"a",function(){return r}),n.d(e,"b",function(){return u})},be8f:function(t,e){t.exports={data:function(){return{mainTitle:"個人帳務管理工具",copyright:"© 2019 陳品均(Chen Jim) 版權所有"}},created:function(){selfData=this},methods:{}}},bf45:function(t,e,n){"use strict";var r=n("cc97"),u=n("e8bd"),a=(n("a041"),n("2877")),i=Object(a["a"])(u["default"],r["a"],r["b"],!1,null,null,null);e["default"]=i.exports},cb92:function(t,e,n){"use strict";var r=n("2d50"),u=n.n(r);e["default"]=u.a},cc97:function(t,e,n){"use strict";var r=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",[n("span",[t._v(t._s(t.title))]),n("hr"),t._m(0)])},u=[function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",{attrs:{id:"flexBox"}},[n("fieldset",{attrs:{id:"main"}},[n("legend",[t._v("主項目")]),n("table",{attrs:{border:"1"}},[n("colgroup",[n("col"),n("col")]),n("tr",[n("td",[t._v("交易代碼")]),n("td",[n("input",{attrs:{type:"text",id:""}})])]),n("tr",[n("td",[t._v("交易日期")]),n("td",[n("input",{attrs:{type:"date"}})])]),n("tr",[n("td",[t._v("說明")]),n("td",[n("input",{attrs:{type:"text"}})])])])]),n("fieldset",{staticClass:"detial"},[n("legend",{attrs:{align:"center"}},[t._v("借方")])]),n("fieldset",{staticClass:"detial"},[n("legend",{attrs:{align:"center"}},[t._v("貸方")])])])}];n.d(e,"a",function(){return r}),n.d(e,"b",function(){return u})},d68d:function(t,e,n){"use strict";var r=n("9d1d"),u=n.n(r);e["default"]=u.a},e00d:function(t,e,n){"use strict";var r=n("3f6f"),u=n.n(r);u.a},e375:function(t,e,n){"use strict";var r=n("be8f"),u=n.n(r);e["default"]=u.a},e8bd:function(t,e,n){"use strict";var r=n("60ef"),u=n.n(r);e["default"]=u.a}});
//# sourceMappingURL=app.59512b1a.js.map