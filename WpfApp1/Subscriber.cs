using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ServiceReference1;

namespace WpfApp1
{

    public class Subscriber
    {
        Subscription<EventDetails> eventDetailsSubscrption;
        EventAggregator eventAggregator;
        bool isValid = false;

        public Subscriber(EventAggregator eve)
        {
            eventAggregator = eve;
            eventDetailsSubscrption = eventAggregator.Subscribe<EventDetails>(this.Test);
            if (eventDetailsSubscrption != null)
            {
                isValid = true;
            }
        }

        private void Test(EventDetails test)
        {
            Console.WriteLine(test.ToString());

            //recieve the event and process it

        }

        ~Subscriber()
        {
            eventAggregator.UnSubscribe(eventDetailsSubscrption);
        }
    }
}
