namespace TandC.GeometryAstro.EventBus
{
    public static class EventBusHolder
    {
        private static readonly EventBus _eventBus = new EventBus();

        public static EventBus EventBus => _eventBus;
    }
}