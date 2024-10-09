using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInput : NetworkBehaviour
{
    private PlayerMovement m_playerMovement;

    [SerializeField] private int m_shootBufferTime;
    private bool m_canShoot;

    private Coroutine m_shootBufferCoroutine;


    private void Start()
    {
        m_playerMovement = GetComponent<PlayerMovement>();
        m_canShoot = false;
        m_shootBufferCoroutine = null;
    }

    IEnumerator ShootBuffer()
    {
        m_canShoot = true;
        yield return new WaitForSeconds(m_shootBufferTime);
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

                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                LayerMask layerMaskGround = 1 << 3;

                if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layerMaskGround))
                {
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.yellow);

                    m_playerMovement.SetDestination(hit.point);
                }
            }


            if (touch.phase == TouchPhase.Began && touch.phase == TouchPhase.Moved)
            {
                if (m_shootBufferCoroutine == null)
                {
                    m_shootBufferCoroutine = StartCoroutine(ShootBuffer());
                }


            }

            if (touch.phase == TouchPhase.Ended && touch.phase == TouchPhase.Moved)
            {
                //TODO Shoot de canon
                if (m_canShoot) Debug.Log("Tir de canon");

            }
        }

    }
}
