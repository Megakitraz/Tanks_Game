using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Missile : NetworkBehaviour
{
    [SerializeField] private float m_speed;

    [SerializeField] private float m_lifeTime;

    public PlayerAttack PlayerOwner;
    public NetworkConnectionToClient ConnectionToClient;

    private void Start()
    {
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter: {gameObject.name}-{other.name}");

        if (other.GetComponent<NetworkBehaviour>().connectionToClient == ConnectionToClient) return;

        if (other.transform.TryGetComponent<PlayerStats>(out PlayerStats playerStats))
        {
            playerStats.Health -= 1;
            BeforeDestroy();
        }
    }

    private void BeforeDestroy()
    {
        PlayerOwner.m_missileGameObject = null;
        NetworkServer.Destroy(gameObject);
    }
}
