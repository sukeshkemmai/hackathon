using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static EventAggregator eve = new EventAggregator();
        static Publisher pub;
        static Subscriber sub;

        public MainWindow()
        {
            InitializeComponent();

            pub = new Publisher(eve);
            sub = new Subscriber(eve);
            
            var proxy = new ServiceReference1.Service1Client();
            var eventStream = proxy.GetData(new ServiceReference1.EventDetails() { Name = "Door1", Description = "Opened", Timestamp = DateTime.Now });

            ServiceReference1.EventDetails eventDetails;

            using (var stringReader = new StringReader(eventStream))
            {
                var serializer = new XmlSerializer(typeof(ServiceReference1.EventDetails));
                eventDetails = serializer.Deserialize(stringReader) as ServiceReference1.EventDetails;

                pub.PublishMessage(eventDetails);
            }


        }
    }
}
