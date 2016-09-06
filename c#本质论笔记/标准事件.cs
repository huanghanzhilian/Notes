namespace DotNetEvents
{
    using System;
    using System.Collections.Generic;
	
	//1、在发行者类和订阅方类均可看见的范围中声明自定义数据的类。  然后添加保留您的自定义事件数据所需的成员。
    //如果不需要与事件一起发送自定义数据，则不需要
    public class CustomEventArgs : EventArgs
    {
        public CustomEventArgs(string s)
        {
            message = s;
        }
        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }

    // Class that publishes an event
    class Publisher
    {
		//2、在发布类中声明一个委托。
        //使用泛型EventHandler<TEventArgs>，则不需要
        //public delegate void CustomEventHandler(object sender, CustomEventArgs a);
		
		//3、声明事件
		
		//未自定义EventArgs 类，事件类型就是非泛型 EventHandler 委托。无需声明委托，省略步骤2
		//public event EventHandler RaiseCustomEvent;
		
		//如果使用的是非泛型EventHandler，并且有自定义 EventArgs，在发布类中声明事件并将步骤2的委托用作类型
		//public event CustomEventHandler RaiseCustomEvent;
		
        //使用泛型EventHandler，不需要自定义委托省略步骤2
        public event EventHandler<CustomEventArgs> RaiseCustomEvent;

        public void DoSomething()
        {
            // Write some code that does something useful here
            // then raise the event. You can also raise an event
            // before you execute a block of code.
            OnRaiseCustomEvent(new CustomEventArgs("Did something"));

        }

        // Wrap event invocations inside a protected virtual method
        // to allow derived classes to override the event invocation behavior
        protected virtual void OnRaiseCustomEvent(CustomEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<CustomEventArgs> handler = RaiseCustomEvent;

            // Event will be null if there are no subscribers
            if (handler != null)
            {
                // Format the string to send inside the CustomEventArgs parameter
                e.Message += String.Format(" at {0}", DateTime.Now.ToString());

                // Use the () operator to raise the event.
                handler(this, e);
            }
        }
    }

    //Class that subscribes to an event
    class Subscriber
    {
        private string id;
        public Subscriber(string ID, Publisher pub)
        {
            id = ID;
            // Subscribe to the event using C# 2.0 syntax
            pub.RaiseCustomEvent += HandleCustomEvent;
        }

        // Define what actions to take when the event is raised.
        void HandleCustomEvent(object sender, CustomEventArgs e)
        {
            Console.WriteLine(id + " received this message: {0}", e.Message);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Publisher pub = new Publisher();
            Subscriber sub1 = new Subscriber("sub1", pub);
            Subscriber sub2 = new Subscriber("sub2", pub);

            // Call the method that raises the event.
            pub.DoSomething();

            // Keep the console window open
            Console.WriteLine("Press Enter to close this window.");
            Console.ReadLine();
        }
    }
}