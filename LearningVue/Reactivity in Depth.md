# 深入响应时原理
## 如何追踪变化
data选项中的对象的属性被转为getter/setter，每个组件实例有一个相应的watcher实例，当setter被调用时，watcher重新计算，更新关联组件
## 变化检测问题
vue不能检测到对象属性的添加或删除，只有在初始化时才会将属性转化为getter/setter，所以只有data对象中存在的属性才是响应的。
```js
var vm = new Vue({
  data:{
  a:1
  }
})
// `vm.a` 是响应的
vm.b = 2
// `vm.b` 是非响应的
```
Vue 不允许在已经创建的实例上动态添加新的根级响应式属性(root-level reactive property)。然而它可以使用 Vue.set(object, key, value) 方法将响应属性添加到嵌套的对象上：`Vue.set(vm.someObject, 'b', 2)`，还可以使用 vm.$set 实例方法，这也是全局 Vue.set 方法的别名：`this.$set(this.someObject,'b',2)`  
想向已有对象上添加一些属性，例如使用 Object.assign() 或 _.extend() 方法来添加属性。但是，添加到对象上的新属性不会触发更新。在这种情况下可以创建一个新的对象，让它包含原对象的属性和新的属性：`this.someObject = Object.assign({}, this.someObject, { a: 1, b: 2 })`  
## 异步更新队列
Vue 异步执行 DOM 更新。只要观察到数据变化，Vue 将开启一个队列，并缓冲在同一事件循环中发生的所有数据改变。如果同一个 watcher 被多次触发，只会一次推入到队列中。这种在缓冲时去除重复数据对于避免不必要的计算和 DOM 操作上非常重要。然后，在下一个的事件循环“tick”中，Vue 刷新队列并执行实际（已去重的）工作。Vue 在内部尝试对异步队列使用原生的 Promise.then 和 MutationObserver，如果执行环境不支持，会采用 setTimeout(fn, 0) 代替。
为了在数据变化之后等待 Vue 完成更新 DOM ，可以在数据变化之后立即使用 Vue.nextTick(callback) 。这样回调函数在 DOM 更新完成后就会调用。
```html
<div id="example">{{message}}</div>
<script>
var vm = new Vue({
  el: '#example',
  data: {
    message: '123'
  }
})
vm.message = 'new message' // 更改数据
vm.$el.textContent === 'new message' // false
Vue.nextTick(function () {
  vm.$el.textContent === 'new message' // true
})
//在组件内使用 vm.$nextTick() 实例方法特别方便，因为它不需要全局 Vue ，并且回调函数中的 this 将自动绑定到当前的 Vue 实例上
this.$nextTick(function () {
  this.$el.textContent === 'new message' // true
})
</script>
```