using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea : MonoBehaviour
{
    Vector3 positionFromplayer;
    GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<AvatarControl>().gameObject;
        CalculateDistanceFromPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        MoveAlongPlayer();
    }

    private void CalculateDistanceFromPlayer()
    {
        positionFromplayer = transform.position - player.transform.position;
    }

    private void MoveAlongPlayer()
    {
        // Keep a constant distance
        Vector3 movedPosition = player.transform.position + positionFromplayer;
        movedPosition.y = transform.position.y;

        transform.position = movedPosition;
    }

    
}
