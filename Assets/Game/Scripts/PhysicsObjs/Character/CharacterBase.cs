using System;
using Game.Scripts.Weapon;
using UnityEngine;

namespace Game.Scripts.PhysicsObjs.Character
{
    public abstract class CharacterBase : PhysicsObject, ICharacter
    {
        [SerializeField] protected Transform muzzlePoint;

        public bool IsGameRunning { get; set; }

        private IWeapon _weapon;

        protected float Speed { get; set; }

        private Vector2 _initialPosition;
        private Action<ICharacter> _onDeathCallback;

        public void Initialize(Transform spawnTransform, IWeapon weapon, Action<ICharacter> gameManagerCallback)
        {
            SetWeapon(weapon);
            _onDeathCallback = gameManagerCallback;
            _initialPosition = spawnTransform.position;
        }

        public void Fire()
        {
            if (!IsGameRunning) return;

            if (_weapon == null) throw new NullReferenceException("Weapon is not assigned. Use SetWeapon method");
            _weapon.Fire(transform.right, muzzlePoint, gameObject.layer);
        }

        public void AddForce(Vector2 force)
        {
            if (!IsGameRunning) return;
            Rb.AddForce(force);
        }

        public void Rotate(float angle) => Rb.MoveRotation(angle);
        public Vector2 GetMuzzlePosition() => muzzlePoint.position;
        public Transform GetTransform() => Rb.transform;
        public void SetWeapon(IWeapon weapon) => _weapon = weapon;
        public virtual void Deactivate() => gameObject.SetActive(false);
        public virtual void Activate() => gameObject.SetActive(true);
        public void ResetCharacter() => Rb.position = _initialPosition;

        public void OnDeath()
        {
            if (_onDeathCallback == null) throw new NullReferenceException("OnDeathCallback is null");
            _onDeathCallback.Invoke(this);
        }
    }
}
