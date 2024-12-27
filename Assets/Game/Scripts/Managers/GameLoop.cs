using System;
using System.Threading.Tasks;
using VContainer;

namespace Game.Scripts.Managers
{
    public interface IGameLoop
    {
        public void StartGame();
        public void RestartGameAsync();
    }

    public sealed class GameLoop : IGameLoop
    {
        private IUnitsManager _unitsManager;
        private ISpawnManager _spawnManager;

        private const int GameRestartDelayMs = 1000;

        [Inject]
        private void Construct(IUnitsManager unitsManager, ISpawnManager spawnManager)
        {
            _unitsManager = unitsManager;
            _spawnManager = spawnManager;
        }

        public void StartGame()
        {
            _unitsManager.SetGameRunning(true);
            _unitsManager.ActivateUnits();
        }

        public async void RestartGameAsync()
        {
            try
            {
                _spawnManager.DespawnProjectiles();
                _unitsManager.DeactivateUnits();
                _unitsManager.ActivateUnits();
                _unitsManager.ResetUnits();

                await Task.Delay(GameRestartDelayMs);
                _unitsManager.SetGameRunning(true);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
