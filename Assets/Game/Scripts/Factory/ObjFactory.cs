using System;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Game.Scripts.Factory
{
    public sealed class ObjFactory
    {
        private IObjectResolver _resolver;

        [Inject]
        private void Construct(IObjectResolver resolver) => _resolver = resolver;

        public T CreateAndInject<T>(T prefab, Transform spawnPositionTransform, Transform parent = null)
            where T : MonoBehaviour
        {
            if (prefab == null) throw new ArgumentNullException(nameof(prefab), "Prefab cannot be null.");

            var spawnPosition = spawnPositionTransform ? spawnPositionTransform.position : Vector3.zero;
            var spawnRotation = spawnPositionTransform ? spawnPositionTransform.rotation : Quaternion.identity;

            var obj = Object.Instantiate(prefab, spawnPosition, spawnRotation, parent: parent);
            var component = obj.GetComponent<T>() ??
                            throw new MissingComponentException(
                                $"Component of type {typeof(T).Name} not found on instantiated object {obj.name}.");
            _resolver.Inject(component);
            return component;
        }
    }
}
