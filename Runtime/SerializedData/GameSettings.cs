using System;

namespace OneM.GameDataSystem
{
    [Serializable]
    public sealed class GameSettings
    {
        public string LanguageCode;
        public AudioData Audio = new();
        //TODO other settings
    }
}