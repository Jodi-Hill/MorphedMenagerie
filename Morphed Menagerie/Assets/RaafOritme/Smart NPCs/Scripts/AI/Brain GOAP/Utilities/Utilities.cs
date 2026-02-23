using System;
using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    [Serializable]
    public class Utilities
    {
        public float hunger;
        public float fatigue;

        public float hungerModifier = 0.25f;
        public float fatigueModifier = 0.5f;

        /// <summary>
        /// Updates the utilities accordingly over time.
        /// </summary>
        public void UpdateNeeds()
        {
            // Assume 0 (not fatigued) to 100 (fully fatigued)
            if (fatigue < 100)
            {
                fatigue += Time.deltaTime * fatigueModifier;
            }

            if (hunger < 100)
            {
                hunger += Time.deltaTime * hungerModifier;
            }

            // Clamp values
            fatigue = Mathf.Clamp(fatigue, 0f, 100f);
            hunger = Mathf.Clamp(hunger, 0f, 100f);
        }
    }
}
