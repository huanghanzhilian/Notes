using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 状态模式
{
    /*状态模式(State)
     * 当一个对象的内在状态改变时允许改变其行为，这个对象看起来像是改变了其类
     * 状态模式只要是解决的是当控制一个对象状态转换的条件表达式过于复杂时的情况。把状态的判断逻辑转移到表示不同状态的一系列类当中，可以把复杂的判断逻辑简化
     */

    /*状态模式好处与用处
     * 将与特定状态相关的行为局部化，并且将不同状态的行为分割开来，状态模式通过把各种状态转移逻辑分布到State的子类之间，来减少相互间的依赖
     * 当一个对象的行为取决于它的状态，并且它必须在运行时时刻根据状态改变它的行为时，就可以考虑使用状态模式了
     */

    /// <summary>
    /// State类，抽象状态类，定义一个接口以封装与Context的一个特定状态相关的行为
    /// </summary>
    abstract class State
    {
        public abstract void Handle(Context context);
    }

    /// <summary>
    /// ConcreteState类，具体状态，每一个子类实现一个与Context的一个状态相关的行为
    /// </summary>
    class ConcreteStateA : State
    {
        public override void Handle(Context context)
        {
            //设置ConcreteStateA的下一个状态是ConcreteStateB
            context.State = new ConcreteStateB();
        }
    }

    class ConcreteStateB : State
    {
        public override void Handle(Context context)
        {
            context.State = new ConcreteStateA();
        }
    }

    class Context
    {
        private State state;
        public Context(State state)
        {
            this.state = state;
        }
        public State State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
                Console.WriteLine("当前状态：" + state.GetType().Name);
            }
        }

        public void Request()
        {
            state.Handle(this);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Context c = new Context(new ConcreteStateA());

            c.Request();
            c.Request();
            c.Request();
            c.Request();
            c.Request();

            Console.ReadLine();
        }
    }

    public abstract class WorkState
    {
        public abstract void WriteProgram(Work w);
    }

    public class ForenoonState : WorkState
    {
        public override void WriteProgram(Work w)
        {
            if (w.Hour < 12)
            {
                Console.WriteLine("当前时间：{0}点 上午工作，精神百倍", w.Hour);
            }
            else
            {
                w.SetState(new NoonState());
                w.WriteProgram();
            }
        }
    }

    class NoonState : WorkState
    {
        public override void WriteProgram(Work w)
        {
            if (w.Hour < 13)
            {
                Console.WriteLine("当前时间：{0}点 饿了，午饭；犯困，午休。", w.Hour);
            }
            else
            {
                w.SetState(new EveningState());
                w.WriteProgram();
            }
        }
    }

    class AfterNoonState : WorkState
    {
        public override void WriteProgram(Work w)
        {
            if (w.Hour < 17)
            {
                Console.WriteLine("当前时间：{0}点 下午状态还不错，继续努力", w.Hour);
            }
            else
            {
                w.SetState(new AfterNoonState());
                w.WriteProgram();
            }
        }
    }

    class EveningState : WorkState
    {
        public override void WriteProgram(Work w)
        {
            if (w.TaskFinished)
            {
                w.SetState(new RestState());
                w.WriteProgram();
            }
            else
            {
                if (w.Hour < 21)
                {
                    Console.WriteLine("当前时间：{0}点 加班哦，疲惫之极", w.Hour);
                }
                else
                {
                    w.SetState(new SleepingState());
                    w.WriteProgram();
                }
            }
        }
    }

    internal class SleepingState : WorkState
    {
        public override void WriteProgram(Work w)
        {
            Console.WriteLine("当前时间：{0}点 不行了，睡着了。", w.Hour);
        }
    }

    internal class RestState : WorkState
    {
        public override void WriteProgram(Work w)
        {
            Console.WriteLine("当前时间：{0}点 下班回家了", w.Hour);
        }
    }

    public class Work
    {
        private WorkState current;
        public Work()
        {
            current = new ForenoonState();
        }
        public int Hour { get; set; }
        public bool TaskFinished { get; set; }

        public void SetState(WorkState state)
        {
            current = state;
        }

        public void WriteProgram()
        {
            current.WriteProgram(this);
        }
    }
}
