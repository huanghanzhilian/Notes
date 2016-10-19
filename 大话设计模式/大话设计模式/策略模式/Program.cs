using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 策略模式
{
    /*策略模式(Strategy)
     * 它定义了算法家族，分别封装起来，让它们之间可以互相替换，此模式让算法的变化，不会影响到算法的客户。
     * 策略模式是一种定义一系列算法的方法，从概念上来看，所有这些算法完成的都是相同的工作，只是实现不同，它可以一相同的方式调用所有的算法，减少了各种算法类与算法类之间的耦合
     * 策略模式的Strategy类层次为Context定义了一系列的可供重用的算法或行为。继承有助于析取出这些算法中的公共功能
     * 策略模式简化了单元测试，因为每个算法都有自己的类，可以通过自己的接口单独测试
     * 当不同的行为堆砌在一个类中时，就很难避免使用条件语句来选择合适的行为。将这些行为封装在一个个独立的Strategy类中，可以在使用这些行为的类中消除条件语句。
     * 策略模式是用来封装算法的，可以用来封装任何类型的规则，只要在分析过程中听到需要在不同的事件应用不同的业务规则，就可以考虑使用策略模式处理这种变化的可能性。
    */
    class Program
    {
        static void Main(string[] args)
        {
            var cashContext = new CashContext("normal");
        }
    }

    public class CashContext
    {
        CashSuper cash = null;

        public CashContext(string type)
        {
            switch (type)
            {
                case "normal":
                    cash = new CashNormal();
                    break;
                case "100per300":
                    cash = new CashReturn(300, 100);
                    break;
                case "0.8":
                    cash = new CashRebate(0.8);
                    break;
                default:
                    break;
            }
        }

        public double GetResult(double money)
        {
            return cash.AcceptCash(money);
        }
    }

    public abstract class CashSuper
    {
        public abstract double AcceptCash(double money);
    }

    public class CashNormal : CashSuper
    {
        public override double AcceptCash(double money)
        {
            return money;
        }
    }

    public class CashRebate : CashSuper
    {
        private double moneyRebate = 1d;

        public CashRebate(double moneyRebate)
        {
            this.moneyRebate = moneyRebate;
        }

        public override double AcceptCash(double money)
        {
            return money * moneyRebate;
        }
    }

    public class CashReturn : CashSuper
    {
        private double moneyCondition = 0d;
        private double moneyReturn = 0d;
        public CashReturn(double moneyCondition, double moneyReturn)
        {
            this.moneyCondition = moneyCondition;
            this.moneyReturn = moneyReturn;
        }
        public override double AcceptCash(double money)
        {
            double ret = money;
            if (money >= moneyCondition)
                ret = money - Math.Floor(money / moneyCondition) * moneyReturn;
            return ret;
        }
    }
}
