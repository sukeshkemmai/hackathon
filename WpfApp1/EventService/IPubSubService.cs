using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EventService
{
    [ServiceContract(Namespace = "http://ListPublishSubscribe.Service",
SessionMode = SessionMode.Required, CallbackContract = typeof
(IPubSubContract))]
    public interface IPubSubService
    {
        [OperationContract(IsOneWay = false, IsInitiating = true)]
        void Subscribe();
        [OperationContract(IsOneWay = false, IsInitiating = true)]
        void Unsubscribe();
        [OperationContract(IsOneWay = false)]
        void PublishNameChange(string Name);
    }
}
