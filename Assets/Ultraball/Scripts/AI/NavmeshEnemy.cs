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

    public void SetTargetLocation(Transform position) 
    {
        agent.SetDestination(position.position);
    }
}
