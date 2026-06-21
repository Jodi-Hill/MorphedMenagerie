using System;
using UnityEngine;

public class VFXTrail : MonoBehaviour
{
    public float moveSpeed, turnSpeed;
    public int damageCounter = 69;
    public int cardHealth = 2;
    public GameObject hitText;

    public ParticleSystem explosion;

    public Transform start, card, fighter;

    public bool animate;
    private int index;
    private Transform target;

    private float step;
    private Action callback;
    private int showable;

    private void Update()
    {
        if (animate)
        {
            int effectiveDmg = damageCounter;
            switch (index)
            {
                case 0:
                    target = start;
                    transform.position = target.position;
                    transform.eulerAngles = Vector3.zero;
                    showable = 0;
                    break;
                case 1:
                    target = card;
                    showable = Mathf.Clamp(effectiveDmg, 0, cardHealth);
                    break;
                case 2:
                    target = fighter;
                    showable = Mathf.Clamp(effectiveDmg - cardHealth, 0, int.MaxValue);
                    break;
            }

            // Rotate towards destination
            Vector3 targetDir = target.position - transform.position;
            step = Time.deltaTime * turnSpeed;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);

            // Move forward
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            
            if (Vector3.Distance(transform.position, target.position) < 0.15f)
            {
                if (index != 0)
                {
                    GameObject hitEf = Instantiate(hitText, transform.position, Quaternion.identity);
                    hitEf.GetComponent<HitCount>().text.text = showable + "";
                    hitEf.transform.position -= Vector3.forward;
                }

                index++;
                explosion.Play();
                if (index > 2)
                {
                    animate = false;
                    index = 0;
                    transform.position = Vector3.forward * -2000;
                    callback.Invoke();
                }
            }
        }
    }

    public void StartAnimation(Action _callback)
    {
        animate = true;
        callback = _callback;
    }
}
