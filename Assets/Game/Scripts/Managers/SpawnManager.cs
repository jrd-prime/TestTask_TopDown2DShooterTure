using System;
using System.Collections.Generic;
using Game.Scripts.Factory;
using Game.Scripts.PhysicsObjs.Character;
using Game.Scripts.PhysicsObjs.Character.Enemy;
using Game.Scripts.PhysicsObjs.Projectile;
using Game.Scripts.Settings;
using Game.Scripts.Weapon;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Managers
{
    public interface ISpawnManager
    {
        public ICharacter player { get; }
        public IEnemyCharacter enemy { get; }
        public void DespawnProjectiles();
        public void Init(Action<ICharacter> onDeathCallback);
    }

    public class SpawnManager : MonoBehaviour, ISpawnManager
    {
        public ICharacter player { get; private set; }
        public IEnemyCharacter enemy { get; private set; }

        private IObjectResolver _resolver;
        private ISettingsManager _settingsManager;

        private CustomPool<Projectile> _projectilesPool;
        private PlayerSettings _playerSettings;
        private EnemySettings _enemySettings;

        [Inject]
        private void Construct(IObjectResolver resolver)
        {
            _resolver = resolver;
            _settingsManager = resolver.Resolve<ISettingsManager>();

            CreateObjects();
        }

        private void CreateObjects()
        {
            var holder = new GameObject("ProjectilesHolder");


            var projectileSettings = _settingsManager.GetSettings<ProjectileSettings>();
            _projectilesPool =
                new CustomPool<Projectile>(projectileSettings.prefab, 30, holder.transform, _resolver, true);

            var factory = _resolver.Resolve<ObjFactory>();

            _playerSettings = _settingsManager.GetSettings<PlayerSettings>();
            player = factory.CreateAndInject(_playerSettings.prefab, _playerSettings.playerSpawnPosition);

            _enemySettings = _settingsManager.GetSettings<EnemySettings>();
            enemy = factory.CreateAndInject(_enemySettings.prefab, _enemySettings.enemySpawnPosition);

            List<ICharacter> units = new() { player, enemy };
            var unitsManager = _resolver.Resolve<IUnitsManager>();
            unitsManager.SetUnits(units);
        }

        public void Init(Action<ICharacter> onDeathCallback)
        {
            var playerWeapon = new PlayerWeapon(_projectilesPool);
            player.Initialize(_playerSettings.playerSpawnPosition, playerWeapon, onDeathCallback);

            var enemyWeapon = new EnemyWeapon(_projectilesPool);
            enemy.Initialize(_enemySettings.enemySpawnPosition, enemyWeapon, onDeathCallback);
        }

        public void DespawnProjectiles() => _projectilesPool.ReturnAll();
    }
}
