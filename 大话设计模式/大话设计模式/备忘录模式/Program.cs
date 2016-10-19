﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 备忘录模式
{
    /*备忘录(Memento)
     * 在不破坏封装性的前提下，捕获一个对象的内部状态，并在该对象之外保存这个状态。这样以后就可将该对象恢复到原先保存的状态
     */

    /// <summary>
    /// 发起人
    /// 负责创建一个备忘录Memento，用以记录当前时刻它的内部状态，并可以使用备忘录恢复内部状态
    /// Originator可根据需要决定Memento存储Originator的哪些内部状态
    /// </summary>
    class Originator
    {
        /// <summary>
        /// 需要保存的属性
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 创建备忘
        /// </summary>
        /// <returns></returns>
        public Memento CreateMemento()
        {
            return new Memento(State);
        }

        /// <summary>
        /// 恢复备忘
        /// </summary>
        /// <param name="memento"></param>
        public void SetMemento(Memento memento)
        {
            State = memento.State;
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        public void Show()
        {
            Console.WriteLine("State=" + State);
        }
    }

    /// <summary>
    /// 备忘录
    /// 负责存储Originator对象的内部状态，并可防止Originator以外的对象访问备忘录Memento
    /// 备忘录有两个接口，Caretaker只能看到备忘录的窄接口，它只能将备忘录传递给其他对象。Originator能够看到一个宽接口，允许它访问返回到先前状态所需的所有数据
    /// </summary>
    class Memento
    {
        private string state;

        public Memento(string state)
        {
            this.state = state;
        }

        public string State { get { return state; } }
    }

    /// <summary>
    /// 管理者
    /// 负责保存好备忘录Memento
    /// 不能对备忘录的内容进行操作或检查
    /// </summary>
    class Caretaker
    {
        public Memento Memento { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Originator o = new Originator();
            o.State = "on";
            o.Show();
            Caretaker c = new Caretaker();
            c.Memento = o.CreateMemento();

            o.State = "off";
            o.Show();

            o.SetMemento(c.Memento);
            o.Show();

            Console.Read();
        }
    }
}
