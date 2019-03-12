using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailAvatarControl : MonoBehaviour
{
    public Vector2 horizontalSpeeds = new Vector2 ( 50.0f, 50.0f );
    public float jumpPower = 20.0f;

    Vector2 input;
    Quaternion targetRotation;
    float angle;

    float balance = 0.0f;
    public float balanceMaxAngle = 90.0f;
    public Slider balanceSlider;
    float balanceEasyAngle;
    float balanceMedAngle;
    float balanceHardAngle;
    private float balanceEasyAcc = 1.0075f;
    private float balanceMedAcc = 1.01f;
    private float balanceHardAcc = 1.0125f;

    float turnSpeed;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        balanceSlider.minValue = -balanceMaxAngle;
        balanceSlider.maxValue =  balanceMaxAngle;
        balanceEasyAngle    = balanceMaxAngle / 3f;
        balanceMedAngle  = balanceMaxAngle / 1.5f;
        balanceHardAngle    = balanceMaxAngle / 1.125f;
    }

    void Update()
    {
        GetInput();
        CalculateDirection();
        Balance();
        Rotate();
        Move();
    }

    void GetInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    void CalculateDirection()
    {
        angle = Mathf.Atan2(input.x, 1);
        angle = Mathf.Rad2Deg * angle;
    }

    void Rotate()
    {
        targetRotation = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    void Balance()
    {
        balance += input.x;

        if (Mathf.Abs(balance) > Mathf.Abs(balanceEasyAngle)) balance *= balanceEasyAcc;
        else if (Mathf.Abs(balance) > Mathf.Abs(balanceMedAngle)) balance *= balanceMedAcc;
        else if (Mathf.Abs(balance) > Mathf.Abs(balanceHardAngle)) balance *= balanceHardAcc;

        Mathf.Clamp(balance, -balanceMaxAngle, balanceMaxAngle);
        turnSpeed = Mathf.Abs(balance)/500.0f;

        balanceSlider.value = balance;
    }

    void Move()
    {
        rb.AddForce(transform.right * horizontalSpeeds.y * Time.deltaTime * input.x, ForceMode.VelocityChange);
    }
}
