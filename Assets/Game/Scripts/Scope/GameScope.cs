using System;
using Game.Scripts.Factory;
using Game.Scripts.Managers;
using Game.Scripts.Systems.Enemy;
using Game.Scripts.Systems.Player;
using Game.Scripts.Systems.Projectile;
using Game.Scripts.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Scope
{
    public class GameScope : LifetimeScope
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private SpawnManager spawnManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private FieldManager fieldManager;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<ObjFactory>(Lifetime.Singleton);

            if (gameManager == null)
                throw new MissingReferenceException($"You must add {nameof(gameManager)} to {nameof(GameScope)}");
            if (spawnManager == null)
                throw new MissingReferenceException($"You must add {nameof(spawnManager)} to {nameof(GameScope)}");
            if (uiManager == null)
                throw new MissingReferenceException($"You must add {nameof(uiManager)} to {nameof(GameScope)}");
            if (fieldManager == null)
                throw new MissingReferenceException($"You must add {nameof(fieldManager)} to {nameof(GameScope)}");


            builder.RegisterComponent(gameManager).AsSelf();
            builder.RegisterComponent(spawnManager).As<ISpawnManager>();
            builder.RegisterComponent(uiManager).As<IUIManager>();
            builder.RegisterComponent(fieldManager).AsSelf();

            builder.Register<PlayerMovementSystem>(Lifetime.Singleton)
                .As<IFixedTickable, IInitializable, IDisposable>();

            builder.Register<IPointsManager, PointsManager>(Lifetime.Singleton).As<IDisposable>();
            builder.Register<IPointsUIModel, PointsUIModel>(Lifetime.Singleton).As<IInitializable, IDisposable>();

            builder.Register<OutOfBoundsCheckSystem>(Lifetime.Singleton).AsSelf();
            builder.Register<EnemyMovementSystem>(Lifetime.Singleton).AsSelf().As<IInitializable>();
            builder.Register<EnemyFollowSystem>(Lifetime.Singleton).AsSelf().As<IFixedTickable, IPostStartable>();
            builder.Register<EnemyFiringDecisionSystem>(Lifetime.Singleton).AsSelf();

            builder.Register<IUnitsManager, UnitsManager>(Lifetime.Singleton).As<IInitializable>();
            builder.Register<IGameLoop, GameLoop>(Lifetime.Singleton);
        }
    }
}
