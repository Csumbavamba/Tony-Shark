using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows to Lerp on different axes
public class WaveMovementComponent : MonoBehaviour
{
    [SerializeField] float travelSpeed = 3f;
    [SerializeField] float raiseSpeed = 1f;
    [SerializeField] float height = 1.5f;

    public WaveMovementComponent(float travelSpeed, float raiseSpeed, float height)
    {
        this.travelSpeed = travelSpeed;
        this.raiseSpeed = raiseSpeed;
        this.height = height;

        Start();
    }

    // Movement of the Wave
    float yWaveMovement = 180.0f; // It only goes down from it's position
    Vector3 travelPosition;

    private void Start()
    {
        travelPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    private void Update()
    {
        MoveWaveForward();
    }

    public void MoveWaveForward()
    {
        yWaveMovement += Time.deltaTime * raiseSpeed;

        // Keep moving it forward
        travelPosition -= transform.right * travelSpeed * Time.deltaTime;

        // Move up and down
        travelPosition += -Mathf.Cos(yWaveMovement) * height * transform.up;

        transform.position = travelPosition;
    }
}