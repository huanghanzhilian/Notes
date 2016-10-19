using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 命令模式
{
    /*命令模式(Command)
     * 将一个请求封装为一个对象，从而使你可用不同的请求对客户进行参数化：对请求排队或记录请求日志，以及支持可撤销的操作
     */

    /// <summary>
    /// 用来声明执行操作的接口
    /// </summary>
    abstract class Command
    {
        protected Receiver receiver;

        public Command(Receiver receiver)
        {
            this.receiver = receiver;
        }

        abstract public void Execute();
    }

    /// <summary>
    /// 将一个接受者对象绑定于一个动作，调用接受者相应的操作，以实现Execute
    /// </summary>
    class ConcreteCommand : Command
    {
        public ConcreteCommand(Receiver receiver) : base(receiver)
        {
        }

        public override void Execute()
        {
            this.receiver.Action();
        }
    }

    /// <summary>
    /// 要求该命令执行这个请求
    /// </summary>
    class Invoker
    {
        private Command command;

        public void SetCommand(Command command)
        {
            this.command = command;
        }

        public void ExecuteCommand()
        {
            command.Execute();
        }
    }

    /// <summary>
    /// 知道如何实施与执行一个与请求相关的操作，任何类都可能作为一个接收者
    /// </summary>
    class Receiver
    {
        public void Action()
        {
            Console.WriteLine("执行请求！");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Receiver r = new Receiver();
            Command c = new ConcreteCommand(r);
            Invoker i = new Invoker();
            i.SetCommand(c);
            i.ExecuteCommand();
        }
    }
    /*命令模式作用
     * 第一，它能较容易地设计一个命令队列；
     * 第二，在需要的情况下，可以较容易地将命令记入日志；
     * 第三，允许接受请求的一方决定是否否决请求
     * 第四，可以容易地实现对请求的撤销和重做
     * 第五，由于加进新的具体命令类不影响其他的类，因此增加新的具体命令类很容易
     * 命令模式把请求一个操作的对象与知道怎么执行一个操作的对象分隔开
     */
}
