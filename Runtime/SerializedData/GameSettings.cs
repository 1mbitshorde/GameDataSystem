using System;

namespace OneM.GameDataSystem
{
    [Serializable]
    public sealed class GameSettings
    {
        public string LanguageCode;
        public AudioData Audio = new();
        //TODO other settings

        private const string defaultLanguageCode = "en";

        public bool HasLanguageCode() => !string.IsNullOrEmpty(LanguageCode);

        public void Validate()
        {
            if (!HasLanguageCode()) LanguageCode = defaultLanguageCode;
        }

        public string GetValidLanguageCode() => HasLanguageCode() ? LanguageCode : defaultLanguageCode;
    }
}