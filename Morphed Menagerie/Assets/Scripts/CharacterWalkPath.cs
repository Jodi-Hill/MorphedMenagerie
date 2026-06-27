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
    public CutsceneEnable cutsceneObject;

    float time = 0f;

    public float distance;

    private void Start()
    {
        path.index = 0;
        agent.destination = path.GetCurrentWayPoint();
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, path.GetCurrentWayPoint());

        if (distance <= 0.5f)
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
        cutsceneObject.SwitchCams();
        gameObject.SetActive(false);
    }
}
