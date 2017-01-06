# Vue实例
## 构造器
每个Vue.js应用都是通过构造函数Vue创建一个Vue的*根实例*启动的
```js
var vm = new Vue({
    //选项
})
``` 

实例化Vue时，需要传入一个选项对象，它可以包含数据、模板、关在元素、方法、生命周期钩子等选项  
可以扩展Vue构造器，从而用预定义选项创建可复用的*组件构造器*  
```js
var MyComponent = Vue.extend({
    //扩展选项
})

//所有的'MyComponent'实例都将以预定义的扩展选项被创建
var myComponentInstance = new MyComponent()
```
所有的Vue.js组件都是被扩展的Vue实例。  

## 属性与方法
每个Vue实例都会代理其data对象里所有的属性：
```js
var data = {a:1}
var vm = new Vue({
    data : data    
})

vm.a === data.a // ->true

//设置也会影响到原始数据
vm.a = 2
data.a // -> 2

// 反之亦然
data.a = 3
vm.a // ->3
```

Vue实例暴露了一些有用的实例属性与方法。这些属性与方法都有前缀$，以便于代理的data属性区分。

```js
var data = {a:1}
var vm =new Vue({
    el:'#example',
    data:data
})

vm.$data === data // -> true
vm.$el === document.getElementById('example') // -> true

// $watch 是一个实例方法
vm.$watch('a',function(newVal,oldVal){
    // 这个回调将在'vm.a'改变后调用
})
```

> 注意，不要在实例属性或者回调函数中（如 vm.$watch('a', newVal => this.myMethod())）使用箭头函数。因为箭头函数绑定父上下文，所以 this 不会像预想的一样是 Vue 实例，而是 this.myMethod 未被定义。


