using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfApp1;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            var eve = new EventAggregator();
            var pub = new Publisher(eve);
            var sub = new Subscriber(eve);
            var stopTimer = new Stopwatch();
            WpfApp1.ServiceReference1.EventDetails eventDetails;
            
            var proxy = new WpfApp1.ServiceReference1.Service1Client();
            var eventStream = proxy.GetData(new WpfApp1.ServiceReference1.EventDetails() { Name = "Door1",Timestamp = DateTime.Now });
            stopTimer.Start();
            using (var stringReader = new StringReader(eventStream))
            {
                var serializer = new XmlSerializer(typeof(WpfApp1.ServiceReference1.EventDetails));
                eventDetails = serializer.Deserialize(stringReader) as WpfApp1.ServiceReference1.EventDetails;

                pub.PublishMessage(eventDetails);
                stopTimer.Stop();
            }

            Assert.IsTrue(stopTimer.ElapsedMilliseconds < 5, "Took more than 5 seconds");
        }
    }
}
