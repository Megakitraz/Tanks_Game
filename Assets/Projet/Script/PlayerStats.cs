using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Diagnostics;

public class PlayerStats : NetworkBehaviour
{
    [SerializeField] private int m_maxHealth;
    [SerializeField] private int m_invincibleTime;

    [SyncVar(hook = nameof(OnHealthChanged))]
    private int m_health;

    [SerializeField] private GameObject m_body;

    private Coroutine m_hitInvincibility;

    public int Health
    {
        get { return m_health; }
        set
        {
            if (m_hitInvincibility == null && isServer) // Ensure only server modifies health
            {
                m_health = Mathf.Min(value, m_maxHealth);

                if (m_health <= 0)
                {
                    Dying();
                    UnityEngine.Debug.Log($"{name} Died");
                }
            }
        }
    }

    private void Start()
    {
        if (isServer)
        {
            Health = m_maxHealth; // Only the server sets the initial health value
        }
        m_hitInvincibility = null;
    }

    private void Dying()
    {
        Destroy(gameObject);
    }

    // Hook called when health changes, useful to trigger effects on clients
    private void OnHealthChanged(int oldHealth, int newHealth)
    {
        if (newHealth < oldHealth && m_hitInvincibility == null && newHealth > 0)
        {
            // Start hit invincibility coroutine on clients
            StartCoroutine(HitInvincibility());
        }

        UnityEngine.Debug.Log($"{name} health: {newHealth}");
    }

    // This coroutine will be executed on the clients to show the invincibility effect
    IEnumerator HitInvincibility()
    {
        float divideTime = m_invincibleTime / 6f;

        for (int i = 0; i < 3; i++)
        {
            m_body.SetActive(false);
            yield return new WaitForSeconds(divideTime);

            m_body.SetActive(true);
            yield return new WaitForSeconds(divideTime);
        }

        m_hitInvincibility = null;
    }

    // Server method to apply damage and reduce health
    [Server]
    public void ApplyDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Dying();
        }
    }
}
