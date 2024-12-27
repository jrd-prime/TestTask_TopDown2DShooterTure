using System;
using System.Collections.Generic;
using Game.Scripts.Settings.Main;
using JetBrains.Annotations;
using VContainer;
using VContainer.Unity;

namespace Game.Scripts.Managers
{
    public interface ISettingsManager
    {
        public T GetSettings<T>() where T : InGameSettings;
    }

    [UsedImplicitly]
    public class SettingsManager : ISettingsManager, IInitializable, IDisposable
    {
        private Dictionary<Type, InGameSettings> _settingsCache = new();

        [Inject]
        private void Construct(MainSettings mainSettings)
        {
            _settingsCache = mainSettings.GetSettingsList();
        }

        public void Initialize()
        {
            if (_settingsCache.Count == 0) throw new Exception("No settings registered");
        }

        public T GetSettings<T>() where T : InGameSettings
        {
            if (!_settingsCache.ContainsKey(typeof(T)))
                throw new ArgumentException($"Settings of {nameof(T)} is not registered in {nameof(SettingsManager)}");

            return (T)_settingsCache[typeof(T)];
        }

        public void Dispose() => _settingsCache.Clear();
    }
}
