using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 迭代器模式
{
    /*迭代器模式(Iterator)
     * 提供一种方法顺序访问一个聚合对象中各个元素，而又不暴露该对象的内部表示
     */

    /*
     * 当需要访问一个聚集对象，而且不管这些对象是什么都需要遍历的时候，就应该考虑用迭代器模式
     * 需要对聚集有多种方式遍历时，可以考虑用迭代器模式
     * 为遍历不同的聚集结构提供如开始、下一个、是否结束、当前哪一项等统一的接口
     */

    /// <summary>
    /// 迭代抽象类
    /// 用于定义得到开始对象、得到下一个对象、判断是否到结尾、当前对象等方法，统一接口
    /// </summary>
    abstract class Iterator
    {
        public abstract object First();
        public abstract object Next();
        public abstract bool IsDone();
        public abstract object CurrentItem();
    }

    /// <summary>
    /// 聚合抽象类
    /// </summary>
    abstract class Aggregate
    {
        public abstract Iterator CreateInterator();
    }

    class ConcreteIterator : Iterator
    {
        private ConcreteAggregate aggregate;
        private int current = 0;

        public ConcreteIterator(ConcreteAggregate aggregate)
        {
            this.aggregate = aggregate;
        }

        public override object CurrentItem()
        {
            return aggregate[current];
        }

        public override object First()
        {
            return aggregate[0];
        }

        public override bool IsDone()
        {
            return current >= aggregate.Count;
        }

        public override object Next()
        {
            object ret = null;
            current++;
            if (current < aggregate.Count)
            {
                ret = aggregate[current];
            }
            return ret;
        }
    }

    class ConcreteAggregate : Aggregate
    {
        private IList<object> items = new List<object>();
        public override Iterator CreateInterator()
        {
            return new ConcreteIterator(this);
        }

        public int Count { get { return items.Count; } }

        public object this[int index]
        {
            get { return items[index]; }
            set { items.Insert(index, value); }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

        }
    }
}
