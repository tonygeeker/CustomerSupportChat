namespace CustomerSupportChatApi.BusinessLayer.Interfaces
{
    public interface IObservableQueueWrapper<T>
    {
        event EventHandler Enqueuedhandler;
        void Enqueue(T item);
        T Dequeue();
        int Count();
    }
}
