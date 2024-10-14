using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Missile : NetworkBehaviour
{
    [SerializeField] private float m_speed;

    [SerializeField] private float m_lifeTime;

    [SerializeField] private int m_maxBounceOnWall;

    private float m_cooldownOnBounce;
    private float m_timeOfLastBounce;

    private int m_bounceOnWall;

    [SyncVar] public PlayerAttack PlayerOwner;
    public NetworkConnectionToClient ConnectionToClient;

    private void Start()
    {
        m_cooldownOnBounce = 0.1f;
        m_timeOfLastBounce = -m_cooldownOnBounce;

        StartCoroutine(LifeTime());
    }

    private void Update()
    {
        transform.position = transform.position + transform.forward * m_speed * Time.deltaTime;
    }

    IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(m_lifeTime);

        BeforeDestroy();
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log($"OnCollisionEnter: {gameObject.name}-{other.transform.name}");

        if (other.transform.TryGetComponent<NetworkBehaviour>(out NetworkBehaviour networkBehaviour))
        {
            if (networkBehaviour.connectionToClient == ConnectionToClient) return;
        }

        if (other.transform.TryGetComponent<PlayerStats>(out PlayerStats playerStats))
        {
            playerStats.Health -= 1;
            BeforeDestroy();
        }

        if (other.transform.TryGetComponent<Wall>(out Wall wall))
        {

            if (m_bounceOnWall < m_maxBounceOnWall + 1)
            {
                if(m_timeOfLastBounce + m_cooldownOnBounce < Time.time) Bounce(other.contacts[0].normal);
            }
            else
            {
                BeforeDestroy();
            }
        }
    }

    private void Bounce(Vector3 normal)
    {

        m_bounceOnWall++;
        Vector3 vectorToReverse = -Vector3.Project(transform.forward, normal);
        Vector3 vectorToKeep = Vector3.ProjectOnPlane(transform.forward, normal);

        transform.forward = vectorToReverse + vectorToKeep;

        m_timeOfLastBounce = Time.time;

        Debug.Log($"Bounce: {m_bounceOnWall}");
    }

    private void BeforeDestroy()
    {
        // Null check for PlayerOwner to avoid the error
        if (PlayerOwner != null)
        {
            PlayerOwner.m_missileGameObject = null;
        }

        NetworkServer.Destroy(gameObject);
    }
}
