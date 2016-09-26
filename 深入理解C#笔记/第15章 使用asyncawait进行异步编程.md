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