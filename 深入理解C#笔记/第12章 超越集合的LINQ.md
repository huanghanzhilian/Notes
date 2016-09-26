# 第12章 超越集合的LINQ
## 12.2 用IQueryable和IQueryProvider进行转换
### IQueryable<T>和相关接口的介绍
IQueryable<T>从IEnumerable<T>和非泛型的IQueryable继承而来，而IQueryable又继承于非泛型的IEnumerable。
IQueryable仅有3个属性：QueryProvider、ElementType和Expression。QueryProvider属性是IQueryProvider类型
理解IQueryable的最简单方式就是，把它看作一个查询，在执行的时候，将会生成结果序列。从LINQ的角度看，由于是通过IQueryable的Expression属性返回结果，所以查询的详细信息就保存于表达式树中。一个查询进行执行，就是开始遍历IQueryable的过程（换句话说，即调用GetEnumerator方法，然后对其结果调用MoveNext方法），或者调用IQueryProvider上的Execute方法并传递表达式树。

IQueryProvider用来完成查询，还用它做更多的事情——能用它构建一个更大的查询，这就是LINQ中标准查询操作符的用途。为了构建一个查询，我们需要在相关的IQueryProvider上使用CreateQuery方法。
可把数据源看作是简单的查询（例如，用SQL编写的SELECT * FROM SomeTable）——调用Where、Select、OrderBy及类似方法会生成不同的查询，具体生成的查询是什么取决于第一个关键字。给定任何IQueryable查询后，你可通过执行如下步骤来创建新的查询：
1. 请求现有查询的查询表达式树（使用Expression属性）；
2. 构建一个新的表达式树，包含最初的表达式和你想要的额外功能（例如，过滤、投影或排序）；
3. 请求现有查询的查询提供器（使用Provider属性）；
4. 调用提供器的CreateQuery方法，传递新表达式树。

### 把表达式粘合在一起：Queryable的扩展方法
正如Enumerable类型包含着关于IEnumerable<T>的扩展方法来实现LINQ标准查询操作符一样，Queryable类型包含着关于IQueryable<T>的扩展方法。IEnumerable<T>和Queryable的实现之间有两个巨大的区别。
首先，Enumerable的方法都使用委托作为参数，但对于在别处执行查询的LINQ提供器来说，需要能执行更详细检查的格式——表达式树。
Enumerable和Queryable之间的第2个重大差别是，Enumerable的扩展方法会完成与对应查询操作符相关的实际工作（至少会构建完成这些工作的迭代器）。Queryable中的查询操作符的“实现”做的事情非常少：它们仅仅基于参数创建一个新的查询，或在查询提供器上调用Execute。换句话说，它们只用来构建查询和要执行的请求——不包含操作符背后的逻辑。这意味着，它们可用于任何使用表达式树的LINQ提供器，但是它们单独使用时没有任何意义。它们是代码和提供器细节之间的黏合剂。
### 模拟实际运行的查询提供器
GetEnumerator只在最后才调用，而不在任何中间查询中调用，并且在GetEnumerator被调用的时候，我们已经有了出现在原始查询表达式中的所有信息。
为什么需要IQueryProvider的Execute方法。原因是，并不是所有的查询表达式都生成序列——如果你使用Sum、Count或Average这样的聚合操作符，就不再真正创建一个“数据源”——会立刻对结果进行计算。这个时候Execute就会被调用。

## 12.3 LINQ友好的API和LINQ to XML
### LINQ to XML中的核心类型
* XName表示元素和特性的名称。创建实例时，通常使用字符串的隐式转换（这时不需要使用命名空间）或重载的+(XNamespace, string)操作符
* XNamespace表示XML命名空间，通常是一个URI。创建其实例时常常使用字符串的隐式转换
* XObject是XNode和XAttribute的共同父类：与在DOM API中不同，在LINQ to XML中特性不是节点。如果某方法返回子节点的元素，这里面是不包含特性的
* XNode表示XML树中的节点。它定义了各种用于操作和查询树的成员。XNode还有很多子类没有在图12-4中列出，如XComment和XDeclaration。它们相对来说并不常用，文档、元素和文本才是最常用的节点类型
* XAttribute表示包含名/值对的特性。值从本质上来说是文本，但可以显式转换成其他数据类型，如int和DateTime
* XContainer是XML树中可以包含子内容（主要为元素或文档）的节点
* XText表示文本节点，其派生类XCData表示CDATA文本节点。（CDATA节点大致相当于逐字的字符串字面量，不需要任何转义。）XText很少直接在用户代码中实例化，当将字符串用于元素或文档的内容时，会将其转换为XText实例
* XElement表示元素。它和XAttribute是LINQ to XML中最常用的类。与在DOM API中不同，在创建一个XElement时，不需要创建包含它的文档。如果你不是确实需要一个文档对象（如自定义XML声明），只用元素就可以了
* XDocument表示文档。可以通过Root属性访问其根元素，相当于XmlDocument.DocumentElement。如前所述，你并不总是需要创建一个文档
```c#
//构建元素
XElement noNamespace = new XElement("no-namespace");
//构建带有命名空间的元素
XNamespace ns = "http://csharpindepth.com/sample/namespace";
XElement withNamespace = new XElement(ns + "in-namespace");
```
### 声明式构造
在DOM API中，通常创建一个元素，然后向其中添加内容。在LINQ to XML中使用继承自XContainer的Add方法来实现。XContainer.Add()方法的实现使用了内容模型，签名为Add(object)。XElement（和XDocument）的构造函数签名也使用了同样的模式。在名称之后，可以什么都不指定（创建空元素），也可以指定一个对象（创建包含单个子节点的元素），或对象数组（创建包含多个子节点的元素）。在创建多个子节点的时候，使用了参数数组（C#中的params关键字）。
在创建内容时，不管是通过构造函数还是Add方法，都要考虑以下几点：
* 空引用会被忽略。
* XNode和XAttribute实例可以直接添加。如果它们已经有了父元素，将会被复制，但除此之外不需要任何转换。（编译器会执行一些完整性检查，如确保不会在一个元素中出现重复的特性。）
* 字符串、数字、日期、时间等将使用标准XML格式转换为XText。
* 如果参数实现了IEnumerable（并且没有被其他东西所覆盖），Add方法将迭代其内容，并添加各个值，必要的时候会使用递归。
* 其他没有特殊处理的对象将调用ToString()将其转换为文本。

```c#
new XElement("root",
    new XElement("child",
        new XElement("grandchild", "text")),
    new XElement("other-child"));
```
得到
```xml
<root>
    <child>
        <grandchild>text</grandchild>
    </child>
    <other-child />
</root>
```
```c#
//从示例用户中创建元素
var users = new XElement("users",
    SampleData.AllUsers.Select(user => new XElement("user",
        new XAttribute("name", user.Name),
        new XAttribute("type", user.UserType)))
);
Console.WriteLine(users);
//创建文本节点元素
var developers = new XElement("developers",
    from user in SampleData.AllUsers
    where user.UserType == UserType.Developer
    select new XElement("developer", user.Name)
);
Console.WriteLine(developers);
```
### 查询单个节点
XElement包含很多轴方法（axis method），可用于查询资源。以下是可以直接对单个节点执行查询的轴方法，每个方法都返回适当的IEnumerable<T>。
* Ancestors
* DescendantNodes
* Annotations
* Elements
* Descendants
* ElementsBeforeSelf
* AncestorsAndSelf
* DescendantNodesAndSelf
* Attributes
* ElementAfterSelf
* DescendantsAndSelf
* Nodes
```c#
XElement root = XmlSampleData.GetElement();

var query = root.Element("users").Elements().Select(user => new
    {
        Name = (string) user.Attribute("name"),
        UserType = (string) user.Attribute("type")
    });
foreach (var user in query)
{
    Console.WriteLine ("{0}: {1}", user.Name, user.UserType);
}
```
### 合并查询操作符
```c#
from project in root.Element("projects").Elements()
from subscription in project.Elements("subscription")
select subscription
//等价于
root.Element("projects").Elements()
    .SelectMany(project => project.Elements("subscription"))
//等价于
root.Element("projects").Elements().Elements("subscription")
```
轴方法大多可作为扩展方法的形式使用。可以很容易地在LINQ to XML中编写XPath风格的查询，无须所有类型都为字符串。
```c#
root.Element("projects").Elements()
    .Where(project => ((string) project.Attribute("name"))
                                       .Contains("Media"))
    .Elements("subscription")
```
### 与LINQ和谐相处
LINQ to XML使用了如下三种方式与其他LINQ相适应：
* 在构造函数中消费序列。LINQ是刻意声明式语言，LINQ to XML支持声明式地创建XML结构。
* 在查询方法中返回序列。这大概是数据访问API必须遵循的最为明显的步骤：查询结果应该轻而易举地返回IEumerable<T>或实现了该接口的类。
* 扩展了可以对XML类型的序列所作的查询，这样可以让它们看上去更像是统一的查询API，尽管有些查询必须用于XML。

## 12.4 用并行LINQ代替LINQ to Objects
并行LINQ的背后理念是，某个LINQ to Objects查询需要执行很长的时间，而使用多线程利用多核优势进行查询则可以运行得很快，并且改动也很少。
### ParallelEnumerable、ParallelQuery和AsParallel
ParallelEnumerable是一个静态类，与Enumerable类似。它里面几乎全部是扩展方法，其中大多数都扩展了ParallelQuery这个类型。
该类型包含泛型和非泛型形式（ParallelQuery<TSource>和ParallelQuery），我们大多数情况下都会使用其泛型形式，就像IEnumerable<T>要比IEnumerable常用。此外，还有一个OrderedParallelQuery<TSource>类，它是IOrderedEnumerable<T>的并行版本。
### 调整并行查询
* AsOrdered：强制对查询排序
* AsUnordered：使有序查询变得无序；
* WithCancellation：在该查询中指定取消标记（cancellation token）。取消标记的使用贯穿了整个并行扩展，使任务以安全、可控的方式得以取消。
* WithDegreeOfParallelism：指定执行查询的最大并发任务数。
* WithExecutionMode：强制查询按并行方式执行，即使并行LINQ认为单线程执行得更快。
* WithMergeOptions：改变对结果的缓冲方式，禁止缓冲可以尽量缩短第一条结果的返回时间，但却降低了总的效率；完全缓冲的效率最高，但在查询执行完毕之前，不会返回任何结果。默认情况下使用两者的折中方案。

除了排序，这些不应该影响到查询的结果。
## 12.5 使用LINQ to Rx反转查询模型
### IObservable<T>和IObserver<T>
LINQ to Rx的数据模型与普通IEnumerable<T>的模型在数学上是对偶的。
在开始对拉集合进行迭代时，我们以“请给我一个迭代器”（调用GetEnumerator）开始，然后重复“还有其他项吗？如果有，就给我”（调用MoveNext和Current）。LINQ to Rx则是反向的。它不向迭代器发出请求，而是提供一个观察者。然后，它也不请求下一个项，而是通知你的代码是否准备好了一个项、是否有错误发生、是否到达了数据末端。

```c#
public interface IObservable<T>
{
    IDisposable Subscribe(IObserver<T> observer);
}

public interface IObserver<T>
{
    void OnNext(T value);
    void OnCompleted();
    void OnException(Exception error);
}
```
LINQ to Rx与我们熟悉的事件十分类似。调用一个可观察对象（observable）的Subscribe，就像是对事件使用+=来注册处理程序一样。Subscribe返回的可处置（disposable）值会记住传入的观察者（observer）：处置它就像对同一个处理程序使用-=一样。

在普通的拉模型(IEnumerable)中，调用MoveNext/Current时可能发生如下三种情况：
位于序列末尾，这时MoveNext返回false；
未到达序列末尾，这时MoveNext返回true，Current返回新的值；
出现错误——可能由于网络连接等问题，导致读取下一行失败，这时将抛出异常。
IObserver<T>接口分别用不同的方法来代表这几种情况。通常，观察者将重复调用OnNext方法，并最终调用OnCompleted——这期间如果出现了某种错误，就用OnError代替。在序列结束或发生错误之后不会再调用其他方法。
### 简单的开始
```c#
var observable = Observable.Range(0, 10);
observable.Subscribe(x => Console.WriteLine("Received {0}", x),
                     e => Console.WriteLine("Error: {0}", e),
                     () => Console.WriteLine("Finished"));
```
Range方法返回的是一个冷可观察对象（cold observable）。它处于休眠状态，直到某个观察者订阅了它，它才会向该观察者发送值。如果其他观察者也订阅了该对象，将会得到该范围的一个副本。这与点击按钮这种普通的事件不太相同，对于后者，多个观察者可以同时订阅同一个实际的值序列——并且即便没有任何观察者，也会有效地产生值。（毕竟，就算没有附加任何事件处理程序，你也可以点击按钮。）这种序列称为热可观察对象（hot observable）。
### 查询可观察对象
1. 过滤和投影
```c#
var numbers = Observable.Range(0, 10);
var query = from number in numbers
            where number % 2 == 0
            select number * number;
query.Subscribe(Console.WriteLine);
```
2. 分组
在LINQ to Objects中处理分组时常常要嵌套foreach循环，因此在LINQ to Rx中要嵌套订阅。
在LINQ to Objects中所需的大量的数据缓冲操作，都可以用LINQ to Rx更高效地实现。
```c#
var numbers = Observable.Range(0, 10);
var query = from number in numbers
            group number by number % 3;
query.Subscribe(group => group.Subscribe
    (x => Console.WriteLine("Value: {0}; Group: {1}", x, group.Key)));
```
3. 合并
LINQ to Rx提供了SelectMany的一些重载，其理念仍然与LINQ to Objects相同：原始序列中的每一项都生成一个新的序列，最终的结果是所有这些新序列的组合。
```c#
var query = from x in Observable.Range(1, 3)
            from y in Observable.Range(1, x)
            select new { x, y };
query.Subscribe(Console.WriteLine);
```
4. 新引入的和不支持的
let子句只能在调用Select时使用，但LINQ to Rx并没有实现所有的LINQ to Objects操作符。漏掉的大多是那些缓冲输出结果并返回新的可观察对象的操作符。例如Reverse方法和OrderBy方法。
LINQ to Rx包含Join方法，但它并不直接处理可观察对象，而是处理联接计划（join plan）。这是Rx实现联接演算（join-calculus）的部分内容，超出了本书的范畴。此外，Rx也没有实现GroupJoin方法，因此也不支持join...into。
### 意义何在
Rx提供了一种优雅的方式来思考各种异步处理——如普通.NET事件（可以使用Observable.FromEvent将其视为可观察对象）、异步I/O和调用Web服务。它提供了一种有效的方式来管理复杂性和并发。
## 12.6 扩展LINQ to Object
### 设计和实现指南
1. 单元测试
2. 检查参数
好的方法会检查传入的参数。但这对LINQ操作符来说有一个问题。我们已经看到，很多操作符都返回一个序列，而实现这种功能最简单的方式就是迭代器块。但你应该在调用方法的同时执行参数检查，而不应该等到调用者决定迭代其结果的时候。如果打算使用迭代器块，就把方法分成两部分：在公共方法中执行参数检查，然后调用一个私有方法进行迭代。
3. 优化
IEnumerable<T>本身所支持的操作十分有限，但你所操作的序列的执行时类型可能具备更多的功能。例如，Count()操作符总是可用的，但通常其复杂度为O(n)。然而如果调用的是ICollection<T>实现，就可以直接使用其Count属性，复杂度为O(1)。在.NET 4中，这种优化也包括ICollection。同样，通过索引获取特定的元素一般会很慢，但如果序列实现为IList<T>，就会很高效。
4. 文档
在文档中指明代码对输入的处理和操作符的预期性能是十分重要的。
5. 尽量只迭代一次
6. 释放迭代器
在大多数情况下，我们可以使用foreach语句来迭代数据源。但有时我们需要对第一个元素进行不同的处理，这时直接使用迭代器可以使代码更加简单。在这种情况下，要为迭代器使用using块。
7. 自定义比较器
很多LINQ操作符都包含可以指定适当IEqualityComparer<T>或IComparer<T>的重载。
```c#
//案例
public static T RandomElement<T>(this IEnumerable<T> source,
                                  Random random)
{
    if (source == null)   //❶ 检查参数
    {
        throw new ArgumentNullException("source");
    }
    if (random == null)
    {
        throw new ArgumentNullException("random");
    }
    ICollection collection = source as ICollection;   //❷ 优化集合
    if (collection != null)
    {
        int count = collection.Count;
        if (count == 0)
        {
            throw new InvalidOperationException("Sequence was empty.");
        }
        int index = random.Next(count);
         return source.ElementAt(index);   //用ElementAt进一步优化
    }
    using (IEnumerator<T> iterator = source.GetEnumerator())  //❸ 处理低效的情况
    {
        if (!iterator.MoveNext())
        {
            throw new InvalidOperationException("Sequence was empty.");
         }
        int countSoFar = 1;
        T current = iterator.Current;
        while (iterator.MoveNext())
        {
            countSoFar++;
            if (random.Next(countSoFar) == 0)   //❹ 有时需要取代当前的推测
            {
                current = iterator.Current;
            }
        }
        return current;
    }
}
```