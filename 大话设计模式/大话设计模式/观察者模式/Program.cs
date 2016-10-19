using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 观察者模式
{
    /*观察者模式
     * 又叫做发布-订阅(Publish/Subscribe)模式
     * 定义了一种一对多的依赖关系，让多个观察者对象同时监听某一个主题对象。这个主体对象在状态发生变化时，会通知所有观察者对象，使它们能够自动更新自己。
     * 不推荐使用，可以使用事件来进行解耦
     * 或者使用IObservable<T>和IObserver<T>接口
     */

    abstract class Subject
    {
        private IList<Observer> observers = new List<Observer>();

        public void Attach(Observer observer)
        {
            observers.Add(observer);
        }

        public void Detach(Observer observer)
        {
            observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (Observer o in observers)
            {
                o.Update();
            }
        }
    }

    abstract class Observer
    {
        public abstract void Update();
    }

    class ConcreteSubject : Subject
    {
        public string SubjectState { get; set; }
    }

    class ConcreteObserver : Observer
    {
        private string name;
        private string observerState;
        public ConcreteSubject Subject { get; set; }

        public ConcreteObserver(ConcreteSubject subject, string name)
        {
            this.Subject = subject;
            this.name = name;
        }

        public override void Update()
        {
            observerState = Subject.SubjectState;
            Console.WriteLine("观察者{0}的新状态是{1}", name, observerState);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ConcreteSubject s = new ConcreteSubject();

            s.Attach(new ConcreteObserver(s, "x"));
            s.Attach(new ConcreteObserver(s, "y"));

            s.SubjectState = "ABC";
            s.Notify();

            Console.Read();
        }
    }
}
