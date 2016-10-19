using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 责任链模式
{
    /*责任链模式(Chain of Responsibility)
     * 使多个对象都有机会处理请求，从而避免请求的发送者和接受者之间的耦合关系。将这个对象连成一条链，并沿着这条链传递该请求，直到有一个对象处理它为止
     */

    /*责任链的好处
     * 当客户提交一个请求时，请求是沿链传递直至有一个ConcreteHandler对象负责处理他
     * 接受者和发送者都没有对方的明确信息，且链中的对象自己也并不知道链的结构。结果是职责链可简化对象的互相连接，它们仅需保持一个指向其后继者的引用，而不需保持它所有的候选接受者的引用
     * 可以随时地增加或修改处理一个请求的结构。增强了给对象指派责任的灵活性
     */

    /// <summary>
    /// 定义一个处理请示的接口
    /// </summary>
    abstract class Handler
    {
        protected Handler successor;

        //设置继任者
        public void SetSuccessor(Handler successor) => this.successor = successor;

        //处理请求的抽象方法
        public abstract void HandleRequest(int request);
    }

    /// <summary>
    /// 具体处理者类，处理它所负责的请求，可访问它的后继者，如果可处理该请求，就处理，否则就将请求转发给它的后继者
    /// </summary>
    class ConcreteHandler1 : Handler
    {
        public override void HandleRequest(int request)
        {
            if (request >= 0 && request < 10)
            {
                Console.WriteLine($"{this.GetType().Name}处理请求{request}");
            }
            else if (successor != null)
            {
                successor.HandleRequest(request);
            }
        }
    }

    class ConcreteHandler2 : Handler
    {
        public override void HandleRequest(int request)
        {
            if (request >= 10 && request < 20)
            {
                Console.WriteLine($"{this.GetType().Name}处理请求{request}");
            }
            else if (successor != null)
            {
                successor.HandleRequest(request);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Handler h1 = new ConcreteHandler1();
            Handler h2 = new ConcreteHandler2();
            h1.SetSuccessor(h2);

            int[] requests = { 2, 5, 14, 18, 3 };

            foreach (var item in requests)
            {
                h1.HandleRequest(item);
            }

            CommonManager jinli = new CommonManager("经理");
            Majordomo zongjian = new Majordomo("总监");
            GeneralManager zongjinli = new GeneralManager("总经理");
            jinli.SetSuperior(zongjian);
            zongjian.SetSuperior(zongjinli);

            Request request = new Request { RequestType = "请假", Number = 1, RequestContent = "沈伟请假" };
            jinli.RequestApplication(request);
            Request request2 = new Request { RequestType = "请假", Number = 4, RequestContent = "沈伟请假" };
            jinli.RequestApplication(request2);
            Request request3 = new Request { RequestType = "加薪", Number = 500, RequestContent = "沈伟请求加薪" };
            jinli.RequestApplication(request3);
            Request request4 = new Request { RequestType = "加薪", Number = 1000, RequestContent = "沈伟请求加薪" };
            jinli.RequestApplication(request4);


            Console.Read();
        }
    }

    abstract class Manager
    {
        protected string name;

        protected Manager superior;

        public Manager(string name)
        {
            this.name = name;
        }

        public void SetSuperior(Manager superior)
        {
            this.superior = superior;
        }

        abstract public void RequestApplication(Request request);
    }

    class CommonManager : Manager
    {
        public CommonManager(string name) : base(name)
        {
        }

        public override void RequestApplication(Request request)
        {
            if (request.RequestType == "请假" && request.Number <= 2)
            {
                Console.WriteLine($"{name}:{request.RequestContent} 数量{request.Number} 被批准");
            }
            else
            {
                if (superior != null)
                {
                    superior.RequestApplication(request);
                }
            }
        }
    }

    class Majordomo : Manager
    {
        public Majordomo(string name) : base(name)
        {
        }
        public override void RequestApplication(Request request)
        {
            if (request.RequestType == "请假" && request.Number <= 5)
            {
                Console.WriteLine($"{name}:{request.RequestContent} 数量{request.Number} 被批准");
            }
            else
            {
                if (superior != null)
                {
                    superior.RequestApplication(request);
                }
            }
        }
    }

    class GeneralManager : Manager
    {
        public GeneralManager(string name) : base(name)
        {
        }

        public override void RequestApplication(Request request)
        {
            if (request.RequestType == "请假")
            {
                Console.WriteLine($"{name}:{request.RequestContent} 数量{request.Number} 被批准");
            }
            else if (request.RequestType == "加薪" && request.Number <= 500)
            {
                Console.WriteLine($"{name}:{request.RequestContent} 数量{request.Number} 被批准");
            }
            else if (request.RequestType == "加薪" && request.Number > 500)
            {
                Console.WriteLine($"{name}:{request.RequestContent} 数量{request.Number} 再说吧");
            }
        }
    }

    class Request
    {
        public string RequestType { get; set; }

        public string RequestContent { get; set; }

        public int Number { get; set; }
    }
}
