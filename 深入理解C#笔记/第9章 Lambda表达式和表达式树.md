# 第9章 Lambda表达式和表达式树
## 9.1 作为委托的Lambda表达式
Lambda表达式大多数时候都和一个返回非void的委托类型配合使用——如果不返回一个结果，语法就不像现在这样一目了然。在LINQ中，委托通常被视为数据管道的一部分，接受输入并返回结果来表示投影的值，或者判断某项是否符合当前的过滤条件，等等。
Lambda表达式最冗长的形式是：
```
( 显式类型的参数列表 ) => {语句}
(string text) => { return text.Length; };
```
大多数时候，都可以用一个表达式来表示整个主体，该表达式的值是Lambda的结果。
```
( 显式类型的参数列表 ) => 表达式
(string text) => text.Length
```
编译器大多数时候都能猜出参数类型，不需要你显式声明它们。在这些情况下，可以将Lambda表达式写成：
```
( 隐式类型的参数列表 ) => 表达式
(text) => text.Length
```
如果Lambda表达式只需一个参数，而且那个参数可以隐式指定类型，C# 3就允许省略圆括号。这种格式的Lambda表达式是：
```
参数名 => 表达式
text => text.Length
```
> 高阶(Higher-order)函数
> Lambda表达式的主体本身可以包含另一个Lambda表达式，但做起来就像听起来一样，很容易使人混淆。另外，Lambda表达式的参数可以是另一个委托，这样做同样很乱。这两种情况都是高阶函数的例子。

## 9.2 使用List<T>和事件的简单例子
### 列表的过滤、排序和操作
```c#
class Film
{
    public string Name { get; set; }
    public int Year { get; set; }
}

var films = new List<Film>
{
    new Film { Name = "Jaws", Year = 1975 },
    new Film { Name = "Singing in the Rain", Year = 1952 },
    new Film { Name = "Some like it Hot", Year = 1959 },
    new Film { Name = "The Wizard of Oz", Year = 1939 },
    new Film { Name = "It's a Wonderful Life", Year = 1946 },
    new Film { Name = "American Beauty", Year = 1999 },
    new Film { Name = "High Fidelity", Year = 2000 },
    new Film { Name = "The Usual Suspects", Year = 1995 }
};
Action<Film> print =
     film => Console.WriteLine("Name={0}, Year={1}",   //❶ 创建可用的列表打印委托
                                film.Name, film.Year);
films.ForEach(print);     //❷ 打印原始列表

films.FindAll(film => film.Year < 1960)    //❸ 创建过滤过的列表
     .ForEach(print);

films.Sort((f1, f2) => f1.Name.CompareTo(f2.Name));     //❹ 对原始列表排序
films.ForEach(print);
```
### 在事件处理程序中进行记录
```c#
static void Log(string title, object sender, EventArgs e)
{
    Console.WriteLine("Event: {0}", title);
    Console.WriteLine(" Sender: {0}", sender);
    Console.WriteLine(" Arguments: {0}", e.GetType());
    foreach (PropertyDescriptor prop in
             TypeDescriptor.GetProperties(e))
    {
        string name = prop.DisplayName;
        object value = prop.GetValue(e);
         Console.WriteLine(" {0}={1}", name, value);
     }
}

Button button = new Button { Text = "Click me" };
button.Click += (src, e) => Log("Click", src, e);
button.KeyPress += (src, e) => Log("KeyPress", src, e);
button.MouseClick += (src, e) => Log("MouseClick", src, e);

Form form = new Form { AutoSize = true, Controls = { button } };
Application.Run(form);
```
## 9.3 表达式树
.NET 3.5的表达式树提供了一种抽象的方式将一些代码表示成一个对象树。它类似于CodeDOM，但是在一个稍高的级别上操作。表达式树主要用于LINQ。
### 以编程方式构建表达式树
表达式树是对象构成的树，树中每个节点本身都是一个表达式。不同的表达式类型代表能在代码中执行的不同操作：二元操作（例如加法），一元操作（例如获取一个数组的长度），方法调用，构造函数调用，等等。
Expression类包括两个属性：
* Type属性代表表达式求值后的.NET类型，可把它视为一个返回类型。例如，如果一个表达式要获取一个字符串的Length属性，该表达式的类型就是int。
* NodeType属性返回所代表的表达式的种类。它是ExpressionType枚举的成员，包括LessThan、Multiply和Invoke等。仍然使用上面的例子，对于myString.Length这个属性访问来说，其节点类型是MemberAccess。

xpression有许多派生类，其中一些可能有多个不同的节点类型。例如，BinaryExpression就代表了具有两个操作数的任意操作：算术、逻辑、比较、数组索引，等等。这正是NodeType属性重要的地方，因为它能区分由相同的类表示的不同种类的表达式。
```c#
Expression firstArg = Expression.Constant(2);
Expression secondArg = Expression.Constant(3);
Expression add = Expression.Add(firstArg, secondArg);

Console.WriteLine(add);
//输出为 (2+3)
```
值得注意的是，“叶”表达式在代码中是最先创建的：你自下而上构建了这些表达式。这是由“表达式不易变”这一事实决定的——创建好表达式后，它就永远不会改变。这样就可以随心所欲地缓存和重用表达式。
### 将表达式树编译成委托
LambdaExpression是从Expression派生的类型之一。泛型类Expression<TDelegate>又是从LambdaExpression派生的。
Expression和Expression<TDelegate>类的区别在于，泛型类以静态类型的方式标识了它是什么种类的表达式，也就是说，它确定了返回类型和参数。
LambdaExpression有一个Compile方法能创建恰当类型的委托。Expression<TDelegate>也有一个同名的方法，但它静态类型化后返回TDelegate类型的委托。该委托现在可以采用普通方式执行，就好像它是用一个普通方法或者其他方式来创建的一样。
```c#
Expression firstArg = Expression.Constant(2);
Expression secondArg = Expression.Constant(3);
Expression add = Expression.Add(firstArg, secondArg);

Func<int> compiled = Expression.Lambda<Func<int>>(add).Compile();
Console.WriteLine(compiled());
```
### 将C# Lambda表达式转换成表达式树
Lambda表达式能显式或隐式地转换成恰当的委托实例。然而，这并非唯一能进行的转换。还可以要求编译器通过你的Lambda表达式构建一个表达式树，在执行时创建Expression<TDelegate>的一个实例。
```c#
//用Lambda表达式创建表达式树
Expression<Func<int>> return5 = () => 5;
Func<int> compiled = return5.Compile();
Console.WriteLine(compiled());
```
> 并非**所有**Lambda表达式都能转换成表达式树。不能将带有一个语句块(即使只有一个return语句)的Lambda转换成表达式树——只有对单个表达式求值的Lambda才可以。表达式中还不能包含赋值操作，因为在表达式树中表示不了这种操作。尽管.NET 4扩展了表达式树的功能，但只能转换单一表达式这一限制仍然有效。

```c#
//一个较为复杂的表达式树
Expression<Func<string, string, bool>> expression =
    (x, y) => x.StartsWith(y);

var compiled = expression.Compile();

Console.WriteLine(compiled("First", "Second"));
Console.WriteLine(compiled("First", "Fir"));
// 用代码构造表达式树
MethodInfo method = typeof(string).GetMethod   /*❶ 构造方法调用的各个部件*/
    ("StartsWith", new[] { typeof(string) });
var target = Expression.Parameter(typeof(string), "x");
var methodArg = Expression.Parameter(typeof(string), "y");
Expression[] methodArgs = new[] { methodArg };

Expression call = Expression.Call(target, method, methodArgs);  //❷ 从以上部件创建callExpression

var lambdaParameters = new[] { target, methodArg };   /*❸ 将call转换成Lambda表达式*/
var lambda = Expression.Lambda<Func>
    (call, lambdaParameters);

var compiled = lambda.Compile();

Console.WriteLine(compiled("First", "Second"));
Console.WriteLine(compiled("First", "Fir"));
```
### 位于LINQ核心的表达式树
Lambda表达式提供了编译时检查的能力，而表达式树可以将执行模型从你所需的逻辑中提取出来。
“进程外”LINQ提供器的中心思想在于，我们可以从一个熟悉的源语言（如C#）生成一个表达式树，将结果作为一个中间格式，再将其转换成目标平台上的本地语言，比如SQL。
### LINQ之外的表达式树
1. 优化动态语言运行时
   表达式树是动态语言运行时架构的核心部分。它们具有三个特点对DLR特别有吸引力：
   * 它们是不易变的，因此可以安全地缓存；
   * 它们是可组合的，因此可以在简单的块中构建出复杂的行为；
   * 它们可以编译为委托，后者可以像平常那样进一步JIT编译为本地代码。

   DLR需要对如何处理不同的表达式做出决定，这些表达式会因不同的规则而发生改变。表达式树允许将这些规则（和结果）转换为代码，这与你知道所有的规则和结果后，手工编写代码非常接近。这一概念异常强大，可以使动态代码以惊人的速度执行。
2. 可以放心地对成员的引用进行重构
   可以使用Lambda表达式构建一个代表属性引用的表达式树。方法随后会分析该表达式树，找出你要的属性。然后，它就可以根据这些信息，进行你想要的处理。当然你还可以将表达式树编译为委托，并直接使用。
   ```serializationContext.AddProperty(x => x.BirthDate);```


3. 更简单的反射
   算术操作符与泛型不能很好地结合使用，例如，你很难编写通用的代码将多个值相加。Marc Gravell使用表达式树实现了一个泛型的Operator类和一个非泛型的辅助类，可以用来编写下面这样的代码：
   ```c#
   T runningTotal = initialValue;
   foreach (T item in values)
   {
    runningTotal = Operator.Add(runningTotal, item);
   }
   ```

## 9.4 类型推断和重载决策的改变
> **检查Lambda表达式的主体**　
> Lambda表达式的主体只有在输入参数的类型已知之后才能进行检查。如果x是一个数组或字符串，那么Lambda表达式x =>x.Length就是有效的，但在其他许多情况下它是无效的。当参数类型是显式声明的时候，这并不是一个问题，但对于一个隐式（类型的）参数列表，编译器就必须等待，直到它执行了相应的类型推断之后，才能尝试去理解Lambda表达式的含义。

* 匿名函数（匿名方法和Lambda表达式）的返回类型是根据所有return语句的类型来推断的；
* Lambda表达式要想被编译器理解，所有参数的类型必须为已知；
* 类型推断不要求根据不同的（方法）实参推断出的类型参数的类型完全一致，只要推断出来的结果是兼容的就好；
* 类型推断现在分阶段进行，为一个匿名函数推断的返回类型可作为另一个匿名函数的参数类型使用；
* 涉及匿名函数时，为了找出“最好”的重载方法，要将推断的返回类型考虑在内。
