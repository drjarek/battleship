using System;

namespace GameEngine.Domain
{
    public class IdGenerator : IIdGenerator
    {
        public string New()
        {
            return Guid.NewGuid().ToString();
        }
    }

    public interface IIdGenerator
    {
        public string New();
    }
}