using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using GameEngine.Domain.Exception;
using GameEngine.Domain.Repository;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Repository.Model.Extensions;

namespace GameEngine.Domain
{
    public class BattleshipGenerator : IBattleshipGenerator
    {
        private readonly IGameSettingsRepository _gameSettingsRepository;
        private readonly IBattleshipRepository _battleshipRepository;
        private readonly IIdGenerator _idGenerator;

        public BattleshipGenerator(
            IGameSettingsRepository gameSettingsRepository,
            IBattleshipRepository battleshipRepository,
            IIdGenerator idGenerator)
        {
            _gameSettingsRepository = gameSettingsRepository;
            _battleshipRepository = battleshipRepository;
            _idGenerator = idGenerator;
        }

        public Battleship Generate(int size, string playerId, string gameId)
        {
            var gameSettings = _gameSettingsRepository.Get(gameId);
            if (gameSettings == null)
            {
                throw new NotFoundException($"Game settings for '{gameId}' id not found.");
            }
            var existingBattleships = _battleshipRepository.GetAllPlayerBattleships(gameId, playerId);
            
            //0 - horizontal
            //1 - vertical
            var isHorizontal = RandomNumberGenerator.GetInt32(0, 2) == 0;
            var maxRows = isHorizontal ? gameSettings.MaxRows : gameSettings.MaxRows - size + 1;
            var maxColumns = isHorizontal ? gameSettings.MaxColumns - size + 1 : gameSettings.MaxColumns;

            var allTargets = CrateCoordinates(maxRows, maxColumns);
            var targets = RemoveCoordinatesThatCantBeUsed(allTargets, GetAllBattleshipsPositions(existingBattleships), size, isHorizontal);

            var startCoordinate = GetRandomCoordinate(targets);

            Coordinates endCoordinates;
            if (isHorizontal)
            {
                endCoordinates = new Coordinates
                {
                    Row = startCoordinate.Row,
                    Column = startCoordinate.Column + size - 1
                };
            }
            else
            {
                endCoordinates = new Coordinates
                {
                    Row = startCoordinate.Row + size - 1,
                    Column = startCoordinate.Column
                };
            }
            
            return new Battleship
            {
                Id = _idGenerator.New(),
                GameId = gameSettings.Id,
                PlayerId = playerId,
                Start = startCoordinate,
                End = endCoordinates
            };
        }

        private static IEnumerable<Coordinates> CrateCoordinates(int maxRows, int maxColumns)
        {
            var targets = new List<Coordinates>();
            for (var rowIndex = 0; rowIndex < maxRows; rowIndex++ )
            {
                for(var columnIndex = 0; columnIndex < maxColumns; columnIndex++) 
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
            
        private static List<Coordinates> RemoveCoordinatesThatCantBeUsed(
            IEnumerable<Coordinates> coordinates,
            IReadOnlyCollection<Coordinates> occupiedCoordinates,
            int size,
            bool isHorizontal)
        {
            var unusedTargets = new List<Coordinates>();
            foreach (var oc in occupiedCoordinates)
            {
                for (var i = 1; i < size; i++)
                {
                    var row = oc.Row;
                    var column = oc.Column;
                    if (isHorizontal)
                    {
                        column -= i;
                    }
                    else
                    {
                        row -= i;
                    }
                
                    unusedTargets.Add(new Coordinates
                    {
                        Row = row,
                        Column = column
                    });
                }
                
            }

            return coordinates.Where(cor => 
                !occupiedCoordinates.Any(x => x.Row == cor.Row && x.Column == cor.Column)
                && !unusedTargets.Any(x => x.Row == cor.Row && x.Column == cor.Column)
                ).ToList();
        }

        private static List<Coordinates> GetAllBattleshipsPositions(IEnumerable<Battleship> battleships)
        {
            var positions = new List<Coordinates>();
            foreach (var battleship in battleships)
            {
                positions.AddRange(battleship.GetPositions());
            }

            return positions;
        }
        
        private static Coordinates GetRandomCoordinate(IReadOnlyList<Coordinates> coordinates)
        {
            var random = new Random();
            var index = random.Next(coordinates.Count);
            var target = coordinates[index];

            return target;
        }
        
    }
}