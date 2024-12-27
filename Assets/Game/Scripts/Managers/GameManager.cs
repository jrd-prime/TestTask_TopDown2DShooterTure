using System;
using Game.Scripts.Input;
using Game.Scripts.PhysicsObjs.Character;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Managers
{
    public sealed class GameManager : MonoBehaviour
    {
        private ISpawnManager _spawnManager;
        private IUserInput _input;
        private IPointsManager _pointsManager;
        private IGameLoop _gameLoop;
        private IUnitsManager _unitsManager;

        private Action<ICharacter> _gameManagerCallback;

        [Inject]
        private void Construct(IObjectResolver resolver)
        {
            _pointsManager = resolver.Resolve<IPointsManager>();
            _spawnManager = resolver.Resolve<ISpawnManager>();
            _gameLoop = resolver.Resolve<IGameLoop>();
            _unitsManager = resolver.Resolve<IUnitsManager>();
        }

        private void Start()
        {
            _gameManagerCallback += OnCharacterDeath;
            _spawnManager.Init(_gameManagerCallback);

            _gameLoop.StartGame();
        }

        private void OnCharacterDeath(ICharacter character)
        {
            _pointsManager.AddPointsOnTargetDeath(character);
            _unitsManager.SetGameRunning(false);
            _gameLoop.RestartGameAsync();
        }

        private void OnDestroy() => _gameManagerCallback -= OnCharacterDeath;
    }
}
