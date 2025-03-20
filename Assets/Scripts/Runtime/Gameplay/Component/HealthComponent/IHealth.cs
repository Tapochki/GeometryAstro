namespace TandC.GeometryAstro.Gameplay 
{
    public interface IHealth
    {
        float MaxHealth { get; }
        float CurrentHealth { get; }

        void TakeDamage(float amount);
        void Heal(float amount);
    }
}
