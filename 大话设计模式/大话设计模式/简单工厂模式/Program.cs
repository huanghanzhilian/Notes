using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 简单工厂模式
{
    class Program
    {
        static void Main(string[] args)
        {
            Operation oper = OperationFactory.CreateOperation("/");
            oper.NumberA = 10;
            oper.NumberB = 2;
            double result = oper.GetResult();
        }
    }

    public abstract class Operation
    {
        public double NumberA { get; set; }

        public double NumberB { get; set; }

        public abstract double GetResult();
    }

    public class OperationAdd : Operation
    {
        public override double GetResult()
        {
            return NumberA + NumberB;
        }
    }

    public class OperationDiv : Operation
    {
        public override double GetResult()
        {
            if (NumberB == 0)
                throw new Exception("除数不能为0.");
            return NumberA / NumberB;
        }
    }

    /// <summary>
    /// 运算工厂，创建运算对象
    /// </summary>
    public class OperationFactory
    {
        public static Operation CreateOperation(string operate)
        {
            Operation ret = null;
            switch (operate)
            {
                case "+":
                    ret = new OperationAdd();
                    break;
                case "/":
                    ret = new OperationDiv();
                    break;
                default:
                    break;
            }
            return ret;
        }
    }
}
