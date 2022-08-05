using GameEngine.Domain.Exception;
using GameEngine.Domain.Repository;
using GameEngine.Domain.Repository.Model;

namespace GameEngine.Domain.Service
{
    public class GameService : IGameService
    {
        private readonly IGameSettingsRepository _gameSettingsRepository;

        public GameService(IGameSettingsRepository gameSettingsRepository)
        {
            _gameSettingsRepository = gameSettingsRepository;
        }
    
        public GameSettings Create(GameSettings gameSettings)
        {
            _gameSettingsRepository.Create(gameSettings);

            return gameSettings;
        }

        public void Update(GameSettings gameSettings)
        {
            _gameSettingsRepository.Update(gameSettings);
        }

        public GameSettings Get(string gameId)
        {
            var gameSettings = _gameSettingsRepository.Get(gameId);
            if (gameSettings == null)
            {
                throw new NotFoundException($"Game settings for '{gameId}' id not found");
            }

            return gameSettings;
        }

        public bool TrySetNewStatus(BattleInProgressStatus status)
        {
            if (!CanSetNewStatus(status))
            {
                return false;
            }
            
            var gameSettings = Get(status.GameId);
            gameSettings.GameStatus = GameStatus.BattleInProgress;
            _gameSettingsRepository.Update(gameSettings);

            return true;
        }

        private bool CanSetNewStatus(BattleInProgressStatus status)
        {
            var gameSettings = Get(status.GameId);
            var amount = gameSettings.AllowedBattleshipsSize.Count * gameSettings.NumberOfPlayers;
            if (amount != status.AmountOfBattleships)
            {
                return false;
            }

            return IsNewGameStatusValid(gameSettings.GameStatus, GameStatus.BattleInProgress);
        }

        public bool TrySetNewStatus(GameFinishedStatus status)
        {
            if (!CanSetNewStatus(status)) {
                return false;
            }
            
            var gameSettings = Get(status.GameId);
            gameSettings.GameStatus = GameStatus.Finished;
            _gameSettingsRepository.Update(gameSettings);

            return true;
        }

        private bool CanSetNewStatus(GameFinishedStatus status)
        {
            var gameSettings = Get(status.GameId);

            if (gameSettings.AllowedBattleshipsSize.Count != status.AmountOfDestroyedBattleships)
            {
                return false;
            }

            return IsNewGameStatusValid(gameSettings.GameStatus, GameStatus.Finished);
        }

        public bool TrySetNewStatus(SetupBattleshipStatus status)
        {
            if (!CanSetNewStatus(status)) {
                return false;
            }
            
            var gameSettings = Get(status.GameId);
            gameSettings.GameStatus = GameStatus.SetupBattleships;
            
            _gameSettingsRepository.Update(gameSettings);

            return true;
        }
        
        private bool CanSetNewStatus(SetupBattleshipStatus status)
        {
            var gameSettings = Get(status.GameId);

            if (gameSettings.NumberOfPlayers != status.AmountOfPlayers)
            {
                return false;
            }

            return IsNewGameStatusValid(gameSettings.GameStatus, GameStatus.SetupBattleships);
        }

        private static bool IsNewGameStatusValid(GameStatus actualStatus, GameStatus newGameStatus)
        {
            var newStatusValue = (int)newGameStatus;
            if (newStatusValue == 0) return false;

            var actualStatusValue = (int)actualStatus;

            return actualStatusValue + 1 == newStatusValue;
        }
    }
}

