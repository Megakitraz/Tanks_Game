using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    private void Update()
    {
        if (!isLocalPlayer) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 playerMovement = new Vector3(h,v,0f);

        transform.position = transform.position + playerMovement;

    }
}
