using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace EventService
{
    public interface IPubSubContract
    {
        [OperationContract(IsOneWay = true)]
        void NameChange(string Name);
    }
}
