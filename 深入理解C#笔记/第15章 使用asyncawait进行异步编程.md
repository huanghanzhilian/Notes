# 第15章 使用async/await进行异步编程
## 15.1 异步函数简介
C# 5引入了异步函数（asynchrnous function）的概念。通常是指用async修饰符声明的，可包含await表达式的方法或匿名函数。
await表达式作用：如果表达式等待的值还不可用，那么异步函数将立即返回；当该值可用时，异步函数将（在适当的线程上）回到离开的地方继续执行。此前“在这条语句完成之前不要执行下一条语句”的流程依然不变，只是不再阻塞。
```c#
class AsyncForm : Form
{
    Label label;
    Button button;

    public AsyncForm()
    {
        label = new Label { Location = new Point(10, 20),
                            Text = "Length" };
        button = new Button { Location = new Point(10, 50),
                              Text = "Click" };
        button.Click += DisplayWebSiteLength;   //❶ 包装事件处理程序
         AutoSize = true;
        Controls.Add(label);
        Controls.Add(button);
    }

    async void DisplayWebSiteLength(object sender, EventArgs e)
    {
        label.Text = "Fetching...";
        using (HttpClient client = new HttpClient())
        {
            string text =   /*❷ 开始获取页面 */
                await client.GetStringAsync("http://csharpindepth.com");
            label.Text = text.Length.ToString();   //❸ 更新UI
        }
    }
}

Application.Run(new AsyncForm());
```
> HttpClient在某种程度上是“经过改进的全新”WebClient。它是.NET 4.5之后的首选HTTP API，并且只包含异步操作。如果要编写Windows Store应用程序，你甚至都无法使用WebClient。

## 15.3 语法和语义
async是在声明异步方法时使用的修饰符，await表达式则负责消费异步操作。
### 声明异步方法
异步方法的声明语法与其他方法完全一样，只是要包含async上下文关键字。async可以出现在返回类型之前的任何位置。以下这些都是有效的：
```c#
public static async Task<int> FooAsync() { ... }
public async static Task<int> FooAsync() { ... }
async public Task<int> FooAsync() { ... }
public async virtual Task<int> FooAsync() { ... }
```
async修饰符在生成的代码中没有作用，这个事实是非常重要的。对调用方法来说，它只是一个可能会返回任务的普通方法。你可以将一个（具有适当签名的）已有方法改成使用async，反之亦然。对于源代码和二进制来说，这都是一个兼容的转换。
### 异步方法的返回类型
调用者和异步方法之间是通过返回值来通信的。异步函数的返回类型只能为：
* void
* Task
* Task<TResult>

之所以将异步方法设计为可以返回void，是为了和事件处理程序兼容。
对于一个异步方法，只有在作为事件订阅者时才应该返回void。在其他不需要特定返回值的情况下，最好将方法声明为返回Task。这样，调用者可以等待操作完成，以及探测失败情况等。

### 可等待模式
> await的约束
> 与yield return一样，使用await表达式也有一些约束条件。它不能在catch或finally块、非异步匿名函数、lock语句块或不安全代码中使用。
### await表达式的流
1. 展开复杂的表达式
await后面有时是方法调用的结果，有时是属性。
```c#
string pageText = await new HttpClient().GetStringAsync(url);
//await只是在操作一个值。上面的代码跟下面的是等价的：
Task<string> task = new HttpClient().GetStringAsync(url);
string pageText = await task;
```
await表达式的结果也可以用作方法实参，或作为其他表达式的一部分。
```c#
AddPayment(await employee.GetHourlyRateAsync() *
           await timeSheet.GetHoursWorkedAsync(employee.Id));
//拆分开来
Task<decimal> hourlyRateTask = employee.GetHourlyRateAsync();
decimal hourlyRate = await hourlyRateTask;
Task<int> hoursWorkedTask = timeSheet.GetHoursWorkedAsync(employee.Id);
int hoursWorked = await hoursWorkedTask;
AddPayment(hourlyRate * hoursWorked);           
```
2. 可见的行为
执行过程到达await表达式后，存在着两种可能：等待中的异步操作已经完成，或还未完成。
如果操作已经完成，那么执行流程就非常简单，只需继续执行即可。如果操作失败，并且由一个代表该失败的异常所捕获，则会抛出该异常。否则，将得到该操作所返回的结果，所有这一切，都无需任何线程上下文切换或附加任何后续操作。
异步操作仍在执行时。在这种情况下，方法异步地等待操作完成，然后继续执行适当的上下文。这种“异步等待”意味着方法将不再执行，它把后续操作附加在了异步操作上，然后返回。异步操作确保该方法在正确的线程中恢复。其中正确的线程通常指线程池线程（具体使用哪个线程都无妨）或UI线程。
在遇到第一个真正的异步await表达式之前，方法的执行是完全同步的。调用异步方法，与在单独的线程中启动一个新任务不同，并且你应确保总是编写能够快速返回的异步方法。当然，这取决于所写代码的上下文，但一般应避免在异步方法中执行耗时的工作。而应将其分离到其他方法，并为其创建一个Task。
3. 使用可等待模式的成员
## 15.4 异步匿名函数
可以通过异步匿名函数来创建表示异步操作的委托
```c#
Func<Task> lambda = async () => await Task.Delay(1000);
Func<Task<int>> anonMethod = async delegate()
{
    Console.WriteLine("Started");
    await Task.Delay(1000);
    Console.WriteLine("Finished");
    return 10;
};
```
与异步方法一样，在创建委托时，委托签名的返回类型必须为void、Task或Task<T>。
```c#
Func<int,Task<int>> function = async x =>
{
    Console.WriteLine("Starting... x={0}", x);
    await Task.Delay(x * 1000);
    Console.WriteLine("Finished... x={0}", x);
    return x * 2;
};
Task<int> first = function(5);
Task<int> second = function(3);
Console.WriteLine("First result: {0}", first.Result);
Console.WriteLine("Second result: {0}", second.Result);
```

