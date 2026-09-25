using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Camera cam;
    public Transform player;
    public float smoothSpeed;
    public Vector3 offset;
    public Vector3 androidOffset;
    public float size;
    public float androidSize;

    private Vector3 ofs;

    void Awake()
    {
#if UNITY_ANDROID
        cam.orthographicSize = androidSize;
        ofs = androidOffset;
#else
        cam.orthographicSize = size;
        ofs = offset;
#endif

        Vector3 desiredPos = new Vector3(player.position.x + ofs.x, player.position.y + ofs.y, player.position.z + ofs.z);
        transform.position = desiredPos;
    }
    
    void FixedUpdate()
    {
        if (player == null)
            return;

        float yPos;

        /*if ((player.position.y + ofs.y) < -2)
            yPos = 0;
        else*/
            yPos = player.position.y + ofs.y;

        Vector3 desiredPos = new Vector3(player.position.x + ofs.x, yPos, player.position.z + ofs.z);
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPos;
    }

    public void StopFollowing()
    {
        player = null;
    }
}
