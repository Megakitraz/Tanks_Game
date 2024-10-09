using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Missile : NetworkBehaviour
{
    [SerializeField] private float m_speed;

    [SerializeField] private float m_lifeTime;

    public PlayerAttack PlayerOwner;

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

        Destroy(gameObject);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == PlayerOwner) return;

        if (collision.transform.TryGetComponent<PlayerStats>(out PlayerStats playerStats))
        {
            playerStats.Health -= 1;
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        PlayerOwner.m_missileGameObject = null;
    }
}
