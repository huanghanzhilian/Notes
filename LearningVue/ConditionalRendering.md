# 条件渲染
## 使用key控制元素的可重用
Vue 尝试尽可能高效的渲染元素，通常会复用已有元素而不是从头开始渲染。这么做除了使 Vue 更快之外还可以得到一些好处。
```html
<template v-if="loginType === 'username'">
  <label>Username</label>
  <input placeholder="Enter your username">
</template>
<template v-else>
  <label>Email</label>
  <input placeholder="Enter your email address">
</template>
```
在代码中切换 loginType 不会删除用户已经输入的内容，两个模版由于使用了相同的元素，<input> 会被复用，仅仅是替换了他们的 placeholder。  
Vue 提供一种方式让你可以自己决定是否要复用元素。你要做的是添加一个属性 key ，key 必须带有唯一的值。
```html
<template v-if="loginType === 'username'">
  <label>Username</label>
  <input placeholder="Enter your username" key="username-input">
</template>
<template v-else>
  <label>Email</label>
  <input placeholder="Enter your email address" key="email-input">
</template>
```

## v-if VS v-show
v-if 是真实的条件渲染，因为它会确保条件块在切换当中适当地销毁与重建条件块内的事件监听器和子组件。  
v-if 也是惰性的：如果在初始渲染时条件为假，则什么也不做——在条件第一次变为真时才开始局部编译（编译会被缓存起来）。  
相比之下， v-show 简单得多——元素始终被编译并保留，只是简单地基于 CSS 切换。  
一般来说， v-if 有更高的切换消耗而 v-show 有更高的初始渲染消耗。因此，如果需要频繁切换使用 v-show 较好，如果在运行时条件不大可能改变则使用 v-if 较好。