using System;
using Game.Scripts.Managers;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Game.Scripts.Input
{
    public interface IUserInput
    {
        public ReactiveProperty<Vector2> MoveDirection { get; }
        public ReactiveProperty<Vector2> MousePosition { get; }
        public Subject<Unit> Shoot { get; }
    }

    public sealed class PCUserInput : MonoBehaviour, IUserInput
    {
        public ReactiveProperty<Vector2> MoveDirection { get; } = new();
        public ReactiveProperty<Vector2> MousePosition { get; } = new();
        public Subject<Unit> Shoot { get; } = new();

        private ICameraManager _cameraManager;

        private PCInputActions _gameInputActions;
        private Vector2 _movementInput;
        private Camera _cam;

        [Inject]
        private void Construct(ICameraManager cameraManager) => _cameraManager = cameraManager;

        private void Awake()
        {
            Screen.SetResolution(1280, 720, false);
            InputSystem.EnableDevice(Mouse.current);

            _gameInputActions = new PCInputActions();
            _gameInputActions.Enable();

            _gameInputActions.Player.Move.performed += OnMovePerformed;
            _gameInputActions.Player.Move.canceled += OnMoveCanceled;
            _gameInputActions.Player.MousePosition.performed += OnMousePositionPerformed;
            _gameInputActions.Player.Click.performed += OnShootPerformed;
        }

        private void Start()
        {
            _cam = _cameraManager.GetMainCamera();
            if (_cam == null) throw new NullReferenceException("Camera is null");
        }

        private void OnMousePositionPerformed(InputAction.CallbackContext context)
        {
            var screenPos = context.ReadValue<Vector2>();
            MousePosition.Value = _cam.ScreenToWorldPoint(screenPos);
        }


        private void OnMovePerformed(InputAction.CallbackContext context) =>
            MoveDirection.Value = context.ReadValue<Vector2>();


        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            if (MoveDirection.CurrentValue != Vector2.zero) MoveDirection.Value = Vector2.zero;
        }

        private void OnShootPerformed(InputAction.CallbackContext context) => Shoot.OnNext(Unit.Default);

        private void OnDestroy()
        {
            _gameInputActions.Player.Move.performed -= OnMovePerformed;
            _gameInputActions.Player.Move.canceled -= OnMoveCanceled;
            _gameInputActions.Player.MousePosition.performed -= OnMousePositionPerformed;
            _gameInputActions.Player.Click.performed -= OnShootPerformed;
            _gameInputActions.Disable();
        }
    }
}
