using UnityEngine;

namespace Game.Scripts.Help
{
    public static class DrawLines
    {
#if UNITY_EDITOR
        public static void DrawOutOfBounds(Camera mainCamera)
        {
            if (!mainCamera) return;

            var verticalSize = mainCamera.orthographicSize;
            var horizontalSize = verticalSize * mainCamera.aspect;

            var cameraPosition = mainCamera.transform.position;

            var bottomLeft = new Vector3(cameraPosition.x - horizontalSize, cameraPosition.y - verticalSize, 0);
            var bottomRight = new Vector3(cameraPosition.x + horizontalSize, cameraPosition.y - verticalSize, 0);
            var topLeft = new Vector3(cameraPosition.x - horizontalSize, cameraPosition.y + verticalSize, 0);
            var topRight = new Vector3(cameraPosition.x + horizontalSize, cameraPosition.y + verticalSize, 0);

            Debug.DrawLine(bottomLeft, bottomRight, Color.green);
            Debug.DrawLine(bottomRight, topRight, Color.green);
            Debug.DrawLine(topRight, topLeft, Color.green);
            Debug.DrawLine(topLeft, bottomLeft, Color.green);
        }
#endif

#if UNITY_EDITOR
        public static void DrawBoxCast(Vector2 muzzlePosition, Vector2 directionToTarget, float distanceToTarget,
            Vector2 projectileColliderSize)
        {
            var halfSize = projectileColliderSize / 2f;

            var right = new Vector2(directionToTarget.y, -directionToTarget.x).normalized * halfSize.x;
            var up = directionToTarget * halfSize.y;
            const float duration = 1f;

            Debug.DrawLine(muzzlePosition + right + up,
                muzzlePosition + right - up + directionToTarget * distanceToTarget, Color.blue, duration);
            Debug.DrawLine(muzzlePosition - right + up,
                muzzlePosition - right - up + directionToTarget * distanceToTarget, Color.blue, duration);
            Debug.DrawLine(muzzlePosition - right - up + directionToTarget * distanceToTarget,
                muzzlePosition + right - up + directionToTarget * distanceToTarget, Color.blue, duration);
            Debug.DrawLine(muzzlePosition + right + up,
                muzzlePosition + right + up + directionToTarget * distanceToTarget, Color.blue, duration);
        }
#endif
    }
}
