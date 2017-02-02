using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DelegateEventTest : MonoBehaviour {

    #region Delegate

    #region StartEngine
    delegate void StartEngine();

    //Use a List of A delegate to Impliment different function. 
    public void StartEngineTest()
    {
        foreach (StartEngine starters in GetVehicleStarts())
        {
            starters();
        }
    }

    List<StartEngine> GetVehicleStarts() {
        List<StartEngine> starters = new List<StartEngine>();
        starters.Add(Car.StartCar);
        starters.Add(Motorcycle.StartMotorcycle);
        starters.Add(Airplane.StartAirplane);
        return starters;
    }

    class Car
    {
        public static void StartCar()
        {
            Debug.Log("The car is starting");
        }
    }
    class Motorcycle
    {
        public static void StartMotorcycle()
        {
            Debug.Log("The Motorcycle is starting");
        }
    }
    class Airplane
    {
        public static void StartAirplane()
        {
            Debug.Log("The Airplane is starting");
        }
    }
    #endregion

    #region DelegateMessage
    //just think delegate like a interface which contains a single function
    public class DelegateManager
    {
        public static void DelegateMessage(string message)
        {
            Debug.Log(message + "DelegateManager::DelegateMessage ");
        }
    }
    //declares
    public delegate void Del(string message);
    //Instantiate the delegate
    Del handler = DelegateManager.DelegateMessage;
    public void DelMessageTestFunc() {
        handler("Called by handler in DelMessageTestFunc");
    }

    void DelMessageCallback(int param1, int param2, Del callBack)
    {
        callBack("The numer is : " + (param1 + param2).ToString() + " Called by DelMessageCallback");
    }

    public void DelMessageCallbackTest()
    {
        DelMessageCallback(1, 2, handler);
    }

    public void DelMessageTestFunc2()
    {
        DMFunc obj = new DMFunc();
        Del d1 = obj.DMFunc1; //get instance function
        Del d2 = obj.DMFunc2; //get instance function
        Del d3 = DelegateManager.DelegateMessage;//get static function

        Del allFunc;
        allFunc = d1 + d2;
        allFunc("AllFunc called In DelMessageTestFunc2 -->");

        Debug.Log("==========================");

        allFunc += d3;
        allFunc("AllFunc#2 called In DelMessageTestFunc2-->");

        Debug.Log("==========================");

        allFunc -= d1;
        allFunc("AllFunc#3 called In DelMessageTestFunc2-->");
    }
    public class DMFunc
    {
        public void DMFunc1(string message)
        {
            Debug.Log(message + "DMFunc1");
        }
        public void DMFunc2(string message)
        {
            Debug.Log(message + "DMFunc2");
        }
    }
    #endregion

    #endregion

    #region MyEventTest

    #region NormalEventType
    //normal type of Event need to use a delegate to handle publisher....
    class MessageTestEventArgs : EventArgs
    {
        private string message;
        public MessageTestEventArgs(string s)
        {
            message = s;
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }

    //Publisher will Send a eventArgs, and all Subscribers will recive eventArgs, and implimemt 
    //the function which register to Publisher's event
    class Publisher
    {
        public delegate void MessageEvent_Handler(object sender, MessageTestEventArgs args);

        public event MessageEvent_Handler raiseCustomEvent;

        public void SendEvent()
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            MessageEvent_Handler handler = raiseCustomEvent;

            if (handler != null)
            {
                MessageTestEventArgs args = new MessageTestEventArgs(" 'Hello MessageTestEventArgs' ");
                args.Message += "From Publisher in SendEvent function";
                handler(this, args);
            }
        }
    }

    class Subscriber
    {
        private string id;
        public Subscriber(string ID, Publisher pub)
        {
            id = ID;
            //register to the Publisher
            pub.raiseCustomEvent += SubMessageEvent;
        }
        //Define what actions to do when the Sender raise a Event
        void SubMessageEvent(object sender, MessageTestEventArgs args)
        {
            Debug.LogFormat(id + " recieved {0}", args.Message);
        }

        public void CleanWarrning() { }
    }

    public void EventTest()
    {
        Publisher pub = new Publisher();
        Subscriber sub1 = new Subscriber("sub1", pub);
        Subscriber sub2 = new Subscriber("sub2", pub);

        pub.SendEvent();
        sub1.CleanWarrning();//Do nothing just for clean warnning of variable never been used
        sub2.CleanWarrning();//Do nothing just for clean warnning of variable never been used
    }
    #endregion

    #region GenericEventType

    public class GMessageEventArgs : EventArgs
    {
        private string message;
        public GMessageEventArgs(string s)
        {
            message = s;
        }
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }

    //Publisher will Send a eventArgs, and all Subscribers will recive eventArgs, and implimemt 
    //the function which register to Publisher's event
    class GPublisher
    {
        public event EventHandler<GMessageEventArgs> MessageEventHandler;

        public void SendGEvent()
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<GMessageEventArgs> handler = MessageEventHandler;

            if (handler != null)
            {
                GMessageEventArgs args = new GMessageEventArgs("'Hello GMessageEventArgs' ");
                args.Message += "from GPublisher SendMessage Function";
                handler(this, args);
            }
        }
    }

    class GSubscriber
    {
        private string id;
        public GSubscriber(string ID, GPublisher pub)
        {
            id = ID;
            pub.MessageEventHandler += HandlerGMessageEvent;
        }
        void HandlerGMessageEvent(object sender, GMessageEventArgs args)
        {
            Debug.Log(id + " Received message "+args.Message);
        }

        public void CleanWarrning() { }

    }

    public void GMessageTest()
    {
        GPublisher pub = new GPublisher();
        GSubscriber sub1 = new GSubscriber("sub1",pub);
        GSubscriber sub2 = new GSubscriber("sub2", pub);
        pub.SendGEvent();

        sub1.CleanWarrning();
        sub2.CleanWarrning();
    }

    #endregion

    #region EventDerivedClasses

    public class ShapEventArgs : EventArgs
    {
        private double newArea;
        public ShapEventArgs(double a)
        {
            newArea = a;
        }
        public double NewArea
        {
            get { return newArea; }
        }
    }

    public abstract class Shap
    {
        protected double area;
        public double Area
        {
            get { return area; }
            set { area = value; }
        }
        public event EventHandler<ShapEventArgs> ShapeChangeEventHandler;
        public abstract void Draw();
        protected virtual void SendShapChangedEvent(ShapEventArgs args)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<ShapEventArgs> handler = ShapeChangeEventHandler;
            if (handler != null)
            {
                handler(this, args);
            }
        }
    }

    public class Circle : Shap
    {
        private double radius;
        public Circle(double r)
        {
            radius = r;
            area = 3.141592654 * radius * radius;
        }
        public void Update(double r)
        {
            radius = r;
            area = 3.141592654 * radius * radius;
            ShapEventArgs args = new ShapEventArgs(area);
            SendShapChangedEvent(args);
        }
        protected override void SendShapChangedEvent(ShapEventArgs args)
        {
            base.SendShapChangedEvent(args);
        }

        public override void Draw()
        {
            Debug.Log("Drawing a circle");
        }
    }

    public class ShapSubScriber
    {
        string id;

        public ShapSubScriber(string ID,Shap s)
        {
            id = ID;
            s.ShapeChangeEventHandler += HandleShapChangedEvent;
        }

        public void HandleShapChangedEvent(object sender, ShapEventArgs args)
        {
            Shap s = (Shap)sender;
            Debug.Log(id + " Received a Event from Circle; get new area is now " + args.NewArea);
            s.Draw();
        }

        public void CleanWarrning() { }

    }

    public void ShapEventTest()
    {
        Circle c1 = new Circle(52);
        ShapSubScriber CircleSub1 = new ShapSubScriber("CircleSub1", c1);

        c1.Update(99);//this call will trigger a event

        CircleSub1.CleanWarrning();

    }

    public class ShapContainer
    {
        List<Shap> shaps;
        public ShapContainer()
        {
            shaps = new List<Shap>();
        }
        public void AddShape(Shap s)
        {
            shaps.Add(s);
            s.ShapeChangeEventHandler += HandleShapChangedEvent;
        }
        public void HandleShapChangedEvent(object sender, ShapEventArgs args)
        {
            Shap s = (Shap)sender;
            Debug.Log(" Received a Event from Circle; get new area is now " + args.NewArea);
            s.Draw();
        }

        public void CleanWarrning() { }
    }

    public void ShapContainerTest()
    {
        Circle c1 = new Circle(20);
        Circle c2 = new Circle(15);
        ShapContainer container = new ShapContainer();
        container.AddShape(c1);
        container.AddShape(c2);

        c1.Update(55);
        c2.Update(88);
    }
    #endregion

    #endregion
}
