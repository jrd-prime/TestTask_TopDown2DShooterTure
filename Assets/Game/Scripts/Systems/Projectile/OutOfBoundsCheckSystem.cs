using System;
using Game.Scripts.Help;
using Game.Scripts.Managers;
using UnityEngine;
using VContainer;

namespace Game.Scripts.Systems.Projectile
{
    public sealed class OutOfBoundsCheckSystem
    {
        private Camera _mainCamera;

        [Inject]
        private void Construct(ICameraManager cameraManager) => _mainCamera = cameraManager.GetMainCamera();

        public bool CheckOutOfBounds(Vector3 position)
        {
            if (!_mainCamera) throw new Exception("Camera is null");
            if (!_mainCamera.orthographic) throw new Exception("Camera is not orthographic");

            var verticalSize = _mainCamera.orthographicSize;
            var horizontalSize = verticalSize * _mainCamera.aspect;

            var cameraPosition = _mainCamera.transform.position;

            var left = cameraPosition.x - horizontalSize;
            var right = cameraPosition.x + horizontalSize;
            var bottom = cameraPosition.y - verticalSize;
            var top = cameraPosition.y + verticalSize;

#if UNITY_EDITOR
            DrawLines.DrawOutOfBounds(_mainCamera);
#endif

            return position.x < left || position.x > right || position.y < bottom || position.y > top;
        }
    }
}
