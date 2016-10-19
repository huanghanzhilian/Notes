using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 原型模式
{
    /*原型模式(Prototype)
     * 用原型实例指定创建对象的种类，并通过拷贝这些原型创建新的对象
     * 原型模式其实就是从一个对象再创建另外一个可定制的对象，而且不需要知道任何创建的细节
     * 一般在初始化的信息不发生变化的情况下，克隆是最好的办法。这既隐藏了对象创建的细节，又对性能是大大的提高
     */

    abstract class Prototype
    {
        private string id;

        public Prototype(string id)
        {
            this.id = id;
        }

        public string Id
        {
            get { return id; }
        }

        /// <summary>
        /// Clone方法是抽象类的关键
        /// </summary>
        /// <returns></returns>
        public abstract Prototype Clone();
    }

    class ConcretePrototype : Prototype
    {
        public ConcretePrototype(string id) : base(id)
        {
        }

        public override Prototype Clone()
        {
            return (Prototype)this.MemberwiseClone();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //ConcretePrototype p = new ConcretePrototype("I");
            //ConcretePrototype c = (ConcretePrototype)p.Clone();

            Resume a = new Resume("张三");
            a.SetPersonalInfo("男", "23");
            a.SetWorkExperience("2015-2016", "TC");

            Resume b = (Resume)a.Clone();
            b.SetWorkExperience("2016-2017", "DJ");
            a.Display();
            b.Display();

            Console.Read();
        }
    }

    //在.NET中提供了ICloneable接口，实现这个接口就可以完成原型模式   
    //class Resume : ICloneable
    //{
    //    private string name;
    //    private string sex;
    //    private string age;
    //    private string timeArea;
    //    private string company;

    //    public Resume(string name)
    //    {
    //        this.name = name;
    //    }

    //    public void SetPersonalInfo(string sex, string age)
    //    {
    //        this.sex = sex;
    //        this.age = age;
    //    }

    //    public void SetWorkExperience(string timeArea, string company)
    //    {
    //        this.timeArea = timeArea;
    //        this.company = company;
    //    }

    //    public void Display()
    //    {
    //        Console.WriteLine($"{name} {sex} {age}");
    //        Console.WriteLine($"工作经历：{timeArea} {company}");
    //    }

    //    public object Clone()
    //    {
    //        return this.MemberwiseClone();
    //    }
    //}

    /*浅复制与深复制
     * “浅复制”：被复制对象的所有变量都含有与原来的对象相同的值，而所有的对其他对象的引用都任然指向原来的对象
     * “深复制”：把引用对象的变量指向复制过的新对象，而不是原有的被引用的对象
     */

    class WorkExperience : ICloneable
    {
        public string WorkDate { get; set; }
        public string Company { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    class Resume : ICloneable
    {
        private string name;
        private string sex;
        private string age;
        private WorkExperience work;

        public Resume(string name)
        {
            this.name = name;
        }

        private Resume(WorkExperience work)
        {
            this.work = (WorkExperience)work.Clone();
        }

        public void SetPersonalInfo(string sex, string age)
        {
            this.sex = sex;
            this.age = age;
        }

        public void SetWorkExperience(string workDate, string company)
        {
            this.work.WorkDate = workDate;
            this.work.Company = company;
        }

        public void Display()
        {
            Console.WriteLine($"{name} {sex} {age}");
            Console.WriteLine($"工作经历：{work.WorkDate} {work.Company}");
        }

        public object Clone()
        {
            Resume obj = new Resume(this.work);
            obj.name = this.name;
            obj.sex = this.sex;
            obj.age = this.age;
            return obj;
        }
    }
}