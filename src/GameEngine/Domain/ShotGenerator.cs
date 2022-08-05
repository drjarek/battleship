using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.Domain.Repository.Model;

namespace GameEngine.Domain
{
    public class ShotGenerator
    {
        private readonly GameSettings _gameSettings;

        public ShotGenerator(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }

        public Missile Generate(string playerId, IEnumerable<Missile> missiles)
        {
            var targets = CrateTargets();
            var missileCoordinates = GetCoordinates(missiles);
            
            targets = RemoveHitTargets(targets, missileCoordinates);
            var target = GetRandomTarget(targets);
        
            var shot = new Missile
            {
                GameId = _gameSettings.Id,
                PlayerId = playerId,
                Row = target.Row,
                Column = target.Column
            };

            return shot;
        }

        private static List<Coordinates> GetCoordinates(IEnumerable<Missile> missiles)
        {
            return missiles.Select(missile => new Coordinates { Row = missile.Row, Column = missile.Column }).ToList();
        }

        private List<Coordinates> CrateTargets()
        {
            var targets = new List<Coordinates>();
            for (var rowIndex = 0; rowIndex < _gameSettings.MaxRows; rowIndex++ )
            {
                for(var columnIndex = 0; columnIndex < _gameSettings.MaxColumns; columnIndex++) 
                {
                    targets.Add(new Coordinates
                    {
                        Row = rowIndex,
                        Column = columnIndex
                    });
                }
            }

            return targets;
        }
        
        private static List<Coordinates> RemoveHitTargets(IEnumerable<Coordinates> targets, IReadOnlyCollection<Coordinates> missiles)
        {
            return targets.Where(target => !missiles.Contains(target)).ToList();
        }

        private static Coordinates GetRandomTarget(IReadOnlyList<Coordinates> targets)
        {
            var random = new Random();
            var index = random.Next(targets.Count);
            var target = targets[index];

            return target;
        }
    }
}

