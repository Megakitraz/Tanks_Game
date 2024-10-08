using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerStats : NetworkBehaviour
{
    [SerializeField] private int m_maxHealth;
    [SerializeField] private int m_invicibleTime;

    private int m_health;

    private Collider m_collider;
    [SerializeField] private GameObject m_body;

    private Coroutine m_hitInvicibility;

    public int Health
    {
        get
        {
            return m_health;
        }
        set
        {
            if (m_hitInvicibility == null)
            {
                m_hitInvicibility = StartCoroutine(HitInvicibility());
                m_health = Mathf.Min(value, m_maxHealth);
                if (m_health <= 0)
                {
                    Dying();
                    Debug.Log($"{name} Died");
                }
                else
                {

                    Debug.Log($"{name} health: {m_health}");
                }
            }
            
        }
    }

    private void Start()
    {
        Health = m_maxHealth;
        m_hitInvicibility = null;
        m_collider = GetComponent<Collider>();
    }

    private void Dying()
    {
        Destroy(gameObject);
    }



    IEnumerator HitInvicibility()
    {
        float divideTime = m_invicibleTime / 6f;
        m_collider.enabled = false;

        for (int i = 0; i < 3; i++) 
        {
            m_body.SetActive(false);

            yield return new WaitForSeconds(divideTime);

            m_body.SetActive(true);

            yield return new WaitForSeconds(divideTime);
        }

        m_collider.enabled = true;

        m_hitInvicibility = null;
    }



}
