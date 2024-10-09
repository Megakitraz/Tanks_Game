using UnityEngine;
using Mirror;
using UnityEngine.AI;
using System.Collections;

public class PlayerMovement : NetworkBehaviour
{
    private NavMeshAgent m_agent;

    private void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
    }

    public void SetDestination(Vector3 destination)
    {
        m_agent.destination = destination;
    }
}
