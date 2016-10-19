using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 组合模式
{
    /*组合模式(Composite)
     * 将对象组合成树形结构以表示‘部分-整体’的层次结构。组合模式使得用户对单个对象和组合对象的使用具有一致性。
     */

    /*
     * 需求中是体现部分与整体层次的结构时，以及你希望用户可以忽略组合对象与单个对象的不同，统一地使用组合结构中的所有对象时，就应该考虑使用组合模式了
     */

    /// <summary>
    /// 组合中的对象声明接口，在适当情况下，实现所有类公有接口的默认行为。声明一个接口用于访问和管理Component的子部件
    /// </summary>
    abstract class Component
    {
        protected string name;

        public Component(string name)
        {
            this.name = name;
        }
        //通常都用Add和Remove方法来提供增加或移除树叶或树枝的功能
        public abstract void Add(Component c);
        public abstract void Remove(Component c);
        public abstract void Display(int depth);
    }

    /// <summary>
    /// 在组合中表示叶子节点对象，叶子节点没有子节点
    /// </summary>
    class Leaf : Component
    {
        public Leaf(string name) : base(name)
        {
        }
        //由于叶子没有再增加分枝和树叶，所以Add和Remove方法实现它没有意义，但这样做可以消除叶节点和枝节点对象在抽象层次的区别，它们具备完全一致的接口
        public override void Add(Component c)
        {
            Console.WriteLine("Cannot add to a leaf");
        }

        public override void Remove(Component c)
        {
            Console.WriteLine("Cannot remove from a leaf");
        }

        /// <summary>
        /// 叶节点的具体方法，此处是显示其名称和级别
        /// </summary>
        /// <param name="depth"></param>
        public override void Display(int depth)
        {
            Console.WriteLine(new String('-', depth) + name);
        }
    }

    class Composite : Component
    {
        //一个子对象集合用来储存其下属的枝节点和叶节点
        private List<Component> children = new List<Component>();

        public Composite(string name) : base(name)
        {
        }

        public override void Add(Component c)
        {
            children.Add(c);
        }

        public override void Remove(Component c)
        {
            children.Remove(c);
        }

        /// <summary>
        /// 显示其枝节点 名称，并对其下级进行遍历
        /// </summary>
        /// <param name="depth"></param>
        public override void Display(int depth)
        {
            Console.WriteLine(new String('-', depth) + name);

            foreach (Component component in children)
            {
                component.Display(depth + 2);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Composite root = new Composite("root");
            root.Add(new Leaf("Leaf A"));
            root.Add(new Leaf("Leaf B"));

            Composite comp = new Composite("Composite X");
            comp.Add(new Leaf("Leaf XA"));
            comp.Add(new Leaf("Leaf XB"));

            root.Add(comp);

            Composite comp2 = new Composite("Composite XY");
            comp2.Add(new Leaf("Leaf XYA"));
            comp2.Add(new Leaf("Leaf XYB"));

            comp.Add(comp2);

            root.Add(new Leaf("Leaf C"));

            Leaf leaf = new Leaf("Leaf D");
            root.Add(leaf);
            root.Remove(leaf);

            root.Display(1);

            ConcreteCompany rootCompany = new ConcreteCompany("南京总公司");
            rootCompany.Add(new HRDepartment("总公司人力资源部"));


            Console.Read();
        }
    }

    abstract class Company
    {
        protected string name;

        public Company(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="c"></param>
        public abstract void Add(Company c);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="c"></param>
        public abstract void Remove(Company c);

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="depth"></param>
        public abstract void Display(int depth);

        /// <summary>
        /// 履行职责
        /// </summary>
        public abstract void LineOfDuty();
    }

    class ConcreteCompany : Company
    {
        private List<Company> children = new List<Company>();
        public ConcreteCompany(string name) : base(name)
        {
        }

        public override void Add(Company c)
        {
            children.Add(c);
        }

        public override void Display(int depth)
        {
            Console.WriteLine(new String('-', depth));

            foreach (var item in children)
            {
                item.Display(depth + 2);
            }
        }

        public override void LineOfDuty()
        {
            foreach (var item in children)
            {
                item.LineOfDuty();
            }
        }

        public override void Remove(Company c)
        {
            children.Remove(c);
        }
    }

    class HRDepartment : Company
    {
        public HRDepartment(string name) : base(name)
        {
        }

        public override void Add(Company c)
        {
        }

        public override void Display(int depth)
        {
            Console.WriteLine(new String('-', depth));
        }

        public override void LineOfDuty()
        {
            Console.WriteLine("{0} 员工招聘培训管理", name);
        }

        public override void Remove(Company c)
        {
        }
    }

    class FinanceDepartment : Company
    {
        public FinanceDepartment(string name) : base(name)
        {
        }

        public override void Add(Company c)
        {
        }

        public override void Display(int depth)
        {
            Console.WriteLine(new String('-', depth));
        }

        public override void LineOfDuty()
        {
            Console.WriteLine("{0} 公司财务收支处理", name);
        }

        public override void Remove(Company c)
        {
        }
    }
}
