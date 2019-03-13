using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarControl : MonoBehaviour
{
    public Transform tony;
    public Transform shark;
    
    Vector2 input;
    CharacterController cc;
    public LayerMask ground;
    public LayerMask wave;
    RaycastHit groundHit;
    RaycastHit waveHit;
    RaycastHit previousHit;
    public float snapDistance = 2.0f;
    public bool debug;

    //-----For Movement
    Vector3 maxVelocity;
    Vector3 minVelocity;
    Vector3 velocity = Vector3.zero;
    Vector3 acceleration = Vector3.zero;
    public Vector3 moveSpeed = new Vector3(10.0f, 10.0f, 10.0f);
    public float groundFriction = 1.0f;



    //-----For Balance
    public RawImage balanceNeedle;
    float balance = 0.0f;
    float turnSpeed;
    public float balanceMaxAngle = 90.0f;
    float balanceEasyAngle;
    float balanceMedAngle;
    float balanceHardAngle;
    private float balanceEasyAcc = 1.01f;
    private float balanceMedAcc = 1.015f;
    private float balanceHardAcc = 1.02f;
    Riding riding = Riding.NONE;

    //-----For Rotation
    Quaternion targetRotation;
    float angle;

    enum Riding
    {
        LEFT,
        RIGHT,
        NONE
    }


    private void Start()
    {
        cc = GetComponent<CharacterController>();

        //-----For Balance
        balanceEasyAngle = balanceMaxAngle / 18f;
        balanceMedAngle = balanceMaxAngle / 2f;
        balanceHardAngle = balanceMaxAngle / 1.5f;

        //-----For Movement
        maxVelocity.y = moveSpeed.y * 5;
        maxVelocity.x = moveSpeed.x * 3;
        

        minVelocity.z = moveSpeed.z;
    }

    private void Update()
    {
        GetInput();
        Balance();
        Rotate();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void GetInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    void Move()
    {


        print(velocity.z);

        if (!OnGround())
        {
            //If we're not on the ground, be affected by gravity
            acceleration.y += -2 * Time.deltaTime;
            print(acceleration.y);
            MoveHorizontal(0.5f);
        }
        else
        {
            acceleration.y = Mathf.Lerp(acceleration.y, 0, 0.1f);
            OnWave();
            MoveHorizontal(1f);
            if (riding != Riding.NONE) velocity.z += moveSpeed.z * 0.5f;
            if (Mathf.Abs(velocity.z) > minVelocity.z)
            {
                velocity.z = Mathf.Lerp(velocity.z, minVelocity.z * Mathf.Sign(velocity.z), 0.125f);
            }
            else velocity.z = minVelocity.z;

            if (Mathf.Abs(velocity.x) > maxVelocity.x) Mathf.Lerp(velocity.x, maxVelocity.x * Mathf.Sign(velocity.x), 1f);
        }


        velocity += acceleration;
        transform.position += velocity * Time.deltaTime;
    }

    void MoveHorizontal(float multiplier)
    {
        //Side to Side using velocity
        if(Mathf.Abs(velocity.x + moveSpeed.x * turnSpeed * Mathf.Sign(balance) * multiplier) < maxVelocity.x) velocity.x += moveSpeed.x * turnSpeed * Mathf.Sign(balance) * multiplier;

        ////Side to Side using acceleration
        //if (Mathf.Abs(acceleration.x + moveSpeed.x * turnSpeed * Mathf.Sign(balance)) < maxVelocity.x)
        //{
        //    acceleration.x = moveSpeed.x * turnSpeed * Mathf.Sign(balance);
        //}
        //else acceleration.x = Mathf.Sign(acceleration.x) * maxVelocity.x;
    }

    bool OnWave()
    {
        if (groundHit.transform.tag == "Rideable")
        {
            riding = Riding.LEFT;
        }
        else if (riding != Riding.NONE)
        {
            riding = Riding.NONE;
            return false;
        }
        return false;

    }

    bool OnGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, snapDistance + 0.5f, ground))
        {
            if (Vector3.Distance(transform.position, groundHit.point) < snapDistance)
            {
                transform.position = Vector3.Lerp(transform.position, groundHit.point + Vector3.up * snapDistance, 0.5f);
            }
            //riding = Riding.NONE;
            //acceleration.y = Mathf.Lerp(acceleration.y, 0.0f, 0.001f);

            //friction
            //if(acceleration.x > 2 && acceleration.x < 2) acceleration.x -= Mathf.Sign(acceleration.x) * groundFriction;
            
            velocity.y = 0.0f;
            return true;
        }
        return false;
    }

    void Balance()
    {
        balance += input.x;

        if (Mathf.Abs(balance) > balanceEasyAngle) balance *= balanceEasyAcc;
        else if (Mathf.Abs(balance) > balanceMedAngle) balance *= balanceMedAcc;
        else if (Mathf.Abs(balance) > balanceHardAngle) balance *= balanceHardAcc;

        balance = Mathf.Clamp(balance, -balanceMaxAngle, balanceMaxAngle);
        
        turnSpeed = Mathf.Pow(balance / balanceMaxAngle, 2);

        SetTonyRotation();

        balanceNeedle.transform.rotation = Quaternion.Euler(0.0f, 0.0f , -balance);
    }

    void Rotate()
    {

        targetRotation = Quaternion.Euler(0, balance, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.0125f);
    }

    void SetTonyRotation()
    {
        tony.SetPositionAndRotation(this.transform.position + new Vector3(0.0f, 1.0f, 0.0f), this.transform.rotation);
        tony.RotateAround(shark.transform.position, shark.transform.up, 0.0f + -balance);
    }

}
