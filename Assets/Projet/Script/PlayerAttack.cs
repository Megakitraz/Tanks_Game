using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerAttack : NetworkBehaviour
{
    [SerializeField] private GameObject m_missilePrefab;
    [SerializeField] private Transform m_shootPosition;

    [NonSerialized] public GameObject m_missileGameObject;
    private Missile m_missile;

    private void Start()
    {
        m_missileGameObject = null;
    }

    // This function is called on the client, but it sends a command to the server
    public void ShootMissile()
    {
        if (isLocalPlayer)  // Ensure only the local player can shoot
        {
            CmdShootMissile();
        }
    }

    // This command is called on the server when a client requests to shoot a missile
    [Command]
    private void CmdShootMissile()
    {
        if (m_missileGameObject == null)
        {
            // Instantiate the missile at the shoot position
            m_missileGameObject = m_shootPosition != null
                ? Instantiate(m_missilePrefab, m_shootPosition.position, m_shootPosition.rotation)
                : Instantiate(m_missilePrefab);

            // Ensure the missile has a NetworkIdentity component
            if (m_missileGameObject.GetComponent<NetworkIdentity>() == null)
            {
                Debug.LogError("Missile prefab is missing NetworkIdentity. Please add it.", m_missileGameObject);
                return;
            }

            // Give the missile a unique name using the player's NetworkIdentity netId
            m_missileGameObject.name = $"{m_missilePrefab.name} [connId={GetComponent<NetworkIdentity>().netId}]";

            // Get the Missile component and assign the player as the owner
            m_missile = m_missileGameObject.GetComponent<Missile>();
            m_missile.PlayerOwner = this;
            m_missile.ConnectionToClient = connectionToClient;

            // Spawn the missile on the server and assign the connection of the player as the owner
            NetworkServer.Spawn(m_missileGameObject, connectionToClient);
        }
    }
}
