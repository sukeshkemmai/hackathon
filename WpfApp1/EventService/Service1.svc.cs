using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace EventService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PubSubService : IPubSubService
    {
        public delegate void NameChangeEventHandler(object sender, ServiceEventArgs e);
        public static event NameChangeEventHandler NameChangeEvent;

        IPubSubContract ServiceCallback = null;
        NameChangeEventHandler NameHandler = null;

        public void Subscribe()
        {
            ServiceCallback = OperationContext.Current.GetCallbackChannel<IPubSubContract>();
            NameHandler = new NameChangeEventHandler(PublishNameChangeHandler);
            NameChangeEvent += NameHandler;
        }

        public void Unsubscribe()
        {
            NameChangeEvent -= NameHandler;
        }

        public void PublishNameChange(string Name)
        {
            ServiceEventArgs se = new ServiceEventArgs();
            se.Name = Name;
            NameChangeEvent(this, se);
        }

        public void PublishNameChangeHandler(object sender, ServiceEventArgs se)
        {
            ServiceCallback.NameChange(se.Name);

        }
    }

    [CallbackBehaviorAttribute(UseSynchronizationContext = false)]
    public class ServiceCallback : IPubSubServiceCallback
    {
        public void NameChange(string Name)
        {
            Client.MyEventCallbackEvent(Name);
        }
    }
}
