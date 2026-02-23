namespace RaafOritme.SmartNPCs
{
    public class Personality
    {
        public PersonalityType Type { get; private set; }

        public Personality(PersonalityType _type)
        {
            Type = _type;
        }

        /// <summary>
        /// Gain a bias based on the personality of the NPC and  the emotion.
        /// </summary>
        /// <param name="_emotionType"></param>
        /// <param name="_emotionValue"></param>
        /// <returns></returns>
        public float ModifyBasedOnPersonality(EmotionType _emotionType, float _emotionValue)
        {
            switch (Type)
            {
                case PersonalityType.Aggressive:
                    if (_emotionType == EmotionType.Fear) return _emotionValue * 0.5f; // aggressive npc's have less fear
                    if (_emotionType == EmotionType.Anger) return _emotionValue * 2f; // aggressive npc's gain more anger
                    if (_emotionType == EmotionType.Happiness) return _emotionValue * 0.75f; // aggressive npc's gain slightly less happiness
                    // aggressive npc's have no anxiety modifier
                    break;
                case PersonalityType.Cautious:
                    if (_emotionType == EmotionType.Fear) return _emotionValue * 1.5f; // cautious npc's have more fear
                    if (_emotionType == EmotionType.Anger) return _emotionValue * 0.75f; // cautious npc's gain slightly less anger
                    if (_emotionType == EmotionType.Happiness) return _emotionValue * 1.25f; // cautious npc's gain slightly more happiness
                    if (_emotionType == EmotionType.Anxiety) return _emotionValue * 2f; // cautious npc's have more anxiety
                    break;
                case PersonalityType.Empathetic:
                    if (_emotionType == EmotionType.Fear) return _emotionValue * 0.5f; // empathetic npc's have less fear
                    if (_emotionType == EmotionType.Anger) return _emotionValue * 0.5f; // empathetic npc's gain less anger
                    if (_emotionType == EmotionType.Happiness) return _emotionValue * 2; // empathetic npc's gain more happiness
                    if (_emotionType == EmotionType.Anxiety) return _emotionValue * 0.5f; // empathetic npc's have less anxiety
                    break;
            }

            return _emotionValue;
        }
    }
}
