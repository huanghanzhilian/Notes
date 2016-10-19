using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 中介者模式
{
    /*中介者模式(Mediator)
     * 用一个中介对象来封装一系列的对象交互。中介者使各对象不需要显式地相互引用，从而使其耦合松散，而且可以独立地改变它们之间的交互
     */    

    /// <summary>
    /// 抽象中介者
    /// 定义了同事对象到中介者对象的接口
    /// </summary>
    abstract class Mediator
    {
        public abstract void Send(string message, Colleague colleague);
    }

    /// <summary>
    /// 抽象同事类
    /// </summary>
    abstract class Colleague
    {
        protected Mediator mediator;
        public Colleague(Mediator mediator)
        {
            this.mediator = mediator;
        }
    }

    class ConcreteMediator : Mediator
    {
        private ConcreteColleague1 colleage1;

        private ConcreteColleague2 colleage2;

        public ConcreteColleague1 Colleage1 { set { colleage1 = value; } }

        public ConcreteColleague2 Colleage2 { set { colleage2 = value; } }

        public override void Send(string message, Colleague colleague)
        {
            if (colleague == colleage1)
            {
                colleage2.Notify(message);
            }
            else
            {
                colleage1.Notify(message);
            }
        }
    }

    class ConcreteColleague1 : Colleague
    {
        public ConcreteColleague1(Mediator mediator) : base(mediator)
        {
        }

        public void Send(string message)
        {
            mediator.Send(message, this);
        }

        public void Notify(string message)
        {
            Console.WriteLine("同事1得到信息：" + message);
        }
    }

    class ConcreteColleague2 : Colleague
    {
        public ConcreteColleague2(Mediator mediator) : base(mediator)
        {
        }

        public void Send(string message)
        {
            mediator.Send(message, this);
        }

        public void Notify(string message)
        {
            Console.WriteLine("同事2得到信息：" + message);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ConcreteMediator m = new ConcreteMediator();

            ConcreteColleague1 c1 = new ConcreteColleague1(m);
            ConcreteColleague2 c2 = new ConcreteColleague2(m);

            m.Colleage1 = c1;
            m.Colleage2 = c2;

            c1.Send("吃过饭了吗？");
            c2.Send("没有呢，你打算请客？");

            UnitedNationsSecurityCouncil unsc = new UnitedNationsSecurityCouncil();
            USA usa = new USA(unsc);
            Iraq iraq = new Iraq(unsc);

            unsc.Usa = usa;
            unsc.Iraq = iraq;

            usa.Declare("不准研制核武器，否则要发动战争");
            iraq.Declare("我们没有核武器，也不怕侵略");

        }
    }

    abstract class UnitedNations
    {
        public abstract void Declare(string message, Country country);
    }

    abstract class Country
    {
        protected UnitedNations mediator;

        public Country(UnitedNations mediator)
        {
            this.mediator = mediator;
        }
    }

    class USA : Country
    {
        public USA(UnitedNations mediator) : base(mediator)
        {
        }

        public void Declare(string message)
        {
            mediator.Declare(message, this);
        }

        public void GetMessage(string message)
        {
            Console.WriteLine($"美国获得对方信息：{message}");
        }
    }

    class Iraq : Country
    {
        public Iraq(UnitedNations mediator) : base(mediator)
        {
        }

        public void Declare(string message)
        {
            mediator.Declare(message, this);
        }

        public void GetMessage(string message)
        {
            Console.WriteLine($"伊拉克获得对方信息：{message}");
        }
    }

    class UnitedNationsSecurityCouncil : UnitedNations
    {
        private USA usa;
        private Iraq iraq;
        public USA Usa { set { usa = value; } }
        public Iraq Iraq { set { iraq = value; } }
        public override void Declare(string message, Country country)
        {
            if (country == usa)
            {
                iraq.GetMessage(message);
            }
            else
            {
                usa.GetMessage(message);
            }
        }
    }
}
