using UnityEngine;

public class AnimationOnTrigger : MonoBehaviour
{
    [SerializeField] private Animator myAnimationController;
    [SerializeField] private ParticleSystem[] particleEffects;
    [SerializeField] private GameObject objectToEnable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            myAnimationController.SetBool ("playSpin", true);

            foreach (ParticleSystem ps in particleEffects)
            {
                if (ps != null)
                    ps.Play();
            }

            Debug.Log("trigger entered");
        }
    }

    public void EnableObject()
    {
        if (objectToEnable != null)
            objectToEnable.SetActive(true);

        StopParticles();
    }

    private void StopParticles()
    {
        foreach (ParticleSystem ps in particleEffects)
        {
            if (ps != null) 
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
