using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 适配器模式
{
    /*适配器模式(Adapter)
     * 将一个类的接口转换成客户希望的另外一个接口。Adapter模式使得原本由于接口不兼容而不能一起工作的那些类可以一起工作
     */

    /*
     * 系统的数据和行为都正确，但接口不符时，我们应该考虑用适配器，目的是使控制范围之外的一个原有对象与某个接口匹配。适配器模式主要应用于希望复用一些现存的类，但是接口又与复用环境要求不一致的情况
     *适配器模式有两种类型，类适配器模式和对象适配器模式。
     * 类适配器模式通过多重继承对一个接口与另一个接口进行匹配，而C#不支持多重继承，这里演示的是对象适配器。 
     */

    /*.Net中适配器模式的应用
     * DataAdapter用作DataSet和数据源之间的适配器以便检索和保存数据。DataAdapter通过映射Fill(这更改了DataSet中的数据以便与数据源中的数据相匹配)和Update(这更改了数据源中的数据以便与DataSet中的数据相匹配)来提供这一适配器
     */

    /// <summary>
    /// 这是客户所期待的接口。目标可以是具体的或抽象的类，也可以是接口
    /// </summary>
    class Target
    {
        public virtual void Request()
        {
            Console.WriteLine("普通请求");
        }
    }

    /// <summary>
    /// 需要适配的类
    /// </summary>
    class Adaptee
    {
        public void SpecificRequest()
        {
            Console.WriteLine("特殊请求");
        }
    }

    /// <summary>
    /// 通过在内部包装一个Adaptee对象，把源接口转换成目标接口
    /// </summary>
    class Adapter : Target
    {
        //建立一个私有的Adaptee对象
        private Adaptee adaptee = new Adaptee();

        //把表面上调用Request()方法变成实际调用SpecificRequest()
        public override void Request()
        {
            adaptee.SpecificRequest();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Target target = new Adapter();
            target.Request();

            Player m = new Forwards("巴蒂尔");
            //适配器
            Player ym = new Translator("姚明");

            Console.Read();
        }
    }

    abstract class Player
    {
        protected string name;
        public Player(string name)
        {
            this.name = name;
        }
        public abstract void Attack();
        public abstract void Defense();
    }

    class Forwards : Player
    {
        public Forwards(string name) : base(name)
        {
        }

        public override void Attack()
        {
            Console.WriteLine("前锋{0}进攻", name);
        }

        public override void Defense()
        {
            Console.WriteLine("前锋{1}防守", name);
        }
    }

    class ForeignCenter
    {
        public string Name { get; set; }

        public void 进攻()
        {
            Console.WriteLine("外籍中锋{0}进攻");
        }

        public void 防守()
        {
            Console.WriteLine("外籍中锋{0}防守");
        }
    }

    class Translator : Player
    {
        private ForeignCenter wjzf = new ForeignCenter();
        public Translator(string name) : base(name)
        {
            wjzf.Name = name;
        }

        public override void Attack()
        {
            wjzf.进攻();
        }

        public override void Defense()
        {
            wjzf.防守();
        }
    }
}
