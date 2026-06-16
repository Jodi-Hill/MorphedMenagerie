using UnityEngine;
using TMPro;
using System;

public class VFXTrail : MonoBehaviour
{
    public float moveSpeed, turnSpeed;
    public int damageCounter = 5;
    public int cardHealth = 2;
    public TextMeshPro text;

    public ParticleSystem explosion;

    public Transform start, card, fighter;

    public bool animate;
    private int index;
    private Transform target;

    private float step;
    private Action callback;

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
                    text.text = effectiveDmg + "";
                    break;
                case 1:
                    target = card;
                    text.text = effectiveDmg + "";
                    break;
                case 2:
                    target = fighter;
                    effectiveDmg -= cardHealth;
                    if (effectiveDmg < 0) effectiveDmg = 0;
                    text.text = effectiveDmg + "";
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
