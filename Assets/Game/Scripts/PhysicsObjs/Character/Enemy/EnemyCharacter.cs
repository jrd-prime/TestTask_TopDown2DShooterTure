using System;
using System.Threading.Tasks;
using Game.Scripts.Managers;
using Game.Scripts.Settings;
using Game.Scripts.Systems.Enemy;
using Pathfinding;
using UnityEngine;
using VContainer;

namespace Game.Scripts.PhysicsObjs.Character.Enemy
{
    public interface IEnemyCharacter : ICharacter
    {
        public void SetTarget(Transform targetTransform);
    }

    [RequireComponent(typeof(AIPath), typeof(AIDestinationSetter))]
    public class EnemyCharacter : CharacterBase, IEnemyCharacter
    {
        private AIDestinationSetter _destinationSetter;
        private AIPath _aiPath;
        private EnemyFiringDecisionSystem _enemyFiringDecisionSystem;

        private Transform _targetTransform;
        private bool _isFiringInProgress;
        private EnemySettings _enemySettings;
        private int _aimDelayMs;

        [Inject]
        private void Construct(EnemyFiringDecisionSystem enemyFiringDecisionSystem, ISettingsManager settingsManager)
        {
            _enemyFiringDecisionSystem = enemyFiringDecisionSystem;
            _enemySettings = settingsManager.GetSettings<EnemySettings>();
        }

        protected new void Awake()
        {
            base.Awake();
            _destinationSetter = GetComponent<AIDestinationSetter>();
            _aiPath = GetComponent<AIPath>();
        }
 
        private void Start()
        {
            if (_enemySettings == null) throw new NullReferenceException("EnemySettings is null");
            _aimDelayMs = _enemySettings.aimDelayMs;
            Speed = _enemySettings.speed;
            _aiPath.maxSpeed = Speed;
        }

        private void FixedUpdate()
        {
            if (!_isFiringInProgress) _aiPath.canMove = IsGameRunning;

            if (!gameObject.activeSelf || _isFiringInProgress) return;

            if (_enemyFiringDecisionSystem.IsTargetInSight(GetMuzzlePosition(), _targetTransform))
            {
                StopAndFireAsync();
            }
            /*  else{//TODO Target is not in sight. Mb ricochet?}*/
        }

        public void SetTarget(Transform targetTransform)
        {
            _targetTransform = targetTransform;

            if (_destinationSetter == null) throw new Exception("AIDestinationSetter is null");

            _destinationSetter.target = _targetTransform;
        }

        private async void StopAndFireAsync()
        {
            if (_isFiringInProgress) return;

            _isFiringInProgress = true;
            _aiPath.canMove = false;

            await Aim();

            Fire();

            _isFiringInProgress = false;
        }

        private async Task Aim() => await Task.Delay(_aimDelayMs);
    }
}
