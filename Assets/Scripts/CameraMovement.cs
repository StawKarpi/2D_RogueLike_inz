using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 p = transform.position;
        p.x = player.position.x;
        p.y = player.position.y;

        transform.position = p;
    }
}
