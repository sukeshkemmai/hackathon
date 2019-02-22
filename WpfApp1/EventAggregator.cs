using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class EventAggregator
    {
        private readonly object lockObj = new object();
        private Dictionary<Type, IList> subscriber;

        public EventAggregator()
        {
            subscriber = new Dictionary<Type, IList>();
        }

        public void Publish<EventDetails>(EventDetails eventDetails)
        {
            Type t = typeof(EventDetails);
            IList sublst;
            if (subscriber.ContainsKey(t))
            {
                lock (lockObj)
                {
                    sublst = new List<Subscription<EventDetails>>(subscriber[t].Cast<Subscription<EventDetails>>());
                }

                foreach (Subscription<EventDetails> sub in sublst)
                {
                    var action = sub.CreatAction();
                    if (action != null)
                        action(eventDetails);
                }
            }
        }

        public Subscription<EventDetails> Subscribe<EventDetails>(Action<EventDetails> action)
        {
            if (subscriber.Count > 9)
            {
                return null;
            }

            Type t = typeof(EventDetails);
            IList actionlst;
            var actiondetail = new Subscription<EventDetails>(action, this);

            lock (lockObj)
            {
                if (!subscriber.TryGetValue(t, out actionlst))
                {
                    actionlst = new List<Subscription<EventDetails>>();
                    actionlst.Add(actiondetail);
                    subscriber.Add(t, actionlst);
                }
                else
                {
                    actionlst.Add(actiondetail);
                }
            }

            return actiondetail;
        }

        public void UnSubscribe<EventDetails>(Subscription<EventDetails> subscription)
        {
            Type t = typeof(EventDetails);
            if (subscriber.ContainsKey(t))
            {
                lock (lockObj)
                {
                    subscriber[t].Remove(subscription);
                }
                subscription = null;
            }
        }

    }

    public class Subscription<EventDetails> : IDisposable
    {
        public readonly MethodInfo MethodInfo;
        private readonly EventAggregator EventAggregator;
        public readonly WeakReference TargetObjet;
        public readonly bool IsStatic;

        private bool isDisposed;
        public Subscription(Action<EventDetails> action, EventAggregator eventAggregator)
        {
            MethodInfo = action.Method;
            if (action.Target == null)
                IsStatic = true;
            TargetObjet = new WeakReference(action.Target);
            EventAggregator = eventAggregator;
        }

        ~Subscription()
        {
            if (!isDisposed)
                Dispose();
        }

        public void Dispose()
        {
            EventAggregator.UnSubscribe(this);
            isDisposed = true;
        }

        public Action<EventDetails> CreatAction()
        {
            if (TargetObjet.Target != null && TargetObjet.IsAlive)
                return (Action<EventDetails>)Delegate.CreateDelegate(typeof(Action<EventDetails>), TargetObjet.Target, MethodInfo);
            if (this.IsStatic)
                return (Action<EventDetails>)Delegate.CreateDelegate(typeof(Action<EventDetails>), MethodInfo);

            return null;
        }
    }

}
