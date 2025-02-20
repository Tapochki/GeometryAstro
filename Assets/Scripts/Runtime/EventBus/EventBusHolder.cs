namespace TandC.GeometryAstro.EventBus
{
    public class EventBusHolder
    {
        public EventBus EventBus { get; private set; }

        public EventBusHolder() 
        {
            EventBus = new EventBus();
        }
    }
}