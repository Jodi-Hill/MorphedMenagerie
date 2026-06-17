using UnityEngine;

public class AnimationOnTrigger : MonoBehaviour
{
    [SerializeField] private Animator myAnimationController;
    [SerializeField] private ParticleSystem[] particleEffects;
    [SerializeField] private GameObject objectToEnable;
    [SerializeField] private GameObject objectToDisable;
    [SerializeField] private GameObject text;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
        }
    }

    public void Triggered()
    {
        myAnimationController.SetBool("playSpin", true);

        foreach (ParticleSystem ps in particleEffects)
        {
            if (ps != null)
                ps.Play();
        }

        Debug.Log("trigger entered");
    }

    public void EnableObject()
    {
        if (text != null)
        {
            Debug.Log("text shown");
            text.SetActive(true);
        }

        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }

        if (objectToDisable != null)
        {
            objectToDisable.SetActive(false);
        }

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
