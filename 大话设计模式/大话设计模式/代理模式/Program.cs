using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 代理模式
{
    /*代理模式(Proxy)
     * 为其他对象提供一种代理以控制对这个对象的访问。
     */

    /// <summary>
    /// Subject类，定义了RealSubject和Proxy的公用接口，这样就在任何使用RealSubject的地方都可以使用Proxy
    /// </summary>
    abstract class Subject
    {
        public abstract void Request();
    }

    /// <summary>
    /// RealSubject类，定义Proxy所代表的真实实体
    /// </summary>
    class RealSubject : Subject
    {
        public override void Request()
        {
            Console.WriteLine("真实的请求");
        }
    }

    /// <summary>
    /// Proxy类，保存一个引用使得代理可以访问实体，并提供一个与Subject的接口相同的接口，这样代理就可以用来代替实体。
    /// </summary>
    class Proxy : Subject
    {
        RealSubject realSubject;
        public override void Request()
        {
            if (realSubject == null)
            {
                realSubject = new RealSubject();
            }
            realSubject.Request();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Proxy proxy = new Proxy();
            proxy.Request();
        }
    }

    /*代理模式应用
     * 1.远程代理，也就是为一个对象在不同的地址空间提供局部代表。这样可以隐藏一个对象存在于不同地址空间的事实。
     * 例子：.Net中的WebService。在项目中引用一个WebService，项目中生成一个WebReference的文件夹和一些文件，这就是代理。客户端程序调用代理就可以解决远程访问的问题。
     * 2.虚拟代理，是根据需要创建开销很大的对象。通过它来存放实例化需要很长时间的真是对象。
     * 例子：浏览器用代理模式来优化下载。
     * 3.安全代理，用来控制真是对象访问时的权限。
     * 4.智能指引，是指当调用真实的对象时，处理另外一些事。
     */
}
