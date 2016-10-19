using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 外观模式
{
    /*外观模式(Facade)
     * 为子系统中的一组接口提供一个一致的界面，此模式定义了一个高层接口，这个接口使得这一子系统更加容易使用
     */

    /// <summary>
    /// 子系统类
    /// </summary>
    class SubSystemOne
    {
        public void MethodOne()
        {
            Console.WriteLine("子系统方法一");
        }
    }

    class SubSystemTwo
    {
        public void MethodTwo()
        {
            Console.WriteLine("子系统方法二");
        }
    }

    /// <summary>
    /// 外观类，它需要了解所有的子系统的方法或属性，进行组合，以备外界调用
    /// </summary>
    class Facade
    {
        SubSystemOne one;
        SubSystemTwo two;

        public Facade()
        {
            one = new SubSystemOne();
            two = new SubSystemTwo();
        }

        public void MethodA()
        {
            one.MethodOne();
        }
        public void MethodB()
        {
            two.MethodTwo();
            one.MethodOne();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //由于Facade的作用，客户端可以根本不知两个子系统的存在
            Facade facade = new Facade();
            facade.MethodA();
        }
    }
    /*何时使用外观模式
     * 首先，在设计初期阶段，应该要有意识的将不同的两个层分离，比如经典的三层架构，就需要考虑在数据访问层和业务逻辑层、业务逻辑层和表示层的层与层之间建立外观Facade
     * 其次，在开发阶段，子系统往往因为不断的重构演化而变得越来越复杂，增加外观Facade可以提供一个简单的接口，减少它们之间的依赖。
     * 第三，在维护一个遗留的大型系统时，可能这个系统已经非常难以维护和扩展了，为新系统开发一个外观Facade类，来提供设计粗糙或高度复杂的遗留代码的比较清晰的接口，让新系统与Facade对象交互，Facade与遗留代码交互所有复杂的工作。
     */
}
