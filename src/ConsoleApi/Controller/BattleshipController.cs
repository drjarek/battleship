using System;
using GameEngine.Application;
using GameEngine.Application.GunApp.Command;
using GameEngine.Domain;

namespace ConsoleApi.Controller
{
    public class BattleshipController
    {
        private readonly ICommandHandler<FireMissileCommand> _handler;
        private readonly IIdGenerator _idGenerator;

        private const string AllowedColumns = "ABCDEFGHIJK";

        public BattleshipController(ICommandHandler<FireMissileCommand> handler, IIdGenerator idGenerator)
        {
            _handler = handler;
            _idGenerator = idGenerator;
        }
        
        public void Fire(string coordinates, string gameId, string playerId)
        {
            if (coordinates.Length is > 3 or 0)
            {
                ThrowException();
            }

            var columnName = coordinates[0].ToString().ToUpper();
            if (!AllowedColumns.Contains(columnName))
            {
                ThrowException();
            }

            var column = AllowedColumns.IndexOf(columnName, StringComparison.Ordinal);

            var rowName = coordinates[1..];
            if (!int.TryParse(rowName, out var row))
            {
                ThrowException();
            }

            if (row is 0 or > 10)
            {
                ThrowException();
            }

            row -= 1;
            
            _handler.Handle(new FireMissileCommand(_idGenerator.New(), gameId, playerId, row, column));
        }

        private static void ThrowException()
        {
            throw new Exception("Invalid coordinates");
        }
    }
}