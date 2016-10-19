using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Reflection;

namespace 抽象工厂模式
{
    /*抽象工厂模式(Abstract Factory)
     * 提供一个创建一些列相关或相互依赖对象的接口，而无需指定它们具体的类
     */
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    class User
    {
        public int ID { get; set; }

        public string Name { get; set; }
    }

    interface IUser
    {
        void Insert(User user);

        User GetUser(int id);
    }

    class SqlServerUser : IUser
    {
        public User GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(User user)
        {
            throw new NotImplementedException();
        }
    }

    class AccessUser : IUser
    {
        public User GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(User user)
        {
            throw new NotImplementedException();
        }
    }

    class DataAccess
    {
        private static readonly string assemblyName = "抽象工厂模式";

        private static readonly string db = ConfigurationManager.AppSettings["DB"];

        public static IUser CreateUser()
        {
            string className = assemblyName + "." + db + "User";
            return (IUser)Assembly.Load(assemblyName).CreateInstance(className);
        }
    }
}
