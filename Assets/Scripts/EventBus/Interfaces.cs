namespace TandC.EventBus 
{
    public interface IEvent { }

    public interface IBaseEventReceiver
    {
        public UniqueId Id { get; }
    }

    public interface IEventReceiver<T> : IBaseEventReceiver where T : struct, IEvent
    {
        void OnEvent(T @event);
    }
}

