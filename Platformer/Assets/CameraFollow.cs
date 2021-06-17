using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public SpriteRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControls>().transform;
        rend = player.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPos = player.position + Vector3.back * 10 + Vector3.up * 2;
        if (rend.flipX)
        {
            targetPos += Vector3.left;
        }
        else
        {
            targetPos += Vector3.right;
        }
               
        Vector3 currentPos = transform.position;
        transform.position = Vector3.Lerp(currentPos, targetPos, 0.01f);
    }
}
