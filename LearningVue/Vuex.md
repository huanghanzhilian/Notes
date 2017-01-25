# Vuex
## State
### 单一状态树
Vuex 使用 单一状态树 ，用一个对象就包含了全部的应用层级状态，一个SPA中有且仅有一个sotre实例。
### 在Vue组件中获得Vuex状态
Vuex的状态储存是响应式的，从store实例中读取状态最简单的放啊就是在计算属性中返回某个状态：
```js
const Counter={
  template:'<div>{{count}}</div>',
  computed:{
    count(){
      return store.state.count
    }
  }
}
```
组件依赖的全局状态(Store)单例，在模块化的构建系统中，在每个需要使用state的组件中需要频繁地导入，并且在测试组件时需要模拟状态。
Vuex通过store选项，提供了一种机制将状态从根组件注入到每一个子组件中（在模块化地构建系统中需调用Vue.use(Vuex)）：
```js
const app=new Vue({
  el:'#app',
  //把store对象提供给"store"选项，这可以把store实例注入所有的子组件
  store,
  components:{Counter},
  template:'<div class="app"><counter></counter></div>'
})
```
在根实例中注测store选项，该store实例会注入到根组件下的所有子组件中，切子组件能通过this.$store访问到。
```js
const Counter={
  template:'<div>{{count}}</div>',
  computed:{
    count(){
      return this.$store.state.count
    }
}
```
### mapState辅助函数
当一个组件需要获取多个状态的时候，声明多个计算属性比较麻烦，可以使用mapState辅助函数来生成计算属性
```js
import { mapState } from 'vuex'
export default{
  //...
  computed:mapState({
    //箭头函数，类似于c#的lambda
    count:state=>state.count,
    //传字符串参数'count'等同于'state=>state.count'
    countAlias:'count',
    //为了能够使用this获取局部状态，必须使用常规函数
    countPlusLocalState(state){
      return state.count+this.localCount
    }
  })
}
```
当映射的计算属性的名称与state的子节点名称相同时，我们也可以给mapState传一个字符串数组
```js
computed:mapState([
  //映射this.count为store.state.count
  'count'
])
```
### 对象展开运算符
mapState函数返回的是一个对象，要将其与局部计算属性混合使用，一般可以通过工具函数将多个对象合并为一个，以将最终对象传给computed属性。还可以使用对象展开运算符`...`来简化写法
```js
computed:{
  localComputed(){/* ... */},
  ...mapState({
    //...
  })
}
```
## Getters
有时候需要从store中的state中派生出一些状态，例如队列表进行过滤并计数：
```js
computed:{
  doneTodosCount(){
    return this.$store.state.todos.fliter(todo=>todo.done).length
  }
}
```
如果多个组件需要用到此属性，要么复制、要么创建共享函数多处导入。在Vuex中我们可以定义getters(可以认为是store的计算属性)。Getters接受state作为其第一个参数：
```js
const store = new Vuex.Store({
  state:{
    todos:[
      {id:1,text:'xxx',done:true},
      {id:2,text:'yyy',done:false}
    ]
  },
  //Getters会暴露为store.getters对象
  getters:{
    doneTodos:state=>{
      return state.todos.filter(todo=>todo.done)
    }
    //Getters也可以接受其他getters作为第二个参数
    doneTodosCount:(state,getters)=>{
      return getters.doneTodos.length
    }
  }
})
```
### mapGetters辅助函数
mapGetters辅助函数仅仅是将store中的getters映射到局部计算属性：
```js
import { mapGetters } from 'vuex'

export default{
  computed:{
    //使用对象展开运算符将getters混入computed对象中
    ...mapGetters([
      'doneTodosCount',
      'anotherGetter',
    ]),
    //如果想将一个getter属性取另一个名字
    mapGetters({
      //映射 this.doneCount 为 store.getters.doneTodosCount
      doneCount:'doneTodosCount'
    })
  }
}
```

## Mutations
更改Vuex的store中的状态的唯一方法是提交mutation，它非常类似事件：每个mutation都有一个字符串的*事件类型(type)*和一个*回调函数(handler)*，我们再回调函数中进行状态更改，并接受state作为第一个参数：
```js
const store = new Vuex.Store({
  state:{
    count:1
  },
  mutations:{
    increment(state){
      state.count++
    }
  }
})
```
要使用store.commit方法来唤醒mutation handler：`store.commit('increment')`
### 提交载荷(Payload)
store.commit可以传入额外的参数，即mutation的载荷(payload):
```js
mutations:{
  increment(state,n){
    state.count+=n
  }
}

store.commit('increment',10)

//载荷可以是一个对象，包含多个字段的mutation会更易读
mutations:{
  increment(state,payload){
    state.count+=payload.amount
  }
}

store.commit('increment',{
  amount:10
})

//对象风格的提交方式

store.commit({
  //使用包含type属性的对象
  type: 'increment',
  amount: 10
})

//使用对象风格的提交时，整个对象都作为载荷传给mutation函数，handler保持不变
mutations: {
  increment (state, payload) {
    state.count += payload.amount
  }
}
```

### Mutations需遵守Vue的响应规则
当store中的状态变更时，见识状态的Vue组件也会自动更新。所以Vuex中的mutation也需要与使用Vue一张遵守一些注意事项：
1. 最好提前在你的store中初始化好所有所需属性
2. 当需要在对象上添加新属性时，你应该
    * 使用Vue.set(obj,'newProp',123)
    * 或者使用对象展开运算符`state.obj={...state.obj,newProp:123}`

### 使用常量替代Mutation事件类型
使用常量代替mutation事件类型可以使linter之类的工具发挥作用，同时把这些常量放在单独的文件中可以让团队对整个app中包含的mutation一目了然：
```js
// mutation-types.js
export const SOME_MUTATION = 'SOME_MUTATION'

// store.js
import Vuex from 'vuex'
import { SOME_MUTATION } from './mutation-types'

const store = new Vuex.Store({
  state: { ... },
  mutations: {
    // 我们可以使用 ES2015 风格的计算属性命名功能来使用一个常量作为函数名
    [SOME_MUTATION] (state) {
      // mutate state
    }
  }
})
```

### mutation必须使同步函数
```js
mutations: {
  someMutation (state) {
    api.callAsyncMethod(() => {
      state.count++
    })
  }
}
```
如果使用异步函数，当mutation触发时，回调函数还没有被调用，在回调函数中进行的状态改变时不可追踪的

### 在组件中提交Mutations
可以在组件中使用 `this.$store.commit('xxx')` 提交 mutation，或者使用 `mapMutations` 辅助函数将组件中的 methods 映射为 store.commit 调用（需要在根节点注入 store）。
```js
import { mapMutations } from 'vuex'

export default {
  // ...
  methods: {
    ...mapMutations([
      'increment' // 映射 this.increment() 为 this.$store.commit('increment')
    ]),
    ...mapMutations({
      add: 'increment' // 映射 this.add() 为 this.$store.commit('increment')
    })
  }
}
```