using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public GameObject target;

    GameObject[] targets;
    NavMeshAgent agent;
    List<float> distances = new List<float>();

    bool players = true; 

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        #region Player Detection call

        // Run player Detection
        if (target == null && players)
        {
            FindTarget();
        }
        else if (!players)
        {
            return;
        }
        #endregion

        if (Vector3.Distance(transform.position, target.transform.position) > agent.stoppingDistance)
        {
            agent.destination = target.transform.position;
        }
    }

    private void FindTarget()
    {
      
        targets = GameObject.FindGameObjectsWithTag("Player");

        // Disable Enemy if no Players are found;
        if (targets == null)
        {
            players = false;
            return;
        }

        // Just ask me what this does if you want to know -Gloops
        foreach (GameObject t in targets)
        {
            float d = Vector3.Distance(transform.position, t.transform.position);
            distances.Add(d);

            if (d == distances.Min())
            {
                target = t;
            }
        }
    }
}
