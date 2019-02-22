using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ServiceReference1;

namespace WpfApp1
{
    public class Publisher
    {
        EventAggregator EventAggregator;
        public Publisher(EventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }

        public void PublishMessage(EventDetails eventDetails)
        {
            EventAggregator.Publish(eventDetails);
        }
    }
}
