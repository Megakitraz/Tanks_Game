using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerStats : NetworkBehaviour
{
    [SerializeField] private int m_maxHealth;

    private int m_health;

    public int Health
    {
        get
        {
            return m_health;
        }
        set
        {
            m_health = Mathf.Min(value, m_maxHealth);
            if(m_health <= 0)
            {
                Dying();
            }
        }
    }

    private void Start()
    {
        Health = m_maxHealth;
    }

    private void Dying()
    {
        Destroy(gameObject);
    }

    

}
