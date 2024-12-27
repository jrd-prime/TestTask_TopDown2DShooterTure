using System;
using UnityEngine;
using VContainer.Unity;

namespace Game.Scripts.Managers
{
    public sealed class CameraManager : MonoBehaviour, ICameraManager, IInitializable
    {
        [SerializeField] private Camera mainCamera;

        public Camera GetMainCamera() => mainCamera;

        public void Initialize()
        {
            if (mainCamera == null) throw new NullReferenceException("Camera is not set!");
        }
    }

    public interface ICameraManager
    {
        public Camera GetMainCamera();
    }
}
