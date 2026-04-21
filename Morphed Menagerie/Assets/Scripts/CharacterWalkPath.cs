using System.IO;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class CharacterWalkPath : MonoBehaviour
{
    [SerializeField] float waitTimeOnwayPoint = 1f;
    [SerializeField] Path path;

    public NavMeshAgent agent;
    public Animator animator;

    float time = 0f;

    private void Start()
    {
        agent.destination = path.GetCurrentWayPoint();
    }

    private void Update()
    {
        if (agent.remainingDistance <= 0.1f)
        {
            time += Time.deltaTime;

            if (time >= waitTimeOnwayPoint)
            {
                time = 0f;

                if (path.pathType == Path.PathType.StopAtEnd && path.IsLastWayPoint())
                {
                    Disappear();
                    return;
                }

                agent.destination = path.GetNextWaypoint();
            }
        }

        float normalizedSpeed = Mathf.InverseLerp (0f, agent.speed, agent.velocity.magnitude);
        animator.SetFloat("speed", normalizedSpeed);
    }

    void Disappear()
    {
        gameObject.SetActive(false);
    }
}
