# Class与Style绑定
## 绑定HTML Class
### 对象语法
我们可传给v-bind:class 一个对象，以动态地切换class`<div :class="{ active:isActive,'text-danger':hasError }"></div>`，也可以直接绑定一个对象
```html
<div v-bind:class="classObject"></div>
<script>
data: {
  classObject: {
    active: true,
    'text-danger': false
  }
}
//也可是绑定计算属性
data: {
  isActive: true,
  error: null
},
computed: {
  classObject: function () {
    return {
      active: this.isActive && !this.error,
      'text-danger': this.error && this.error.type === 'fatal',
    }
  }
}
</script>
```
### 数组语法
可以绑定一个数组来应用一个class列表`<div :class="[activeClass,errClass]">`，想根据条件判断也可以使用三元表达式`<div :class="[isActive?activeClass:'',errorClass]">`，也可以在数组语法中使用对象语法`<div :class="[{active:isActive},errorClass]">`

### 用在组件上
在一个定制的组件上用到class属性的时候，这些类将被添加到根元素上，这个元素上已经存在的类不会被覆盖。
```html
<script>
    Vue.component('my-component',{
        template:'<p class="foo bar">Hi</p>'
    })
</script>
<my-component clas="baz boo"></my-component>
<!--渲染为-->
<p class="foo bar baz boo">Hi</p>
```

## 绑定内联样式
### 对象语法
v-bind:style 的对象语法十分直观——看着非常像 CSS ，其实它是一个 JavaScript 对象。 CSS 属性名可以用驼峰式（camelCase）或短横分隔命名（kebab-case）
```html
<div v-bind:style="{ color: activeColor, fontSize: fontSize + 'px' }"></div>
```
### 数组语法
v-bind:style 的数组语法可以将多个样式对象应用到一个元素上
```html
<div v-bind:style="[baseStyles, overridingStyles]">
```