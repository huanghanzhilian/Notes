using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 解析器模式
{
    /*解析器模式(interpreter)
     * 给定一个语言，定义它的文法的一种表示，并定义一个解释器，这个解析器使用该表示来解释语言中的句子
     * 如果一种特定类型的问题发生的频率足够高，那么可能就值得将该问题的各个实例表述为一个简单语言的句子，这样就可以构建一个解释器，该解释器通过解释这些句子来解决该问题
     */

    /*当有一个语言需要解释执行，并且你可将该语言中的句子表示为一个抽象语法树时，可使用解释器模式
     * 使用解释器模式，可以很容易地改变和扩展文法，因为该模式使用类来表示文法规则，你可使用继承来改变或扩展该文法。也比较容易实现文法，因为定义抽象语法树中各个节点的类的实现大体类似，这些类都易于直接编写
     * 解释器模式也有不足，解释器模式为文法中的每一条规则至少定义了一个类，因此包含许多规则的文法可能难以管理和维护。建议当文法非常复杂时，使用其他的技术如语法分析程序或编译器生成器来处理
     */

    /// <summary>
    /// 声明一个抽象的解析操作，这个接口为 抽象语法树中所有的节点所共享
    /// </summary>
    abstract class AbstractExpression
    {
        public abstract void Interpret(Context context);
    }

    /// <summary>
    /// 终结符表达式，实现与文法中的终结符相关联的解释操作
    /// </summary>
    class TerminalExpression : AbstractExpression
    {
        public override void Interpret(Context context)
        {
            Console.WriteLine("终端解释器");
        }
    }

    /// <summary>
    /// 非终结符表达式，为文法中的非终结符实现解释操作。对文法中每一条规则R1、R2。。。Rn都需要一个具体的非终结符表达式类 
    /// </summary>
    class NonterminalExpression : AbstractExpression
    {
        public override void Interpret(Context context)
        {
            Console.WriteLine("非终端解释器");
        }
    }

    /// <summary>
    /// 包含解释器之外的一些局部信息
    /// </summary>
    public class Context
    {
        public string Input { get; set; }

        public string Output { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PlayerContext context = new PlayerContext();
            Console.WriteLine("上海滩：");
            context.PlayText = "O 2 E 0.5 A 3 E 0.5 G 0.5 D 3 E 0.5 G 0.5 A 0.5 O 3 C 1 O 2 A 0.5 G 1 C 0.5 E 0.5 D 3";
            Expression expression = null;
            try
            {
                while (context.PlayText.Length > 0)
                {
                    string str = context.PlayText.Substring(0, 1);
                    switch (str)
                    {
                        case "O": expression = new Scale(); break;
                        case "C":
                        case "D":
                        case "E":
                        case "F":
                        case "G":
                        case "A":
                        case "B": expression = new Note(); break;
                        default:
                            break;
                    }
                    expression.Interpret(context);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }

    class PlayerContext
    {
        public string PlayText { get; set; }
    }

    abstract class Expression
    {
        public void Interpret(PlayerContext context)
        {
            if (context.PlayText.Length == 0)
            {
                return;
            }
            string playKey = context.PlayText.Substring(0, 1);
            context.PlayText = context.PlayText.Substring(2);
            double playValue = double.Parse(context.PlayText.Substring(0, context.PlayText.IndexOf(" ")));
            context.PlayText = context.PlayText.Substring(context.PlayText.IndexOf(" "), 1);

            Excute(playKey, playValue);
        }

        public abstract void Excute(string key, double value);
    }

    class Note : Expression
    {
        public override void Excute(string key, double value)
        {
            string note = "";
            switch (key)
            {
                case "C":
                    note = "1";
                    break;
                case "D":
                    note = "2";
                    break;
                case "E":
                    note = "3";
                    break;
                case "F":
                    note = "4";
                    break;
                case "G":
                    note = "5";
                    break;
                case "A":
                    note = "6";
                    break;
                case "B":
                    note = "7";
                    break;
            }
            Console.Write(note + " ");
        }
    }

    class Scale : Expression
    {
        public override void Excute(string key, double value)
        {
            string scale = "";
            switch (Convert.ToInt32(value))
            {
                case 1: scale = "低音"; break;
                case 2: scale = "中音"; break;
                case 3: scale = "高音"; break;
                default:
                    break;
            }
            Console.Write(scale + " ");
        }
    }
}
