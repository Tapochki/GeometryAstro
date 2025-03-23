namespace TandC.GeometryAstro.Gameplay 
{
    public interface IEnemy : ITickable
    {
        public void Freeze(float freezeTimer = 3f);
    }
}

