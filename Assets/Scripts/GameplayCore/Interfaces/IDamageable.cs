namespace GameplayCore
{
    /// <summary>
    /// Implement this on any enemy that can receive damage.
    /// </summary>
    public interface IDamageable
    {
        void TakeDamage(float damage, float critChance = 0f, float critMultiplier = 1f);
    }
}
