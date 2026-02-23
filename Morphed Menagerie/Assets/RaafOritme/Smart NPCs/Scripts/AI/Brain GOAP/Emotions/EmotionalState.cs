using System;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class EmotionalState
    {
        public float Fear { get; private set; }
        public float Anger { get; private set; }
        public float Happiness { get; private set; }
        public float Anxiety { get; private set; }

        /// <summary>
        /// Update over time or at once.
        /// </summary>
        /// <param name="_amount"></param>
        private void UpdateFear(float _amount)
        {
            Fear = Mathf.Clamp(Fear + _amount, 0, 100);
        }

        /// <summary>
        /// Update over time or at once.
        /// </summary>
        /// <param name="_amount"></param>
        private void UpdateAnger(float _amount)
        {
            Anger = Mathf.Clamp(Anger + _amount, 0, 100);
        }

        /// <summary>
        /// Update over time or at once.
        /// </summary>
        /// <param name="_amount"></param>
        private void UpdateHappiness(float _amount)
        {
            Happiness = Mathf.Clamp(Happiness + _amount, 0, 100);
        }

        /// <summary>
        /// Update over time or at once.
        /// </summary>
        /// <param name="_amount"></param>
        private void UpdateAnxiety(float _amount)
        {
            Anxiety = Mathf.Clamp(Anxiety + _amount, 0, 100);
        }

        /// <summary>
        /// Update emotions in general.
        /// </summary>
        public void Update()
        {
            // fear
            if (Fear > 0)
            {
                Fear -= Time.deltaTime * 0.25f;
            }

            // anger
            if (Anger > 0)
            {
                Anger -= Time.deltaTime * 0.5f;
            }

            // happiness
            if (Anger > 50 && Happiness > 0)
            {
                Happiness -= Time.deltaTime * 0.5f;
            }
            else if (Fear < 25 && Anxiety < 25 && Happiness < 100)
            {
                Happiness += Time.deltaTime * 0.25f;
            }
            else if (Happiness > 50)
            {
                Happiness -= Time.deltaTime * 0.25f;
            }

            // anxiety
            if (Fear > 50 && Anxiety < 100)
            {
                Anxiety += Time.deltaTime * 0.25f;
            }
            else if (Anxiety > 0)
            {
                Anxiety -= Time.deltaTime * 0.25f;
            }
        }

        /// <summary>
        /// Update specific emotion with corresponding value.
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_value"></param>
        public void UpdateType(EmotionType _type, float _value)
        {
            switch (_type)
            {
                case EmotionType.Fear:
                    UpdateFear(_value);
                    break;
                case EmotionType.Anger:
                    UpdateAnger(_value);
                    break;
                case EmotionType.Happiness:
                    UpdateHappiness(_value);
                    break;
                case EmotionType.Anxiety:
                    UpdateAnxiety(_value);
                    break;
            }
        }

        /// <summary>
        /// Update specific emotion with corresponding value.
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_value"></param>
        public void UpdateType(string _type, float _value)
        {
            Enum.TryParse(_type, out EmotionType emotionType);
            UpdateType(emotionType, _value);
        }

        /// <summary>
        /// Update specific emotion with corresponding value.
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_value"></param>
        public void SetType(EmotionType _type, float _value)
        {
            switch (_type)
            {
                case EmotionType.Fear:
                    Fear = _value;
                    break;
                case EmotionType.Anger:
                    Anger = _value;
                    break;
                case EmotionType.Happiness:
                    Happiness = _value;
                    break;
                case EmotionType.Anxiety:
                    Anxiety = _value;
                    break;
            }
        }

        /// <summary>
        /// Update specific emotion with corresponding value.
        /// </summary>
        /// <param name="_type"></param>
        /// <param name="_value"></param>
        public void SetType(string _type, float _value)
        {
            Enum.TryParse(_type, out EmotionType emotionType);
            SetType(emotionType, _value);
        }

        /// <summary>
        /// Returns leading emotion type.
        /// </summary>
        /// <returns></returns>
        public EmotionType GetLeadingEmotion()
        {
            if (Fear > Anger && Fear > Happiness && Fear > Anxiety)
            {
                return EmotionType.Fear;
            }
            if (Anger > Fear && Anger > Happiness && Anger > Anxiety)
            {
                return EmotionType.Anger;
            }
            if (Happiness > Anger && Happiness > Fear && Happiness > Anxiety)
            {
                return EmotionType.Happiness;
            }
            return EmotionType.Anxiety;
        }

        /// <summary>
        /// Return value of a specific emotion.
        /// </summary>
        /// <param name="_emotion"></param>
        /// <returns></returns>
        public float GetEmotionValue(EmotionType _emotion)
        {
            switch (_emotion)
            {
                case EmotionType.Fear:
                    return Fear;
                case EmotionType.Anger:
                    return Anger;
                case EmotionType.Happiness:
                    return Happiness;
            }
            return Anxiety;
        }
    }
}
