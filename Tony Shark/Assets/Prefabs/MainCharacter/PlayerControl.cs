using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    private Camera myCamera;
    private GameObject tony;
    private GameObject shark;
    Rigidbody myBody;

    public Slider balanceSlider;

    //-----Input
    private float balanceInput;

    //-----Movement
    private Vector3 movementVector;

    //horizontal
    float horizontalMovment;

    //vertical
    float verticalMovement;

    //foward
    float fowardMovement;


    private float balance = 0.0f;
    private float balanceMinorAcceleration = 1.0075f;
    private float balanceModerateAcceleration = 1.01f;
    private float balanceMajorAcceleration = 1.0125f;
    public float balanceMax = 90;
    private Transform sharkInitTransform;

    //-----Jump
    private float jumpInput;
    private float jumpPower;
    private float jumpMax = 15.0f;
    private float jumpIncrement = 0.1f;


    void Start()
    {
        //-----Initialise Object Reference
        myCamera = GetComponentInChildren<Camera>();
        tony = GameObject.Find("/MainCharacter/Player");
        shark = GameObject.Find("/MainCharacter/Shark");
        sharkInitTransform = shark.transform;
        myBody = GetComponent<Rigidbody>();

        balanceSlider.minValue = -balanceMax;
        balanceSlider.maxValue = balanceMax;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        ProcessMovement();

        balanceSlider.value = balance;
    }

    void GetInput()
    {
        balanceInput = Input.GetAxis("Horizontal");
        jumpInput = 0;
        if (Input.GetKey("space")) jumpInput = 1;
        if (Input.GetKeyUp("space")) jumpInput = -1;
        
    }

    void ProcessMovement()
    {
        balance += balanceInput;
        balance *= 1.01f;


        //-----Minor Acceleration
        if (balance > balanceMax / 4 || balance < balanceMax / 4)
        {
            balance *= balanceMinorAcceleration;
        }

        //-----Moderate Acceleration
        if (balance > balanceMax / 2 || balance < balanceMax / 2)
        {
            balance *= balanceModerateAcceleration;
        }

        //-----Major Acceleration
        if (balance > balanceMax || balance < -balanceMax)
        {
            balance = 0;
        }
        else if(balance > balanceMax - balanceMax/4 || balance < - balanceMax + balanceMax/4)
        {
            balance *= balanceMajorAcceleration;
        }


        //horizontal
        horizontalMovment = balance/2;

        //vertical

        if (Input.GetKey("space") && jumpPower < jumpMax)
        {
            jumpPower += jumpIncrement;
            
        }
        if (Input.GetKeyUp("space"))
        {
            myBody.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
            jumpPower = 0;
        }

        //foward
        fowardMovement = 10.0f;
        

        


        movementVector = Vector3.right * horizontalMovment;
        movementVector += Vector3.forward * fowardMovement;
        movementVector += Vector3.up * verticalMovement;

        

        //shark
        shark.transform.rotation = sharkInitTransform.rotation;
        //shark.transform.Rotate(this.transform.right, -balance/200);

        //tony
        tony.transform.SetPositionAndRotation(this.transform.position + new Vector3(0.0f, 1.0f, 0.0f), this.transform.rotation);
        tony.transform.RotateAround(shark.transform.position, shark.transform.up, 0.0f + -balance);
    }

    void FixedUpdate()
    {
        myBody.MovePosition(myBody.position + movementVector * Time.deltaTime);
}
}


