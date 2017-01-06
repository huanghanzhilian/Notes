# 模板语法
## 差值
### 文本
最常见的是使用"Mustache"语法的文本插值，插值双向绑定
```html
<span>Message:{{msg}}</span>
```

通过使用v-once指令，能执行一次性插值，数据改变时，插值处的内容不会更新
```html
<span v-once>This will never change:{{msg}}</span>
```

### 纯HTML
双大括号会将数据解释为纯文本，而非HTML。要输出HTML，需要使用v-html指令
```html
<div v-html="rawHtml"></div>
```
被插入的内容都会被当做 HTML —— 数据绑定会被忽略。所以不能使用v-html开符合局部模板，组件更适合担任UI重用与复用的基本单元

> 你的站点上动态渲染的任意 HTML 可能会非常危险，因为它很容易导致 XSS 攻击。请只对可信内容使用 HTML 插值，绝不要对用户提供的内容插值。

### 属性
Mustache不能在HTML属性中使用，应使用v-bind指令：
```html
<div v-bind:id="dynamicId"></div>
```

### 使用javaScript表达式
```html
{{ number + 1 }}
{{ ok ? 'YES' : 'NO' }}
{{ message.split('').reverse().join('') }}
<div v-bind:id="'list-' + id"></div>

<!-- 这是语句，不是表达式 -->
{{ var a = 1 }}
<!-- 流控制也不会生效，请使用三元表达式 -->
{{ if (ok) { return message } }}
```

> 模板表达式都被放在沙盒中，只能访问全局变量的一个白名单，如 Math 和 Date 。你不应该在模板表达式中试图访问用户定义的全局变量。

## 指令
指令（Directives）是带有 v- 前缀的特殊属性。指令属性的值预期是单一 JavaScript 表达式。指令的职责就是当其表达式的值改变时相应地将某些行为应用到 DOM 上。
### 参数
一些指令能接受一个“参数”，在指令后以冒号指明。例如， v-bind 指令被用来响应地更新 HTML 属性；v-on 指令用于监听 DOM 事件
### 修饰符
修饰符（Modifiers）是以半角句号 . 指明的特殊后缀，用于指出一个指定应该以特殊方式绑定。

## 过滤器
Vue.js允许自定义过滤器，被用作一些常见的文本格式化。过滤器应该被添加在mustache插值的尾部，由“管道符”指示：
```html
<!-- in mustache -->
{{ message | capitalize }}
<!-- in v-bind -->
<div v-bind:id="rawId | formatId"></div>
```

> Vue 2.x 中，过滤器只能在 mustache 绑定和 v-bind 表达式（从 2.1.0 开始支持）中使用，因为过滤器设计目的就是用于文本转换。为了在其他指令中实现更复杂的数据变换，你应该使用计算属性。

过滤器函数总是接受表达式的值作为第一个参数
```js
new Vue({
  // ...
  filters: {
    capitalize: function (value) {
      if (!value) return ''
      value = value.toString()
      return value.charAt(0).toUpperCase() + value.slice(1)
    }
  }
})
```
过滤器可以串联：`{{ message | filterA | filterB }}`  
过滤器是JavaScript函数，因此可以接受参数：`{{message | filterA('arg1',arg2) }}`，字符串arg1将传给过滤器作为第二个参数，arg2表达式的值将在被求值之后传给过滤器作为第三个参数。

## 缩写
```html
<!-- 完整语法 -->
<a v-bind:href="url"></a>

<!-- 缩写 -->
<a :href="url"></a>

<!-- 完整语法 -->
<a v-on:click="doSomeThing"></a>

<!-- 缩写 -->
<a @click="doSomeThing"></a>
```
