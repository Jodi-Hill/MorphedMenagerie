using System;
using UnityEngine;

public class VFXTrail : MonoBehaviour
{
    public float moveSpeed, turnSpeed;
    public int damageCounter = 69;
    public int cardHealth = 2;
    public GameObject hitText;

    public ParticleSystem explosion;

    public bool animate;

    private float step;
    private Action callback;
    private Transform[] path;
    private int pathId = 0;
    private bool noDmg;

    public void ChangeSpeed(int value)
    {
        moveSpeed = value;
        turnSpeed = value * 4;
    }

    private void Update()
    {
        if (animate)
        {
            if (!noDmg && damageCounter <= 0)
            {
                animate = false;
                transform.position = Vector3.forward * -2000;
                callback.Invoke();
            }

            int effectiveDmg = damageCounter;

            // Rotate towards destination
            Vector3 targetDir = path[pathId].position - transform.position;
            step = Time.deltaTime * turnSpeed;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);

            // Move forward
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            
            if (Vector3.Distance(transform.position, path[pathId].position) < 0.15f)
            {
                if (pathId != 0)
                {
                    CardDetection targetCard = path[pathId].GetComponent<CardDetection>();
                    EnemyDetection enemyDetection = path[pathId].GetComponent<EnemyDetection>();
                    HeroView hero = path[pathId].GetComponent<HeroView>();
                    if (noDmg) // buff card
                    {
                        if (targetCard != null)
                        {
                            targetCard.card.healthValue += cardHealth;
                            targetCard.card.attackValue += damageCounter;
                            targetCard.linkedCard.CalculateAura(targetCard.card.attackValue);
                        }
                        if (enemyDetection != null)
                        {
                            enemyDetection.card.healthValue += cardHealth;
                            enemyDetection.card.attackValue += damageCounter;
                        }
                    }
                    else // damage card
                    {
                        int hp = 0;
                        GameObject hitEf = Instantiate(hitText, transform.position, Quaternion.identity);
                        hitEf.GetComponent<HitCount>().text.text = damageCounter + "";
                        hitEf.transform.position -= Vector3.forward;
                        if (targetCard != null)
                        {
                            hp = targetCard.card.healthValue;
                            targetCard.Damage(damageCounter);
                        }
                        if (enemyDetection != null)
                        {
                            hp = enemyDetection.card.healthValue;
                            enemyDetection.Damage(damageCounter);
                        }
                        if (hero != null)
                        {
                            hero.Damage(damageCounter);
                        }
                        damageCounter -= hp;
                    }
                }

                pathId++;
                explosion.Play();
                if (pathId >= path.Length)
                {
                    animate = false;
                    transform.position = Vector3.forward * -2000;
                    callback.Invoke();
                }
            }
        }
    }

    public void StartAnimation(Action _callback, Transform[] _path, bool _noDmg)
    {
        path = _path;
        animate = true;
        noDmg = _noDmg;
        callback = _callback;
        pathId = 0;
        damageCounter = 0;
        cardHealth = 0;

        // get starting values
        CardDetection targetCard = path[pathId].GetComponent<CardDetection>();
        EnemyDetection enemyDetection = path[pathId].GetComponent<EnemyDetection>();
        if (noDmg)
        {
            if (targetCard != null) cardHealth += targetCard.card.healthValue;
            if (enemyDetection != null) cardHealth += enemyDetection.card.healthValue;
        }
        if (targetCard != null) damageCounter += targetCard.card.attackValue;
        if (enemyDetection != null) damageCounter += enemyDetection.card.attackValue;

        if (damageCounter <= 0 && !noDmg)
        {
            animate = false;
            transform.position = Vector3.forward * -2000;
            callback.Invoke();
            return;
        }
        transform.position = path[pathId].position;
        transform.eulerAngles = Vector3.zero;
    }
}
