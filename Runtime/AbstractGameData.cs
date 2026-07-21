using System;
using UnityEngine;

namespace OneM.GameDataSystem
{
    /// <summary>
    /// Abstract base class to persist Game Data.
    /// </summary>
    public abstract class AbstractGameData : ScriptableObject
    {
        // All fields should be public and named in CamelCase (including inner classes)
        public int SlotIndex;
        public uint Saves;
        public ulong GameSecondsTime;
        public SerializedDateTime Created;
        public SerializedDateTime LastUpdate;
        public GameVersion Version = new();
        public GameSettings Settings = new();

        public event Action OnUpdated;

        public bool IsNewGame() => Saves <= 1; // Always saving once when create a new GameData
        public bool HasValidLanguage() => Settings.HasLanguageCode();

        public void UpdateData(int slot)
        {
            Saves++;
            SlotIndex = slot;
            LastUpdate = DateTime.Now;
            if (Created.IsEmpty()) Created = DateTime.Now;
            Version.Update();

            InvokeUpdate();
        }

        public void InvokeUpdate() => OnUpdated?.Invoke();

        public AbstractGameData Copy()
        {
            var className = GetType().Name;
            var copy = CreateInstance(className) as AbstractGameData;
            var json = JsonUtility.ToJson(this);

            JsonUtility.FromJsonOverwrite(json, copy);
            copy.Validate();

            return copy;
        }

        public virtual void ResetData()
        {
            var className = GetType().Name;
            var data = CreateInstance(className);
            var json = JsonUtility.ToJson(data);
            JsonUtility.FromJsonOverwrite(json, this);
        }

        public virtual void Validate() => Settings.Validate();

        public override string ToString() => GetDisplayName();
        public virtual string GetDisplayName() => $"Game Data {SlotIndex:D2}";

        public string GetDisplayGameTime()
        {
            var hours = GameSecondsTime / 3600;
            var minutes = (GameSecondsTime % 3600) / 60;
            var seconds = GameSecondsTime % 60;

            return $"{hours:D2}:{minutes:D2}:{seconds:D2}";
        }

        public static T CreateValidInstance<T>() where T : AbstractGameData
        {
            var instance = CreateInstance<T>();
            instance.Validate();
            return instance;
        }
    }
}