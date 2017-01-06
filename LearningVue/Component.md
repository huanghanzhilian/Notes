# 组件
## 什么是组件
组件（Component）是 Vue.js 最强大的功能之一。组件可以扩展 HTML 元素，封装可重用的代码。在较高层面上，组件是自定义元素， Vue.js 的编译器为它添加特殊功能。在有些情况下，组件也可以是原生 HTML 元素的形式，以 is 特性扩展。

## 使用组件
```html
<!DOCTYPE html>
<html>
<head>
	<title>Component</title>
</head>
<body>
<div id="div1">
	<my-component></my-component>
	<!--无法使用，不会被渲染-->
	<my-second-component></my-second-component>
</div>
<div id="div2">
	<my-second-component></my-second-component>
</div>
<div id="div3">
	<table style="border: 1px solid">
		<!-- table标签中不允许my-row标签，渲染时会出错 -->
		<my-row></my-row>
		<!-- 使用is属性来解决 -->
		<tr is='my-row'></tr>
	</table>
	<!-- 使用x-template、JavaScript内联模板字符串和.vue组件时没有这样的限制，推荐使用字符串模板 -->
</div>
<div id="div4">
	<simple-counter></simple-counter>
  	<simple-counter></simple-counter>
  	<simple-counter></simple-counter>
</div>
</body>
<script src="Scripts/vue.js"></script>
<script type="text/javascript">
	//全局注册
	//先注册，再使用
	Vue.component('my-component',{
		template:'<div>A custom component!</div>'
	});
	var vm1=new Vue({
		el:'#div1'
	});
	//局部注册
	var vm2=new Vue({
		el:'#div2',
		components:{
			'my-second-component':{
				template:'<div>Another custom component!</div>'
			}
		}
	});
	var vm3=new Vue({
		el:'#div3',
		components:{
			'my-row':{
				template:'<tr><td>1</td><td>2</td></tr>'
			}
		}
	});

	var data = { counter: 0 }
	Vue.component('simple-counter', {
  		template: '<button v-on:click="counter += 1">{{ counter }}</button>',
  	// data 是一个函数，因此 Vue 不会警告，
  	
  	data: function () {
  			// 但是我们为每一个组件返回了同一个对象引用
    		//return data

    		//应改为
    		return {
    			counter:0
    		}
  		}
	})

	var vm4=new Vue({
		el:'#div4'
	});

	
</script>
</html>
```

### 构成组件
在 Vue.js 中，父子组件的关系可以总结为 props down, events up 。父组件通过 props 向下传递数据给子组件，子组件通过 events 给父组件发送消息。

## Prop
组件实例的作用域是孤立的。这意味着不能并且不应该在子组件的模板内直接引用父组件的数据。可以使用 props 把数据传给子组件。
prop 是父组件用来传递数据的一个自定义属性。子组件需要显式地用 props 选项声明 “prop”：
```html
<!-- HTML 特性不区分大小写。当使用非字符串模版时，prop的名字形式会从 camelCase 转为 kebab-case（短横线隔开） -->
<child my-message="hello!"></child>
<script>
Vue.component('child', {
  // 声明 props
  props: ['myMessage'],
  // 就像 data 一样，prop 可以用在模板内
  // 同样也可以在 vm 实例中像 “this.myMessage” 这样使用
  template: '<span>{{ myMessage }}</span>'
})
</script>
```
### 动态Prop
类似于用 v-bind 绑定 HTML 特性到一个表达式，也可以用 v-bind 动态绑定 props 的值到父组件的数据中。每当父组件的数据变化时，该变化也会传导给子组件：
```html
<div>
  <input v-model="parentMsg">
  <br>
  <child v-bind:my-message="parentMsg"></child>
</div>
```
### 字面量语法vs动态语法
```html
<!-- 传递了一个字符串"1" -->
<comp some-prop="1"></comp>
<!-- 传递实际的数字 -->
<comp v-bind:some-prop="1"></comp>
```
### 单向数据流
prop 是单向绑定的：当父组件的属性变化时，将传导给子组件，但是不会反过来。这是为了防止子组件无意修改了父组件的状态——这会让应用的数据流难以理解。  
通常有两种改变 prop 的情况：
1. prop 作为初始值传入，子组件之后只是将它的初始值作为本地数据的初始值使用；
    ```js
    //定义一个局部 data 属性，并将 prop 的初始值作为局部数据的初始值。
    props: ['initialCounter'],
    data: function () {
        return { counter: this.initialCounter }
    }
    ```
2. prop 作为需要被转变的原始值传入。
    ```js
    //定义一个 computed 属性，此属性从 prop 的值计算得出。
    props: ['size'],
    computed: {
        normalizedSize: function () {
            return this.size.trim().toLowerCase()
        }
    }
    ```
### prop验证
组件可以为 props 指定验证要求。如果未指定验证要求，Vue 会发出警告。当组件给其他人使用时这很有用。
```js
Vue.component('example', {
  props: {
    // 基础类型检测 （`null` 意思是任何类型都可以）
    propA: Number,
    // 多种类型
    propB: [String, Number],
    // 必传且是字符串
    propC: {
      type: String,
      required: true
    },
    // 数字，有默认值
    propD: {
      type: Number,
      default: 100
    },
    // 数组／对象的默认值应当由一个工厂函数返回
    propE: {
      type: Object,
      default: function () {
        return { message: 'hello' }
      }
    },
    // 自定义验证函数
    propF: {
      validator: function (value) {
        return value > 10
      }
    }
  }
})
```

## 自定义事件

父组件使用 props 传递数据给子组件，子组件要把数据传递回去，使用自定义事件

### 使用v-on绑定自定义事件
* 使用`$on(eventName)`监听事件
* 使用`$emit(eventName)`触发事件

```html
<div id="div5">
	<p>{{total}}</p>
	<button-counter @increment="incrementTotal"></button-counter>
	<button-counter @increment="incrementTotal"></button-counter>
</div>
<script>
    Vue.component('button-counter',{
		template:'<button @click="increment">{{counter}}</button>',
		data:function(){return{counter:0}},
		methods:{
			increment:function(){
				this.counter+=1
				this.$emit('increment')
			}
		}
	});
	var vm5=new Vue({
		el:'#div5',
		data:{total:0},
		methods:{
			incrementTotal:function(){
				this.total+=1;
			}
		}
	})
</script>
```
*给组件绑定原生事件*
<my-component v-on:click.native="doTheThing"></my-component>

### 使用自定义事件的表单输入组件
自定义事件也可以用来创建自定义的表单输入组件，使用 v-model 来进行数据双向绑定。
`<input v-model="something">`仅仅是一个语法糖，相当于`<input v-bind:value="something" v-on:input="something = $event.target.value">`
e.g.
```html
<div id="app">
  <currency-input 
    label="Price" 
    v-model="price"
  ></currency-input>
  <currency-input 
    label="Shipping" 
    v-model="shipping"
  ></currency-input>
  <currency-input 
    label="Handling" 
    v-model="handling"
  ></currency-input>
  <currency-input 
    label="Discount" 
    v-model="discount"
  ></currency-input>
  
  <p>Total: ${{ total }}</p>
</div>

<script>
Vue.component('currency-input', {
  template: '\
    <div>\
      <label v-if="label">{{ label }}</label>\
      $\
      <input\
        ref="input"\
        v-bind:value="value"\
        v-on:input="updateValue($event.target.value)"\
        v-on:focus="selectAll"\
        v-on:blur="formatValue"\
      >\
    </div>\
  ',
  props: {
    value: {
      type: Number,
      default: 0
    },
    label: {
      type: String,
      default: ''
    }
  },
  mounted: function () {
    this.formatValue()
  },
  methods: {
    updateValue: function (value) {
      var result = currencyValidator.parse(value, this.value)
      if (result.warning) {
        this.$refs.input.value = result.value
      }
      this.$emit('input', result.value)
    },
    formatValue: function () {
      this.$refs.input.value = currencyValidator.format(this.value)
    },
    selectAll: function (event) {
      // Workaround for Safari bug
      // http://stackoverflow.com/questions/1269722/selecting-text-on-focus-using-jquery-not-working-in-safari-and-chrome
      setTimeout(function () {
      	event.target.select()
      }, 0)
    }
  }
})

new Vue({
  el: '#app',
  data: {
    price: 0,
    shipping: 0,
    handling: 0,
    discount: 0
  },
  computed: {
    total: function () {
      return ((
        this.price * 100 + 
        this.shipping * 100 + 
        this.handling * 100 - 
        this.discount * 100
      ) / 100).toFixed(2)
    }
  }
})
</script>
```

### 非父子组件通信
有时候非父子关系的组件也需要通信。在简单的场景下，使用一个空的 Vue 实例作为中央事件总线：
```js
var bus = new Vue()
// 触发组件 A 中的事件
bus.$emit('id-selected', 1)
// 在组件 B 创建的钩子中监听事件
bus.$on('id-selected', function (id) {
  // ...
})
```
复杂的情况下，考虑使用vuex

## 使用Slot分发内容
为了让组件可以组合，我们需要一种方式来混合父组件的内容与子组件自己的模板。这个过程被称为 内容分发 (或 “transclusion” 如果你熟悉 Angular)。Vue.js 实现了一个内容分发 API ，参照了当前 Web 组件规范草案，使用特殊的 <slot> 元素作为原始内容的插槽。
### 编译作用域
```html
<child-component>
  {{ message }}
</child-component>
```
message绑定到父组件的数据，组件作用域简单地说是：
父组件模板的内容在父组件作用域内编译；子组件模板的内容在子组件作用域内编译。
`<child-component v-show="someChildProperty"></child-component>`无效，因为在父组件中不知道子组件的状态，判断应在在子组件的模板内进行
```js
Vue.component('child-component', {
  // 有效，因为是在正确的作用域内
  template: '<div v-show="someChildProperty">Child</div>',
  data: function () {
    return {
      someChildProperty: true
    }
  }
})
```

### 单个slot
除非子组件模板包含至少一个 <slot> 插口，否则父组件的内容将会被丢弃。当子组件模板只有一个没有属性的 slot 时，父组件整个内容片段将插入到 slot 所在的 DOM 位置，并替换掉 slot 标签本身。
最初在 <slot> 标签中的任何内容都被视为备用内容。备用内容在子组件的作用域内编译，并且只有在宿主元素为空，且没有要插入的内容时才显示备用内容。
```html
<!-- my-component模板 -->
<div>
	<h2>我是子组件的标题</h2>
	<slot>只有在没有要分发的内容时才会显示</slot>
</div>
<!-- 父组件模板 -->
<div>
	<h1>我是父组件的标题</h1>
	<my-component>
		<p>这是一些初始内容</p>
		<p>这是更多的初始内容</p>
	</my-component>
</div>
<!-- 渲染结果 -->
<div>
	<h1>我是父组件的标题</h1>
	<div>
		<h2>我是子组件的标题</h2>
		<p>这是一些初始内容</p>
		<p>这是更多的初始内容</p>
	</div>
</div>
```

### 具名Slot
<slot> 元素可以用一个特殊的属性 name 来配置如何分发内容。多个 slot 可以有不同的名字。具名 slot 将匹配内容片段中有对应 slot 特性的元素。
仍然可以有一个匿名 slot ，它是默认 slot ，作为找不到匹配的内容片段的备用插槽。如果没有默认的 slot ，这些找不到匹配的内容片段将被抛弃。
```html
<!-- app-layout -->
<div class="container">
	<header>
		<slot name="header"></slot>
	</header>
	<main>
		<slot></slot>
	</main>
	<footer>
		<slot name="footer"></slot>
	</footer>
</div>
<!-- 父组件模板 -->
<app-layout>
	<h1 slot="header">这里可能是一个页面标题</h1>
	<p>主要内容的一个段落</p>
	<p>另一个主要段落</p>
	<p slot="footer">这里有一些联系信息</p>
</app-layout>
<!-- 渲染结果 -->
<div class="container">
	<header>
		<h1>这里可能是一个页面标题</h1>
	</header>
	<main>
		<p>主要内容的一个段落</p>
		<p>另一个主要段落</p>
	</main>
	<footer>
		<p>这里有一些联系信息</p>
	</footer>
</div>
```

### 作用域插槽
作用域插槽是一种特殊类型的插槽，用作使用一个（能够传递数据到）可重用模板替换已渲染元素。
在子组件中，只需将数据传递到插槽，就像你将 prop 传递给组件一样
```html
<div class="child">
  <slot text="hello from child"></slot>
</div>
```
在父级中，具有特殊属性 scope 的 <template> 元素，表示它是作用域插槽的模板。scope 的值对应一个临时变量名，此变量接收从子组件中传递的 prop 对象
```html
<div class="parent">
  <child>
    <template scope="props">
      <span>hello from parent</span>
      <span>{{ props.text }}</span>
    </template>
  </child>
</div>
```
渲染结果
```html
<div class="parent">
  <div class="child">
    <span>hello from parent</span>
    <span>hello from child</span>
  </div>
</div>
```            
作用域插槽更具代表性的用例是列表组件，允许组件自定义应该如何渲染列表每一项：
```html
<my-awesome-list :items="items">
  <!-- 作用域插槽也可以在这里命名 -->
  <template slot="item" scope="props">
    <li class="my-fancy-item">{{ props.text }}</li>
  </template>
</my-awesome-list>

<ul>
  <slot name="item"
    v-for="item in items"
    :text="item.text">
    <!-- fallback content here -->
  </slot>
</ul>
```

## 动态组件
多个组件可以使用同一个挂载点，然后动态地在它们之间切换。使用保留的 <component> 元素，动态地绑定到它的 is 特性：
```html
<script>
var vm = new Vue({
  el: '#example',
  data: {
    currentView: 'home'
  },
  components: {
    home: { /* ... */ },
    posts: { /* ... */ },
    archive: { /* ... */ }
  }
})
</script>

<component v-bind:is="currentView">
  <!-- 组件在 vm.currentview 变化时改变！ -->
</component>
```
也可以直接绑定到组件对象上
```js
var Home = {
  template: '<p>Welcome home!</p>'
}
var vm = new Vue({
  el: '#example',
  data: {
    currentView: Home
  }
})
```

### keep-alive
如果把切换出去的组件保留在内存中，可以保留它的状态或避免重新渲染。为此可以添加一个 keep-alive 指令参数：
```html
<keep-alive>
  <component :is="currentView">
    <!-- 非活动组件将被缓存！ -->
  </component>
</keep-alive>
```

## 杂项
### 编写可复用组件
一次性组件跟其它组件紧密耦合没关系，但是可复用组件应当定义一个清晰的公开接口。  
Vue 组件的 API 来自三部分 - props, events 和 slots ：
* Props 允许外部环境传递数据给组件
* Events 允许组件触发外部环境的副作用
* Slots 允许外部环境将额外的内容组合在组件中。

```html
<my-component
  :foo="baz"
  :bar="qux"
  @event-a="doThis"
  @event-b="doThat"
>
  <img slot="icon" src="...">
  <p slot="main-text">Hello!</p>
</my-component>
```

### 子组件索引
尽管有 props 和 events ，但是有时仍然需要在 JavaScript 中直接访问子组件。为此可以使用 ref 为子组件指定一个索引 ID 。
```html
<div id="parent">
  <user-profile ref="profile"></user-profile>
</div>
<script>
var parent = new Vue({ el: '#parent' })
// 访问子组件
var child = parent.$refs.profile
</script>
```
当 ref 和 v-for 一起使用时， ref 是一个数组或对象，包含相应的子组件。

> $refs 只在组件渲染完成后才填充，并且它是非响应式的。它仅仅作为一个直接访问子组件的应急方案——应当避免在模版或计算属性中使用 $refs 。

### 异步组件
在大型应用中，我们可能需要将应用拆分为多个小模块，按需从服务器下载。为了让事情更简单， Vue.js 允许将组件定义为一个工厂函数，动态地解析组件的定义。Vue.js 只在组件需要渲染时触发工厂函数，并且把结果缓存起来，用于后面的再次渲染。例如：
```js
Vue.component('async-example', function (resolve, reject) {
  setTimeout(function () {
    // Pass the component definition to the resolve callback
    resolve({
      template: '<div>I am async!</div>'
    })
  }, 1000)
})
```
工厂函数接收一个 resolve 回调，在收到从服务器下载的组件定义时调用。也可以调用 reject(reason) 指示加载失败。这里 setTimeout 只是为了演示。怎么获取组件完全由你决定。推荐配合使用 ：Webpack 的代码分割功能：
```js
Vue.component('async-webpack-example', function (resolve) {
  // 这个特殊的 require 语法告诉 webpack
  // 自动将编译后的代码分割成不同的块，
  // 这些块将通过 Ajax 请求自动下载。
  require(['./my-async-component'], resolve)
})
```
可以使用 Webpack 2 + ES2015 的语法返回一个 Promise resolve 函数：
```js
Vue.component(
  'async-webpack-example',
  () => System.import('./my-async-component')
)
```

### 组件命名约定
当注册组件（或者 props）时，可以使用 kebab-case ，camelCase ，或 TitleCase 。Vue 不关心这个。
```js
// 在组件定义中
components: {
  // 使用 kebab-case 形式注册
  'kebab-cased-component': { /* ... */ },
  // register using camelCase
  'camelCasedComponent': { /* ... */ },
  // register using TitleCase
  'TitleCasedComponent': { /* ... */ }
}
```
在 HTML 模版中，请使用 kebab-case 形式`<kebab-cased-component></kebab-cased-component>`
当使用字符串模式时，可以不受 HTML 的 case-insensitive 限制。这意味实际上在模版中，你可以使用 camelCase 、 TitleCase 或者 kebab-case 来引用：
```html
<!-- 在字符串模版中可以用任何你喜欢的方式! -->
<my-component></my-component>
<myComponent></myComponent>
<MyComponent></MyComponent>
```

### 递归组件
组件在它的模板内可以递归地调用自己，不过，只有当它有 name 选项时才可以，当你利用Vue.component全局注册了一个组件, 全局的ID作为组件的 name 选项，被自动设置。

### 组件间循环引用
```html
<!-- 树 -->
<!-- tree-folder component -->
<p>
  <span>{{ folder.name }}</span>
  <tree-folder-contents :children="folder.children"/>
</p>

<!-- tree-folder-contents component -->
<ul>
  <li v-for="child in children">
    <tree-folder v-if="child.children" :folder="child"/>
    <span v-else>{{ child.name }}</span>
  </li>
</ul>
```
如上`tree-folder`依赖`tree-folder-contents`，同时`tree-folder-contents`依赖`tree-folder`，使用Vue.component注册时这没有什么问题，但是如果你通过Webpack等组件系统导入的话会报错，它们无法解析先导入那个组件。在上述案例中，我们知道子组件是`tree-folder-contents`,为了解决这个问题可以在beforeCreate这个生命周期钩子中注册它
```js
beforeCreate: function () {
  this.$options.components.TreeFolderContents = require('./tree-folder-contents.vue')
}
```

### 内联模板
如果子组件有 inline-template 特性，组件将把它的内容当作它的模板，而不是把它当作分发内容。这让模板更灵活。
```html
<my-component inline-template>
  <div>
    <p>These are compiled as the component's own template.</p>
    <p>Not parent's transclusion content.</p>
  </div>
</my-component>
```
但是 inline-template 让模板的作用域难以理解。最佳实践是使用 template 选项在组件内定义模板或者在 .vue 文件中使用 template 元素。

### x-template
另一种定义模版的方式是在 JavaScript 标签里使用 text/x-template 类型，并且指定一个id。例如：
```html
<script type="text/x-template" id="hello-world-template">
  <p>Hello hello hello</p>
</script>
<script>
Vue.component('hello-world', {
  template: '#hello-world-template'
})
</script>
```

### 使用 v-once 的低级静态组件(Cheap Static Component)

当组件中包含大量静态内容时，可以考虑使用 v-once 将渲染结果缓存起来
```js
Vue.component('terms-of-service', {
  template: '\
    <div v-once>\
      <h1>Terms of Service</h1>\
      ... a lot of static content ...\
    </div>\
  '
})
```