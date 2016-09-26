# 第11章 查询表达式和LINQ to Objects
## 11.1 LINQ介绍
### LINQ中的基础概念
1. 序列
   序列通过IEnumerable和IEnumerable<T>接口进行封装。序列就像数据项的传送带——每次只能获取它们一个，直到不再想获取数据，或者序列中没有数据了。
   序列和其他集合数据结构（比如列表和数组）之间最大的区别就是，当你从序列读取数据的时候，通常不知道还有多少数据项等待读取，或者不能访问任意的数据项——只能是当前的这个。
   序列是LINQ的基础。在看到一个查询表达式的时候，应该要想到它所涉及的序列：一开始总是存在至少一个序列，且通常在中间过程会转换为其他序列，也可能和更多的序列连接在一起。
2. 延迟执行和流处理
   查询表达式被创建的时候，不会处理任何数据，也不会访问原始的人员列表也未被访问。而是在内存中生成了这个查询的表现形式。判断是否为成人的谓词，以及人到人名的转换，都是通过委托实例来表示的。只有在访问结果IEnumerable<string>的第一个元素的时候，整个车轮才开始向前滚动。
   LINQ的这个特点称为延迟执行。在最终结果的第一个元素被访问的时候，Select转换才会为它的第一个元素调用Where转换。而Where转换会访问列表中的第一个元素，检查这个谓词是否匹配（在这个例子中，是匹配的），并把这个元素返回给\`\`Select。最后，依次提取出名称作为结果返回。
3. 标准查询操作符
   LINQ的标准查询操作符是一个转换的集合，具有明确的含义。微软鼓励LINQ提供器尽可能多得实现这些操作符，并让实现符合预期的行为。
## 11.2 简单的开始：选择元素
### 以数据源作为开始，以选择作为结束
每个查询表达式都以同样的方式开始——声明一个数据序列的数据源：`from element in source`，在第一个子句出现之后，许多不同的事情会发生，不过迟早都会以一个select子句(select子句被称为投影)或group子句来结束。
### 编译器转译是查询表达式基础的转译
编译器把查询表达式转译为普通的C#代码，这是支持C# 3查询表达式的基础。它是以一种机械的方式来进行转换的，不会去理解代码、应用类型引用、检查方法调用的有效性或执行编译器要执行的任何正常工作。这些都在转换完成之后来执行。
### 范围变量和重要的投影
范围变量不像其他种类的变量。在某些方面，它根本就不是变量。它们只能用于查询表达式中，实际代表了从一个表达式传递给另外一个表达式的上下文信息。它们表示了特定序列中的一个元素，而且它们被用于编译器转译中，以便把其他表达式轻易地转译为Lambda表达式。
### Cast、OfType和显式类型的范围变量
Cast和OfType可以处理任意非类型化的序列（它们是非泛型IEnumerable类的扩展方法），并返回强类型的序列。Cast通过把每个元素都转换为目标类型（遇到不是正确类型的任何元素的时候，就会出错）来处理，而OfType首先进行一个测试，以跳过任何具有错误类型的元素。
```c#
ArrayList list = new ArrayList { "First", "Second", "Third" };
IEnumerable<string> strings = list.Cast<string>();
foreach (string item in strings)
{
    Console.WriteLine(item);
}

list = new ArrayList { 1, "not an int", 2, 3 };
IEnumerable<int> ints = list.OfType<int>();
foreach (int item in ints)
{
    Console.WriteLine(item);
}
```
### 重要概念
* LINQ以数据序列为基础，在任何可能的地方都进行流处理。
* 创建一个查询并不会立即执行它：大部分操作都会延迟执行。
* C# 3的查询表达式包括一个把表达式转换为普通C#代码的预处理阶段，接着使用类型推断、重载、Lambda表达式等这些常规的规则来恰当地对转换后的代码进行编译。
* 在查询表达式中声明的变量的作用：它们仅仅是范围变量，通过它们你可以在查询表达式内部一致地引用数据。
## 11.3 对序列进行过滤和排序
### 使用where子句进行过滤
可以编写合并两个条件的单一where子句，来替换多个where子句的使用。在某些情况下，这样可以提高性能，但也要考虑查询表达式的可读性。
### 退化的查询表达式
编译器在转译查询表达式时，如果select子句什么都不做，只是返回同给定的序列相同的序列，编译器会删除所有对Select的调用。这就是所谓的退化查询表达式。
但是Select方法的结果只是数据项的序列，而不是数据源本身。查询表达式的结果和源数据永远不会是同一个对象，提供器能返回一个易变的结果对象，并知道即使面对一个退化查询，对返回数据集的改变也不会影响到“主”数据。
### 使用orderby子句进行排序
orderby子句的一般语法。它们基本上是上下文关键字orderby，后面跟一个或多个排序规则。一个排序规则就是一个表达式（可以使用范围变量），后面可以紧跟ascending或descending关键字，它的意思显而易见（默认规则是升序。）对于主排序规则的转译就是调用OrderBy或OrderByDescending，而其他子排序规则通过调用ThenBy或ThenByDescending来进行转换。
## 11.4 let子句和透明标识符
### 用let来进行中间计算
let子句引入了一个新的范围变量，它的值是基于其他范围变量。语法是极其简单的：`let标识符＝表达式`
```c#
var query = from user in SampleData.AllUsers
            let length = user.Name.Length
            orderby length
            select new { Name = user.Name, Length = length };
foreach (var entry in query)
{
    Console.WriteLine("{0}: {1}", entry.Length, entry.Name);
}
```
### 透明标识符
上面的代码在投影时使用了两个范围变量，但是Select()方法只对单个序列起作用，这就需要创建一个匿名类型来包含两个范围变量
```c#
SampleData.AllUsers
          .Select(user => new { user, length = user.Name.Length })
          .OrderBy(z => z.length)
          .Select(z => new { Name = z.user.Name, Length = z.length })
```
## 11.5 联接
### 使用join子句的内联接
内联接涉及两个序列。一个键选择器（key selector）表达式应用于第1个序列的每个元素，另外一个键选择器（可能完全不同）应用于第2个序列的每个元素。联接的结果是一个包含所有配对元素的序列，配对的规则是第1个元素的键与第2个元素的键相同。
> 如果两个键类型可以隐式地从其中一个转换到另外一个，也是有效的。其中的一个类型相比另一个类型来说，必须是更好的选择。编译器在推断隐式类型数组的类型时，也是使用同样的方式。

```c#
var query = from defect in SampleData.AllDefects
            join subscription in SampleData.AllSubscriptions
                on defect.Project equals subscription.Project
            select new { defect.Summary, subscription.EmailAddress };

foreach (var entry in query)
{
    Console.WriteLine("{0}: {1}", entry.EmailAddress, entry.Summary);
}
```
在LINQ to Objects的实现中，返回条目的顺序为：先返回使用左边序列中第1个元素的所有成对数据能被返回（按右边序列的顺序），接着返回使用左边序列中第2个元素的所有成对数据，依次类推。右边序列被缓冲处理，不过左边序列仍然进行流处理——所以，如果你打算把一个巨大的序列联接到一个极小的序列上，应尽可能把小序列作为右边序列。这种操作仍然是延迟的：在访问第1个数据对时，它才会开始执行，然后再从某个序列中读取数据。这时，它会读取整个右边序列，来建立一个从键到生成这些键的值的映射。之后，它就不需要再次读取右边的序列了，这时你可以迭代左边的序列，生成适当的数据对。
我们通常需要对序列进行过滤，而在联接前进行过滤比在联接后过滤效率要高得多。
### 使用join...into子句进行分组联接
分组联接（group join）的查询表达式看上去与之类似，不过却具有完全不同的结果。分组联接结果中的每个元素由左边序列（使用它的原始范围变量）的某个元素和右边序列的所有匹配元素的序列组成。后者用一个新的范围变量表示，该变量由join子句中into后面的标识符指定。
内联接和分组联接之间的一个重要差异（即分组联接和普通分组之间的差异）是，对于分组联接来说，在左边序列和结果序列之间是一对一的对应关系，即使左边序列中的某些元素在右边序列中没有任何匹配的元素，也无所谓。这是非常重要的，有时会用于**模拟SQL的左外联接**。在左边元素不匹配任何右边元素的时候，嵌入序列就是空的。与内联接一样，分组联接要对右边序列进行缓冲，而对左边序列进行流处理。
编译器将分组联接转译为简单地调用GroupJoin方法
### 使用多个from子句进行交叉联接和合并序列
上面两种联接都是相等联接(equijoin)——左边序列中的元素和右边序列要进行匹配。
交叉联接不在序列之间执行任何匹配操作：结果包含了所有可能的元素对。它们可以简单地使用两个（或多个）from子句来实现。
涉及多个from子句时，其实可认为是在前两个from子句上执行交叉联接，接着把结果序列和下一个from子句再次进行交叉联接，以此类推。每个额外的from子句都通过透明标识符添加了自己的范围变量。它就像指定了多表查询的笛卡儿积。
```c#
var query = from user in SampleData.AllUsers
            from project in SampleData.AllProjects
            select new { User = user, Project = project };
foreach (var pair in query)
{
    Console.WriteLine("{0}/{1}",
                       pair.User.Name,
                       pair.Project.Name);
}
```
左边序列中的每个元素都用来生成右边的一个序列，然后左边这个元素与右边新生成序列的每个元素都组成一对。这并不是通常意义上的交叉联接，而是将多个序列高效地合并（flat）成一个序列。
```c#
var query = from left in Enumerable.Range(1, 4)
            from right in Enumerable.Range(11, left)
            select new { Left = left, Right = right };
foreach (var pair in query)
{
    Console.WriteLine("Left={0}; Right={1}",
                       pair.Left, pair.Right);
}

/*
结果为
Left=1; Right=11
Left=2; Right=11
Left=2; Right=12
Left=3; Right=11
Left=3; Right=12
Left=3; Right=13
Left=4; Right=11
Left=4; Right=12
Left=4; Right=13
Left=4; Right=14
*/
```
编译器用来生成这个序列所调用的方法是SelectMany。它使用单个的输入序列（以我们的说法就是左边序列），一个从左边序列任意元素上生成另外一个序列的委托，以及一个生成结果元素（其包含了每个序列中的元素）的委托。
```c#
//转译后的代码
Enumerable.Range(1, 4)
          .SelectMany(left => Enumerable.Range(11, left),
                     (left, right) => new {Left = left, Right = right})
```
SelectMany的一个有意思的特性是，执行完全是流式的——一次只需处理每个序列的一个元素，因为它为左边序列的每个不同元素使用最新生成的右边序列。把它与内联接和分组联接进行比较，就能看出：在开始返回任何结果之前，它们都要完全加载右边序列。你应该在心中谨记如下问题：序列的预期大小，以及计算多次可能的资源开销，何时考虑要使用哪种类型的联接，哪个作为左边序列，哪个作为右边序列。
```c#
//案例：处理大量日志文件
var query = from file in Directory.GetFiles(logDirectory, "*.log")
            from line in ReadLines(file)
            let entry = new LogEntry(line)
            where entry.Type == EntryType.Error
            select entry;
/*
检索、解析并过滤了整个日志文件集，返回了表示错误的日志项的序列。至关重要的是，不会一次性向内存加载单个日志文件的全部内容，更不会一次性加载所有文件——所有的数据都采用流式处理。
*/            
```
## 11.6 分组和延续
### 使用group...by子句进行分组
语法`group projection by grouping`，该子句与select子句一样，出现在查询表达式的末尾。但它们的相似之处不止于此：projection表达式和select子句使用的投影是同样的类型。
grouping表达式通过其键来决定序列如何分组。整个结果是一个序列，序列中的每个元素本身就是投影后元素的序列，还具有一个Key属性，即用于分组的键；这样的组合是封装在IGrouping<TKey,TElement>接口中的，它扩展了IEnumerable <TElement>。同样，如果你想根据多个值来进行分组，可以使用一个匿名类型作为键。
```c#
var query = from defect in SampleData.AllDefects
            where defect.AssignedTo != null    //❶ 过滤未分配的缺陷
            group defect by defect.AssignedTo;   //❷ 用分配者来分组

foreach (var entry in query)
{
    Console.WriteLine(entry.Key.Name);   //❸ 使用每个条目的键：分配者
    foreach (var defect in entry)     //❹ 遍历数据条目的子序列
    {
        Console.WriteLine(" ({0}) {1}",
        defect.Severity, defect.Summary);
    }
    Console.WriteLine();
}
//转译后的代码
SampleData.AllDefects.Where(defect => defect.AssignedTo != null)
                     .GroupBy(defect => defect.AssignedTo)
```
### 查询延续
查询延续提供了一种方式，把一个查询表达式的结果用作另外一个查询表达式的初始序列。它可以应用于group...by和select子句上，语法对于两者是一样的——你只需使用上下文关键字into，并为新的范围变量提供一个名称就可以了。范围变量接着能用在查询表达式的下一部分。
```c#
var query = from defect in SampleData.AllDefects
            where defect.AssignedTo != null
            group defect by defect.AssignedTo into grouped
            select new { Assignee = grouped.Key,
                         Count = grouped.Count() };
foreach (var entry in query)
{
    Console.WriteLine("{0}: {1}",
                       entry.Assignee.Name, entry.Count);
}
```
## 11.7 在查询表达式和点标记之间做出选择
查询表达式在编译之前，先被转译成普通的C#。用普通的C#调用LINQ查询操作符来代替查询表达式，这种做法并没有官方名称，很多开发者称其为点标记（dot notation）。每个查询表达式都可以写成点标记的形式，反之则不成立：很多LINQ操作符在C#中不存在等价的查询表达式。
### 需要使用点标记的操作
最明显的必须使用点标记的情形是调用Reverse、ToDictionary这类没有相应的查询表达式语法的方法。然而即使查询表达式支持你要使用的查询操作符，也很有可能无法使用你想使用的特定重载。
Enumerable.Where包含一个重载，将父序列的索引作为另一个参数传入委托。因此，要从序列中排除其他项可以这样：`sequence.Where((item, index) => index % 2 == 0)`
Select也有类似的重载，因此，如果你要在排序之后获取序列排序之前的索引，可以这样：`sequence.Select((Item, Index) => new { Item, Index })
        .OrderBy(x => x.Item.Name)`
> 在匿名类型中直接使用Lambda表达式参数，可以打破参数名首字母使用小写字母的惯例，然后使用投影初始化器，就可以避免编写new { Item = item, Index = index }这样让人注意力分散的代码。

### 使用点标记可能会更简单的查询表达式
如果查询表达式所做的仅仅是过滤，倾向于使用点标记。
还有一种情况，使用点标记可以比查询表达式会更加清晰，就是被迫要在查询的某一部分使用点标记。
```c#
var adultNames = (from person in people
                  where person.Age >= 18
                  select person.Name).ToList();
                  
var adultNames = people.Where(person => person.Age >= 18)
                       .Select(person => person.Name)
                       .ToList();                  
```
### 选择查询表达式
在执行某些操作时（特别是联接操作），如果查询表达式使用了透明标识符，这时点标记的可读性就没那么高了。透明标识符之美在于它们是透明的，甚至你只看查询表达式的话都看不到它们！即使一个简单的let子句可能就足以让你选择查询表达式：如果引入一个新的匿名类型只是为了在查询中扩充上下文，那么很快就会让你产生厌烦情绪。
查询表达式占优势的另一种情况是，需要多个Lambda表达式，或多个方法调用。这同样也包括联接在内，你需要为每个联接方指定键选择器，以及最终的结果选择器。