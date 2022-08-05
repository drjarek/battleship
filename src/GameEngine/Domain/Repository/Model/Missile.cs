namespace GameEngine.Domain.Repository.Model
{
    public class Missile : MissileWithoutCoordinates
    {
        public int Row { get; init; }
    
        public int Column { get; init; }
    }
}

