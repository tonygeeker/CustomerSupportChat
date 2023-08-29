using CustomerSupportChatApi.BusinessLayer.Interfaces;
using CustomerSupportChatApi.DataLayer.Models;
using System.Collections;
using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace CustomerSupportChatApi.BusinessLayer.Services
{
    public class ObservableQueueWrapper<T>: IObservableQueueWrapper<T>
    {
        private readonly Queue<T> queue = new Queue<T>();
        public event EventHandler Enqueuedhandler;
        protected virtual void OnEnqueued()
        {
            if (Enqueuedhandler != null)
                Enqueuedhandler(this, EventArgs.Empty);
        }
        public virtual void Enqueue(T item)
        {
            queue.Enqueue(item);
            OnEnqueued();
        }
        public int Count()
        {
            return queue.Count;
        }
        public virtual T Dequeue()
        {
            T item = queue.Dequeue();
            OnEnqueued();
            return item;
        }
    }
}
