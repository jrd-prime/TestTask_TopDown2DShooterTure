using System;
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Input;
using Game.Scripts.PhysicsObjs.Character;
using Game.Scripts.PhysicsObjs.Character.Player;
using JetBrains.Annotations;
using R3;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Managers
{
    public interface IUnitsManager
    {
        public void SetGameRunning(bool b);
        public void ActivateUnits();
        public void ResetUnits();
        public void SetUnits(List<ICharacter> units);
        public void DeactivateUnits();
    }

    [UsedImplicitly]
    public sealed class UnitsManager : IUnitsManager, IInitializable, IDisposable
    {
        private IUserInput _userInput;

        private bool isInitialized => _units.Count > 0;
        private List<ICharacter> _units = new();
        private readonly CompositeDisposable _disposable = new();

        [Inject]
        private void Construct(IUserInput userInput) => _userInput = userInput;

        public void Initialize()
        {
            if (_userInput == null) throw new Exception("IUserInput is null");
            _userInput.Shoot.Subscribe(OnUserFireBtnClicked).AddTo(_disposable);
        }

        public void SetGameRunning(bool b)
        {
            CheckUnitsList();
            foreach (var unit in _units) unit.IsGameRunning = b;
        }

        public void ActivateUnits()
        {
            CheckUnitsList();
            foreach (var unit in _units) unit.Activate();
        }

        public void ResetUnits()
        {
            CheckUnitsList();
            foreach (var unit in _units) unit.ResetCharacter();
        }

        public void SetUnits(List<ICharacter> units) => _units = units;

        public void DeactivateUnits()
        {
            CheckUnitsList();
            foreach (var unit in _units) unit.Deactivate();
        }

        private void OnUserFireBtnClicked(Unit _) => _units.First(character => character is PlayerCharacter).Fire();

        private void CheckUnitsList()
        {
            if (!isInitialized) throw new Exception("Units list is empty. Set units first.");
        }

        public void Dispose() => _disposable?.Dispose();
    }
}
