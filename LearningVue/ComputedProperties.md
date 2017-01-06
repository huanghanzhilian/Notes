# 计算属性
## 计算属性
在模板中绑定表达式是非常便利的，但是它们实际上只用于简单的操作。在模板中放入太多的逻辑会让模板过重且难以维护。这时应该使用计算属性
### 基础例子
```html
<div id="example">
    <p>Original message:"{{message}}"</p>
    <p>Computed reversed message:"{{reversedMessage}}"</p>
</div>
```
```js
var vm=new Vue({
    el:'#example',
    data:{
        message:'Hello'
    },
    computed:{
        reversedMessage:function(){
            return this.message.split('').reverse().join('')
        }
    }
})
```
### 计算缓存 vs Methods
不经过计算属性，我们可以在 method 中定义一个相同的函数来替代它。对于最终的结果，两种方式确实是相同的。然而，不同的是计算属性是基于它的依赖缓存。计算属性只有在它的相关依赖发生改变时才会重新取值。这就意味着只要 message 没有发生改变，多次访问 reversedMessage 计算属性会立即返回之前的计算结果，而不必再次执行函数。
如果不希望有缓存，可用 method 替代。

### 计算属性 vs Watched Property
Vue.js 提供了一个方法 $watch ，它用于观察 Vue 实例上的数据变动。

### 计算setter
计算属性默认只有getter，不过在需要时你也可以提供一个setter：
```js
//...
computed:{
    fullName:{
        get:function(){
            return this.firstName + " " + this.lastName
        },
        set:function(newValue){
            var names=newValue.split(' ')
            this.firstName=names[0]
            this.lastName=names[names.length-1]
        }
    }
}
```

## 观察 Watchers
Vue提供一个通用的方法通过watch选项来响应数据的变化。当在数据变化响应时，执行异步操作或开销较大的操作，这是很有用的。