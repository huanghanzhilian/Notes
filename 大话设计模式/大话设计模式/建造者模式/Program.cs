using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 建造者模式
{
    /*建造者模式(Builder)
     * 将一个复杂对象的构建与它的表示分离，使得同样的构建过程可以创建不同的表示
     */

    /// <summary>
    /// 产品类，由多个部件组成
    /// </summary>
    class Product
    {
        IList<string> parts = new List<string>();
        public void Add(string part)
        {
            parts.Add(part);
        }

        public void Show()
        {
            Console.WriteLine("\n产品 创建----");
            foreach (string part in parts)
            {
                Console.WriteLine(part);
            }
        }
    }

    abstract class Builder
    {
        public abstract void BuildPartA();
        public abstract void BuildPartB();
        public abstract Product GetResult();
    }

    class ConcreteBuilder1 : Builder
    {
        private Product product = new Product();

        public override void BuildPartA()
        {
            product.Add("部件A");
        }

        public override void BuildPartB()
        {
            product.Add("部件B");
        }

        public override Product GetResult()
        {
            return product;
        }
    }

    class ConcreteBuilder2 : Builder
    {
        private Product product = new Product();
        public override void BuildPartA()
        {
            product.Add("部件X");
        }

        public override void BuildPartB()
        {
            product.Add("部件Y");
        }

        public override Product GetResult()
        {
            return product;
        }
    }

    class Director
    {
        public void Construct(Builder builder)
        {
            builder.BuildPartA();
            builder.BuildPartB();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Director director = new Director();
            Builder b1 = new ConcreteBuilder1();

            director.Construct(b1);
            Product p1 = b1.GetResult();
            p1.Show();
        }
    }

    abstract class PersonBuilder
    {
        protected Graphics g;
        protected Pen p;

        public PersonBuilder(Graphics g, Pen p)
        {
            this.g = g;
            this.p = p;
        }

        public abstract void BuildHead();
        public abstract void BuildBody();
        public abstract void BuildArmLeft();
        public abstract void BuildArmRight();
        public abstract void BuildLegLeft();
        public abstract void BuildLegRight();
    }

    class PersonThinBuilder : PersonBuilder
    {
        public PersonThinBuilder(Graphics g, Pen p) : base(g, p)
        {
        }

        public override void BuildArmLeft()
        {
            g.DrawLine(p, 60, 50, 40, 100);
        }

        public override void BuildArmRight()
        {
            g.DrawLine(p, 70, 50, 90, 100);
        }

        public override void BuildBody()
        {
            g.DrawRectangle(p, 60, 50, 10, 50);
        }

        public override void BuildHead()
        {
            g.DrawEllipse(p, 50, 20, 30, 30);
        }

        public override void BuildLegLeft()
        {
            g.DrawLine(p, 60, 100, 45, 150);
        }

        public override void BuildLegRight()
        {
            g.DrawLine(p, 70, 100, 85, 150);
        }
    }

    /// <summary>
    /// 指挥者(Director)
    /// 用来控制建造过程，也用来隔离用户与建造过程的关联
    /// </summary>
    class PersonDirector
    {
        private PersonBuilder pb;
        /// <summary>
        /// 用户告诉指挥者，我需要什么样的小人
        /// </summary>
        /// <param name="pb"></param>
        public PersonDirector(PersonBuilder pb)
        {
            this.pb = pb;
        }
        /// <summary>
        /// 根据用户的选择建造小人
        /// </summary>
        public void CreatePerson()
        {
            pb.BuildHead();
            pb.BuildBody();
            pb.BuildArmLeft();
            pb.BuildArmRight();
            pb.BuildLegLeft();
            pb.BuildLegRight();
        }
    }
}
