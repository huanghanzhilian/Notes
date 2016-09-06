# 第12章 委托和Lambda表达式
一种常见的模式是向方法传递对象，该方法再调用对象的一个方法，这种情况下对象的作用只是向最终被调用的方法传递一个引用。但不是每次传递一个方法时都需要定义新对象，所以我们可以使用委托这个特殊类，它允许像处理其他数据那样处理对方法的引用。
C# 2.0支持用匿名方法这一种不太优雅的语法来创建自定义委托，在C# 3.0之后加入了Lambda表达式，我们应该使用Lambda表达式来创建自定义委托。
## 12.1 委托概述
* 委托的内部机制
  委托实际是特殊的类。.NET中的委托类型总是派生自System.MulticastDelegate，后者派生自System.Delegate。其中两个成员分别是方法信息MethodInfo和对象实例Target。
  所有委托都是不可变的，委托一旦创建好就无法更改。
## 12.2 Lambda表达式
有时为了能转换成委托类型，方法签名比方法主题还要冗长，为了解决这个问题C#2.0引入了匿名方法，C#3.0引入了Lambda表达式，两种语法统称为匿名函数。
Lambda表达式本身划分为两种类型：语句Lambda和表达式Lambda。
Lambda表达式的目的是在需要哦基于很简单的方法生成委托时，避免声明权限成员的麻烦。

* 语句Lambda
  语句Lambda由形参列表，后跟Lambda操作符=>，然后跟一个代码块构成
```c#
(int x,int y)=>{return x>y;}
//只要编译器能从Lambda表达式所转换成的委托推断出类型，所有Lambda表达式都不需要显式声明参数类型。
(x,y)=>{return x>y;}
```
> 考虑在Lambda形参列表中省略类型，只要类型对于读者是显而易见的，或者是无关紧要的细节。

* 表达式Lambda
  表达式Lambda只有要返回的表达式，完全没有语句块
```c#
(x,y)=>x>y
```
## 12.3 匿名方法
匿名方法必须显式指定每个参数的类型，而且必须有一个语句块。要在参数列表前添加关键字delegate，以强调匿名方法必须转换成一个委托类型。
```c#
delegate(int first,int second){
  return first < second;
}
```

* 无参数的匿名方法
  函数主题中不使用任何参数，而委托类型只要求“值”参数时允许彻底省略参数列表。
```c#
delegate { return Console.Realine() == ""; }
```
## 12.4 通用的委托：System.Func和System.Action
Func系列表达式表示有返回值的方法，Action系列代表返回void的方法
Func委托的最后一个类型参数总是委托的返回类型
> **规范**
> 考虑定义自己的委托类型对于可读性的提升是否比使用预定义泛型委托类型所带来的便利性来的重要。

* 委托没有结构相等性
  不能将某个委托类型的对象引用转换成不相关的委托类型，即使这两个委托类型的形参和返回类型完全一致。
  可以创建新委托，让它引用旧委托的Invoke方法。
```c#
TestDel d = new TestDel(Vec.Test);
Func<string, string, string> f = new Vec().d.Invoke;
```

* 捕捉循环变量
  在c#5.0之前Lambda表达式捕捉变量总是使用其最新的值，而不是捕捉并保留变量在委托创建时的值。
```c#
//在c#5.0之前扑捉循环变量的解决方案
var items=new string[]{"Moe","larry"，"Curly"};
var actions = new List<Action>();
foreach(string item in items){
  //每次循环有新变量，委托捕捉的就是不同的变量了
  string _item = item;
  actions.Add(()=>{Console.Write(_item);})
}
foreach(Action action in actions){
  action();
}
```
> 避免在匿名函数中捕捉循环变量

* 表达式树
  表达式树也是一个对象，允许传递编译器对Lambda主体的分析。
  解析表达式树
  [解析表达式树](E:\code\电子书\笔记\c%23本质论笔记\Lambda表达式解析.cs)