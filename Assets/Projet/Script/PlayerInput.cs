using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInput : NetworkBehaviour
{
    private PlayerMovement m_playerMovement;
    private PlayerAttack m_playerAttack;
    private bool m_canShoot;


    private void Start()
    {
        m_playerMovement = GetComponent<PlayerMovement>();
        m_playerAttack = GetComponent<PlayerAttack>();
        m_canShoot = false;
    }


    private void Update()
    {
        if (!isLocalPlayer) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Stationary)
            {
                m_canShoot = false;

                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                LayerMask layerMaskGround = 1 << 3;

                if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layerMaskGround))
                {
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);

                    m_playerMovement.SetDestination(hit.point);
                }
            }


            if (touch.phase == TouchPhase.Moved)
            {
                m_canShoot = true;


            }

            if (touch.phase == TouchPhase.Ended)
            {
                if (m_canShoot)
                {
                    m_playerAttack.ShootMissile();
                }

            }
        }

    }
}
