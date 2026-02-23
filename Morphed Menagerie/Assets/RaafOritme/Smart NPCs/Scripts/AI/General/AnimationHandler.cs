using UnityEngine;

namespace RaafOritme.SmartNPCs
{
    public class AnimationHandler : MonoBehaviour
    {
        private Animator animator;

        private AgentController agentController;
        private AgentController AgentController => gameObject.GetAndCacheComponent(ref agentController);

        public string startWithAnimation;

        private void Start()
        {
            if (startWithAnimation != "")
            {
                SetTrigger(startWithAnimation);
            }
        }

        /// <summary>
        /// Set the movement speed.
        /// </summary>
        /// <param name="_speed"></param>
        public void SetSpeed(float _speed)
        {
            if (CheckAnimator()) animator.SetFloat("Speed", _speed);
        }

        /// <summary>
        /// Set a specific animation trigger.
        /// </summary>
        /// <param name="_trigger"></param>
        public void SetTrigger(string _trigger)
        {
            if (CheckAnimator()) animator.SetTrigger(_trigger);
        }

        /// <summary>
        /// Switch back to the default animation. Can be used to break from a cycled animation.
        /// </summary>
        public void BackToDefault()
        {
            if (startWithAnimation != "")
            {
                SetTrigger(startWithAnimation);
            }
            else
            {
                SetTrigger("Default");
            }
        }

        private bool CheckAnimator()
        {
            if (animator == null)
            {
                animator = AgentController.animator;
            }

            return (animator) ? true : false;
        }
    }
}
