using Game.Scripts.Managers;
using Game.Scripts.PhysicsObjs.Character;
using Game.Scripts.PhysicsObjs.Character.Enemy;
using R3;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Systems.Enemy
{
    public sealed class EnemyFollowSystem : IFixedTickable, IPostStartable
    {
        public ReactiveProperty<Vector2> TargetPosition { get; } = new(Vector2.zero);

        private IEnemyCharacter _enemy;
        private ICharacter _player;
        private ISpawnManager _spawnManager;

        [Inject]
        private void Construct(ISpawnManager spawnManager) => _spawnManager = spawnManager;

        public void PostStart()
        {
            _enemy = _spawnManager.enemy;
            _player = _spawnManager.player;
            var targetTransform = _player.GetTransform();
            _enemy.SetTarget(targetTransform);
        }

        public void FixedTick()
        {
            Vector2 targetPosition = _player.GetTransform().position;
            if (TargetPosition.CurrentValue != targetPosition) TargetPosition.Value = targetPosition;
        }
    }
}
