namespace DotNetEvents
{
    using System;
    using System.Collections.Generic;
	
	//1���ڷ�������Ͷ��ķ�����ɿ����ķ�Χ�������Զ������ݵ��ࡣ  Ȼ����ӱ��������Զ����¼���������ĳ�Ա��
    //�������Ҫ���¼�һ�����Զ������ݣ�����Ҫ
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
		//2���ڷ�����������һ��ί�С�
        //ʹ�÷���EventHandler<TEventArgs>������Ҫ
        //public delegate void CustomEventHandler(object sender, CustomEventArgs a);
		
		//3�������¼�
		
		//δ�Զ���EventArgs �࣬�¼����;��ǷǷ��� EventHandler ί�С���������ί�У�ʡ�Բ���2
		//public event EventHandler RaiseCustomEvent;
		
		//���ʹ�õ��ǷǷ���EventHandler���������Զ��� EventArgs���ڷ������������¼���������2��ί����������
		//public event CustomEventHandler RaiseCustomEvent;
		
        //ʹ�÷���EventHandler������Ҫ�Զ���ί��ʡ�Բ���2
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