using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 单例模式
{
    /*单例模式(Singleton)
     * 保证一个类仅有一个实例，并提供一个访问它的全局访问点
     */

    /// <summary>
    /// Singleton类
    /// 定义一个GetInstance操作，允许客户访问它的唯一实例，GetInstance是一个静态方法，主要负责创建自己的唯一实例
    /// 要在第一次被引用时，才会将自己实例化，所以被称为懒汉
    /// </summary>
    class Singleton
    {
        private static Singleton instance;
        private static readonly object syncRoot = new object();

        private Singleton() { }

        public static Singleton GetInstance()
        {
            //双重锁定
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new Singleton();
                    }
                }
            }
            return instance;
        }
    }

    sealed class Singleton2
    {
        //静态初始化
        //在第一次引用类型的任何成员时创建实例。公共语言运行库负责处理变量初始化
        //静态初始化的方式是在自己被加载时就将自己实例化，所以形象地称之为饿汉式单例模式
        private static readonly Singleton2 instance = new Singleton2();
        private Singleton2() { }
        public static Singleton2 GetInstance()
        {
            return instance;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

        }
    }
}
