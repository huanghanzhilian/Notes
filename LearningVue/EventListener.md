# 事件处理器
## 内联处理器方法
如果需要在内联语句处理器中访问原生 DOM 事件，可以用特殊变量 $event 把它传入方法
```html
<button v-on:click="warn('Form cannot be submitted yet.', $event)">Submit</button>

<script>
//...
methods: {
  warn: function (message, event) {
    // 现在我们可以访问原生事件对象
    if (event) event.preventDefault()
    alert(message)
  }
}
</script>
```

## 事件修饰符
在事件处理程序中调用 event.preventDefault() 或 event.stopPropagation() 是非常常见的需求。尽管我们可以在 methods 中轻松实现这点，但更好的方式是：methods 只有纯粹的数据逻辑，而不是去处理 DOM 事件细节。
Vue.js 为 v-on 提供了 事件修饰符。通过由点(.)表示的指令后缀来调用修饰符。`.stop\.prevent\.capture\.self\.once`
```html
<!-- 阻止单击事件冒泡 -->
<a v-on:click.stop="doThis"></a>
<!-- 提交事件不再重载页面 -->
<form v-on:submit.prevent="onSubmit"></form>
<!-- 修饰符可以串联  -->
<a v-on:click.stop.prevent="doThat"></a>
<!-- 只有修饰符 -->
<form v-on:submit.prevent></form>
<!-- 添加事件侦听器时使用事件捕获模式 -->
<div v-on:click.capture="doThis">...</div>
<!-- 只当事件在该元素本身（而不是子元素）触发时触发回调 -->
<div v-on:click.self="doThat">...</div>
<!-- 只会触发一次 -->
<a v-on:click.once="doThis"></a>
```

## 按键修饰符
```html
<!-- 只有在 keyCode 是 13 时调用 vm.submit() -->
<input v-on:keyup.13="submit">
<!-- Vue 为最常用的按键提供了别名 -->
<!-- 同上 -->
<input v-on:keyup.enter="submit">
<!-- 缩写语法 -->
<input @keyup.enter="submit">
<!-- Alt + C -->
<input @keyup.alt.67="clear">
<!-- Ctrl + Click -->
<div @click.ctrl="doSomething">Do something</div>
```
全部的按键别名
`.enter\.tab\.delete(backspace&delete)\.esc\.space\.up\.down\.left\.right\.ctrl\.alt\.shift\.meta`
另外还可以通过全局`config.keyCodes`对象自定义修饰符的别名`Vue.config.keyCodes.f1=112`