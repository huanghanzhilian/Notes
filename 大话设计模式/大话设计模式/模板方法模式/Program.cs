using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 模板方法模式
{
    /*模板方法模式(TemplateMethod)
     * 定义一个操作中的算法的骨架，而将一些步骤延迟到子类中。模板方法使得子类可以不改变一个算法的结构即可重定义该算法的某些特定步骤。
     */

    /// <summary>
    /// 抽象模板，定义并实现了一个模板方法
    /// </summary>
    abstract class AbstractClass
    {
        //一些抽象行为，放到子类去实现
        public abstract void PrimitiveOperation1();
        public abstract void PrimitiveOperation2();

        //模板方法，给出了逻辑的骨架，而逻辑的组成是一些相应的抽象操作，它们都推迟到子类实现
        public void TemplateMethod()
        {
            PrimitiveOperation1();
            PrimitiveOperation2();
        }
    }

    /// <summary>
    /// 实现父类所定义的一个或多个抽象方法
    /// </summary>
    class ConcreteClassA : AbstractClass
    {
        public override void PrimitiveOperation1()
        {
            throw new NotImplementedException();
        }

        public override void PrimitiveOperation2()
        {
            throw new NotImplementedException();
        }
    }

    class ConcreteClassB : AbstractClass
    {
        public override void PrimitiveOperation1()
        {
            throw new NotImplementedException();
        }

        public override void PrimitiveOperation2()
        {
            throw new NotImplementedException();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            AbstractClass c = new ConcreteClassA();
            c.TemplateMethod();
        }
    }
}
