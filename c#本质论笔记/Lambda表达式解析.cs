using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzeExpressionTree
{
    class Program
    {
        static void Main(string[] args)
        {
            Expression<Func<Person, bool>> expression;
            expression = p => (p.Age > 10 && p.Age < 15) || (p.Age > 85 && p.Age < 100) && p.Name.Length == 10;
            PrintNode(expression, 0);

            Console.ReadLine();
        }

        public static void PrintNode(Expression expression, int indent)
        {
            if (expression is LambdaExpression)
            {
                PrintNode(expression as LambdaExpression, indent);
            }
            else
            {
                PrintSingle(expression, indent);
            }
        }

        private static void PrintNode(LambdaExpression expression, int indent)
        {

        }

        private static void PrintNode(BinaryExpression expression, int indent)
        {
            PrintNode(expression.Left, indent + 1);
            PrintSingle(expression, indent);
            PrintNode(expression.Right, indent + 1);
        }

        private static void PrintSingle(Expression expression, int indent)
        {
            Console.WriteLine("{0," + indent * 5 + "}{1}", "", NodeToString(expression));
        }

        private static string NodeToString(Expression expression)
        {
            var ret = string.Empty;
            switch (expression.NodeType)
            {
                case ExpressionType.Add:                
                    ret = "+";
                    break;
                case ExpressionType.AndAlso:
                    ret = "&&";
                    break;
                case ExpressionType.Divide:
                    ret = "/";
                    break;
                case ExpressionType.Equal:
                    ret = "=";
                    break;
                case ExpressionType.ExclusiveOr:
                    break;
                case ExpressionType.GreaterThan:
                    ret = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    ret = ">=";
                    break;                
                case ExpressionType.LessThan:
                    ret = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    ret = "<=";
                    break;               
                case ExpressionType.Modulo:
                    ret = "%";
                    break;
                case ExpressionType.Multiply:
                    ret = "*";
                    break;                                
                case ExpressionType.NotEqual:
                    ret = "!=";
                    break;
                case ExpressionType.OrElse:
                    ret = "||";
                    break;
                case ExpressionType.Subtract:
                    ret = "-";
                    break;
                default:
                    ret = expression.ToString() + "(" + expression.NodeType.ToString() + ")";
                    break;
            }
            return ret;
        }
    }
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
