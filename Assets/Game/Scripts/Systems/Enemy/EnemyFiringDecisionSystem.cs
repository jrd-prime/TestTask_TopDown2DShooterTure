using Game.Scripts.Help;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.Scripts.Systems.Enemy
{
    [UsedImplicitly]
    public sealed class EnemyFiringDecisionSystem
    {
        private readonly Vector2 _projectileColliderSize = new(0.2f, 0.2f);

        public bool IsTargetInSight(Vector2 muzzlePosition, Transform targetTransform)
        {
            Vector2 targetPosition = targetTransform.position;
            var directionToTarget = (targetPosition - muzzlePosition).normalized;
            var distanceToTarget = Vector2.Distance(muzzlePosition, targetPosition);

#if UNITY_EDITOR
            DrawLines.DrawBoxCast(muzzlePosition, directionToTarget, distanceToTarget, _projectileColliderSize);
#endif

            // Ignore self layer
            var layerMask = ~(1 << LayerMask.NameToLayer(LayerMaskConstants.EnemyLayerName));

            var hit = Physics2D.BoxCast(muzzlePosition, _projectileColliderSize, 0f, directionToTarget,
                distanceToTarget, layerMask);

            if (hit.collider == null) return false;

            return hit.collider.gameObject.layer == LayerMask.NameToLayer(LayerMaskConstants.PlayerLayerName);
        }
    }
}
