# 列表渲染
## v-for
用 v-for 指令根据一组数组的选项列表进行渲染。 v-for 指令需要以 item in items 形式的特殊语法， items 是源数据数组并且 item 是数组元素迭代的别名。

### 基本用法
在 v-for 块中，我们拥有对父作用域属性的完全访问权限。 v-for 还支持一个可选的第二个参数为当前项的索引。
```html
<ul id="example-2">
  <li v-for="(item, index) in items">
    {{ parentMessage }} - {{ index }} - {{ item.message }}
  </li>
</ul>
```    
### 对象迭代v-for
可以用 v-for 通过一个对象的属性来迭代，可以提供第二个的参数为键名，第三个参数为索引
```html
<div v-for="(value, key, index) in object">
  {{ index }}. {{ key }} : {{ value }}
</div>
```

### 整数迭代v-for
```html
<div>
  <!--将重复多次模板-->
  <span v-for="n in 10">{{ n }}</span>
  <!--渲染为-->
  <span>1</span>
  <span>2</span>
  <span>3</span>
  <!-- ... -->
  <span>10</span>
</div>
```

### 组件和v-for
在自定义组件里，你可以像任何普通元素一样用 v-for 。但是不能自动传递数据到组件里，因为组件有自己独立的作用域。为了传递迭代数据到组件里，我们要用 props，使用v-bind来绑定props。
```html
<div id="todo-list-example">
  <input
    v-model="newTodoText"
    v-on:keyup.enter="addNewTodo"
    placeholder="Add a todo"
  >
  <ul>
    <li
      is="todo-item"
      v-for="(todo, index) in todos"
      v-bind:title="todo"
      v-on:remove="todos.splice(index, 1)"
    ></li>
  </ul>
</div>
<script>
//注册组件
Vue.component('todo-item', {
  template: '\
    <li>\
      {{ title }}\
      <button v-on:click="$emit(\'remove\')">X</button>\
    </li>\
  ',
  props: ['title']
})
new Vue({
  el: '#todo-list-example',
  data: {
    newTodoText: '',
    todos: [
      'Do the dishes',
      'Take out the trash',
      'Mow the lawn'
    ]
  },
  methods: {
    addNewTodo: function () {
      this.todos.push(this.newTodoText)
      this.newTodoText = ''
    }
  }
})
</script>
```

## key
为了给 Vue 一个提示，以便它能跟踪每个节点的身份，从而重用和重新排序现有元素，你需要为每项提供一个唯一 key 属性。理想的 key 值是每项都有唯一 id。这个特殊的属性相当于 Vue 1.x 的 track-by ，但它的工作方式类似于一个属性，所以你需要用 v-bind 来绑定动态值
```html
<div v-for="item in items" :key="item.id">
  <!-- 内容 -->
</div>
```
建议尽可能使用 v-for 来提供 key ，除非迭代 DOM 内容足够简单。

## 数组更新检测
### 变异方法
变异方法会改变被这些方法调用的原始数组，将会触发视图更新，这些方法有：`push()\pop()\shift()\unshift()\splice()\sort()\reverse()`
### 重塑数组
`filter()\concat()\slice()`方法不会改变原始数组，但总是返回新数组替换旧数组`vm.items=vm.items.filter(function(item){return item.message.math(/Foo/)})`
### 注意事项
Vue不能检测一下两种情况的数组变动
1. 利用索引直接设置一个项，e.g. `vm.items[i]=newValue`
2. 修改数组长度，e.g. `vm.items.length=newLength`

解决方法如下
```js
//第一种情况
//Vue.set
Vue.set(vm.items,i,newValue);
//Array.prototype.splice
vm.items.splice(indexOfItem,1,newValue)

//第二种情况
vm.items.splice(newLength)
```

## 显示过滤/排序结果
想要显示一个数组的过滤或排序副本，而不实际改变或重置原始数据。在这种情况下，可以创建返回过滤或排序数组的计算属性。
```html
<li v-for="n in evenNumbers">{{n}}</li>
<script>
  //...
  data:{
    numbers:[1,2,3,4,5]
  },
  computed:{
    evenNumbers:function(){
      return this.numbers.filter(function(number){return number%2 === 0})
    }
  }
</script>
```
也可以在计算属性不适用的情况下 (例如，在嵌套 v-for 循环中) 使用 method 方法
```html
<li v-for="n in even(numbers)">{{n}}</li>
<script>
  //...
  data:{
    numbers:[1,2,3,4,5]
  },
  computed:{
    methods:function(numbers){
      return this.numbers.filter(function(number){return number%2 === 0})
    }
  }
</script>
```