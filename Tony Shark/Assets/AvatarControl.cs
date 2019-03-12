using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarControl : MonoBehaviour
{
    
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
    float balance = 0.0f;
    float turnSpeed;
    public float balanceMaxAngle = 90.0f;
    public Slider balanceSlider;
    float balanceEasyAngle;
    float balanceMedAngle;
    float balanceHardAngle;
    private float balanceEasyAcc = 1.0075f;
    private float balanceMedAcc = 1.01f;
    private float balanceHardAcc = 1.0125f;
    Riding riding = Riding.NONE;

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
        balanceSlider.minValue = -balanceMaxAngle;
        balanceSlider.maxValue = balanceMaxAngle;
        balanceEasyAngle = balanceMaxAngle / 3f;
        balanceMedAngle = balanceMaxAngle / 1.5f;
        balanceHardAngle = balanceMaxAngle / 1.125f;

        //-----For Movement
        maxVelocity.y = moveSpeed.y * 5;
        maxVelocity.x = moveSpeed.x * 3;
        

        minVelocity.z = moveSpeed.z;
    }

    private void Update()
    {
        GetInput();
        DrawLines();
        Balance();
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
            acceleration.y += Physics.gravity.y / 2 * Time.deltaTime;
            MoveHorizontal(0.5f);
        }
        else
        {
            OnWave();
            MoveHorizontal(1f);
            if (riding != Riding.NONE) velocity.z += moveSpeed.z * 0.1f;
            if (Mathf.Abs(velocity.z) > minVelocity.z) Mathf.Lerp(velocity.z, minVelocity.z * Mathf.Sign(velocity.z), 0.125f);
            else velocity.z = minVelocity.z;

            if (Mathf.Abs(velocity.x) > maxVelocity.x) Mathf.Lerp(velocity.x, maxVelocity.x * Mathf.Sign(velocity.x), 1f);
        }


        //transform.position += Vector3.right * moveSpeed.x * Time.deltaTime * turnSpeed * Mathf.Sign(balance);
        //transform.position += Vector3.forward * moveSpeed.y * Time.deltaTime * input.y;


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

        //Foward
        //velocity.z = moveSpeed.z * input.y;
    }

    bool OnWave()
    {
        if (groundHit.transform.tag == "Rideable")
        {
            riding = Riding.LEFT;
        }
        else if (riding != Riding.NONE)
        {
            //if (riding == Riding.LEFT)
            //{
            //    velocity.x -= 10.0f;
            //}
            //else if (riding == Riding.RIGHT)
            //{
            //    velocity.x += 10.0f;
            //}
            //velocity.y += 25f;
            riding = Riding.NONE;

            return false;
        }
        return false;

    }

    bool OnGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, snapDistance, ground))
        {
            if (Vector3.Distance(transform.position, groundHit.point) < snapDistance)
            {
                transform.position = Vector3.Lerp(transform.position, groundHit.point + Vector3.up * snapDistance, 0.5f);
            }
            //riding = Riding.NONE;
            acceleration.y = 0.0f;

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

        if (Mathf.Abs(balance) > Mathf.Abs(balanceEasyAngle)) balance *= balanceEasyAcc;
        else if (Mathf.Abs(balance) > Mathf.Abs(balanceMedAngle)) balance *= balanceMedAcc;
        else if (Mathf.Abs(balance) > Mathf.Abs(balanceHardAngle)) balance *= balanceHardAcc;

        Mathf.Clamp(balance, -balanceMaxAngle, balanceMaxAngle);
        
        turnSpeed = Mathf.Pow(balance / balanceMaxAngle, 2);

        balanceSlider.value = balance;
    }

    void DrawLines()
    {
        if (!debug) return;

        Debug.DrawLine(transform.position, transform.position + Vector3.down * snapDistance, Color.red);
        Debug.DrawLine(transform.position, transform.position + (Vector3.down / 2) + Vector3.right * snapDistance, Color.blue);
        Debug.DrawLine(transform.position, transform.position + (Vector3.down / 2) - Vector3.right * snapDistance, Color.green);
    }
}
