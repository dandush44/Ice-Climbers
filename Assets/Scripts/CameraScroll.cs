using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    [Header("Parameters")]
    
    public float platformSpeed;

    [Space(20)]
    
    public Transform player;

    private Vector3 platformSpacing;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = player.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(platformSpacing * platformSpeed * Time.deltaTime);
    }

    public void SetPlatformSpacing(Vector2 spacing)
    {
        platformSpacing = new Vector3(spacing.x, spacing.y, 0);
    }

    public void MoveToPos(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SpeedUp(float f)
    {
        platformSpeed += platformSpeed * f;
    }

    public void StopMoving()
    {
        platformSpeed = 0;
    }
}
