using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Serialization;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public string GetData(EventDetails eventDetails)
        {
            using (var stringWriter = new StringWriter())
            {
                var ser = new XmlSerializer(typeof(EventDetails));
                ser.Serialize(stringWriter, eventDetails);

                return stringWriter.ToString();
            }

        }
        
    }

    public class EventDetails
    {
        string name;
        DateTime timeStamp;
        string desc;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public DateTime Timestamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

        public string Description
        {
            get { return desc; }
            set { desc = value; }
        }

    }

}
