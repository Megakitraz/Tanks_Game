using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerAttack : NetworkBehaviour
{
    [SerializeField] private GameObject m_missilePrefab;

    [SerializeField] private Transform m_shootPosition;

    public GameObject m_missileGameObject;
    private Missile m_missile;

    private void Start()
    {
        m_missileGameObject = null;
    }

    public void ShootMissile()
    {
        
        if(m_missileGameObject == null)
        {
            m_missileGameObject = Instantiate(m_missilePrefab, m_shootPosition.position, m_shootPosition.rotation);
            m_missile = m_missileGameObject.GetComponent<Missile>();
            m_missile.PlayerOwner = this;
        }

    }
}
