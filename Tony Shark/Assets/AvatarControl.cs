using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarControl : MonoBehaviour
{
    public Vector2 horizontalSpeeds = new Vector2(50.0f, 50.0f);
    Vector2 input;
    Rigidbody rb;
    public LayerMask ground;
    RaycastHit hitInfo;
    public float snapDistance = 2.0f;
    public bool debug;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        GetInput();
        DrawLines();
    }

    private void FixedUpdate()
    {
        Hover();
        Move();
    }

    void GetInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
    }

    void Move()
    {
        rb.AddForce(transform.right * horizontalSpeeds.y * Time.deltaTime * input.x, ForceMode.VelocityChange);
    }

    void Hover()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, snapDistance, ground))
        {
            if (Vector3.Distance(transform.position, hitInfo.point) < snapDistance)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up * snapDistance, 0.1f);
            }
        }
    }

    void DrawLines()
    {
        if (!debug) return;

        Debug.DrawLine(rb.position, rb.position + Vector3.down * snapDistance, Color.red);
    }
}
