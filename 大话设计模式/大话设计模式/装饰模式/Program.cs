using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 装饰模式
{
    /*装饰模式(Decorator)
     * 动态地给一个对象添加一些额外的职责，就增加功能来说，装饰模式比生成子类更加灵活
     * 
     */

    /// <summary>
    /// Component定义了一个对象接口，可以给这些对象动态地添加职责
    /// </summary>
    public abstract class Component
    {
        public abstract void Operation();
    }

    /// <summary>
    /// ConcreteComponent是定义了一个具体的对象，也可以给这个对象添加一些职责
    /// </summary>
    public class ConcreteComponent : Component
    {
        public override void Operation()
        {
            Console.WriteLine("具体对象的操作");
        }
    }

    /// <summary>
    /// Decorator装饰抽象类，继承了Component，从外类来扩展Component类的功能，但对于Component来说，无需知道Decorator的存在
    /// </summary>
    public class Decorator : Component
    {
        protected Component component;

        public void SetComponent(Component component)
        {
            this.component = component;
        }

        public override void Operation()
        {
            if (component != null)
            {
                component.Operation();
            }
        }
    }

    /// <summary>
    /// ConcreteDecorator具体的装饰对象，起到给Component添加职责的功能
    /// </summary>
    public class ConcreteDecorator : Decorator
    {
        private void AddedBehavior() { }
        public override void Operation()
        {
            base.Operation();
            AddedBehavior();
            Console.WriteLine("具体装饰对象的操作");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            /*装饰的方法是：
             * 首先用ConcreteComponent实例化对象c
             * 然后用ConcreteDecorator的实例化对象来包装c
             * 最终执行c的Operation()
            */
            //ConcreteComponent c = new ConcreteComponent();
            //ConcreteDecorator d = new ConcreteDecorator();
            //d.SetComponent(c);
            //d.Operation();

            var zhangsan = new Person("张三");
            var tshirt = new TShirts();
            var shorts = new Shorts();
            //
            tshirt.Decorate(zhangsan);
            shorts.Decorate(tshirt);
            shorts.Show();

            Console.Read();
        }
    }

    public class Person
    {
        public Person() { }

        private string name;

        public Person(string name)
        {
            this.name = name;
        }

        public virtual void Show()
        {
            Console.WriteLine($"装扮的{name}");
        }
    }

    /// <summary>
    /// 服饰类(Decorator)
    /// </summary>
    public class Finery : Person
    {
        protected Person component;
        /// <summary>
        /// 打扮
        /// </summary>
        /// <param name="component">装饰对象</param>
        public void Decorate(Person component)
        {
            this.component = component;
        }

        public override void Show()
        {
            if (component != null)
            {
                component.Show();
            }
        }
    }

    /// <summary>
    /// 具体服饰类(ConcreteDecorator)
    /// </summary>
    public class TShirts : Finery
    {
        public override void Show()
        {
            Console.Write("T恤");
            base.Show();
        }
    }

    public class Shorts : Finery
    {
        public override void Show()
        {
            Console.Write("短裤");
            base.Show();
        }
    }
}
