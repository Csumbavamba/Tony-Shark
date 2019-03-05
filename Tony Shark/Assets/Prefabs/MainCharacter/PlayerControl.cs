using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Camera myCamera;
    private GameObject player;
    private GameObject shark;

    //-----Input
    private float balanceInput;

    //-----Movement
    private Vector3 movementVector;
    private float balance = 0.0f;
    private float balanceAcceleration;


    // Start is called before the first frame update
    void Start()
    {
        //-----Initialise Object Reference
        myCamera = GetComponentInChildren<Camera>();
        player = GameObject.Find("/MainCharacter/Player");
        shark = GameObject.Find("/MainCharacter/Shark");
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        ProcessMovement();
    }

    void GetInput()
    {
        balanceInput = Input.GetAxis("Horizontal");
    }

    void ProcessMovement()
    {

        balance = balanceInput;
        
        
        player.transform.RotateAround(shark.transform.position, shark.transform.up, balance);
        print(player.transform.rotation);


    }
}
