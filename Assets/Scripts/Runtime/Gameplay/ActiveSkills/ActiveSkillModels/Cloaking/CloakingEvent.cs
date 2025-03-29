namespace TandC.GeometryAstro.EventBus
{
    public readonly struct CloakingEvent : IEvent
    {
        public readonly bool IsActive;
        public readonly bool IsEvolved;

        public CloakingEvent(bool isActive, bool isEvolved)
        {
            IsActive = isActive;
            IsEvolved = isEvolved;
        }
    }
}

