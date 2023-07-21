using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// base for all navmesh enemies
/// </summary>
public class NavmeshEnemy : MonoBehaviour
{
    NavMeshAgent agent;

    public NavMeshAgent Agent { get => agent; set => agent = value; }

    public void SetTargetLocation(Transform position) 
    {
        Agent.SetDestination(position.position);
    }


}
