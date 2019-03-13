using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGrowth : MonoBehaviour
{
    
    WaveMovement movementComponent;
    bool startedGrowing = false;
    bool stoppedGrowing = false;

    [SerializeField] float growthRate = 0.2f; // Think about making this into a scriptable Object

    public bool StartedGrowing { get => startedGrowing; set => startedGrowing = value; }
    public bool StoppedGrowing { get => stoppedGrowing; }

    private void Awake()
    {
        movementComponent = GetComponent<Wave>().MovementComponent;
    }

    // Increase the scale along the z and y axes while it's going up
    public void StartGrowing()
    {
        startedGrowing = true;

        StopCoroutine(GrowWave());
        StartCoroutine(GrowWave());
    }

    public void StopGrowing()
    {
        stoppedGrowing = true;

        StopCoroutine(GrowWave());
    }

    IEnumerator GrowWave()
    {
        // While the wave is not going down, increase it's size slowly
        while (!movementComponent.StartedBringingDown && movementComponent.ReachedMaxHeight)
        {
            gameObject.transform.localScale += new Vector3(1f, 1f, 1f) * growthRate * Time.deltaTime;
            yield return null;
        }
    }

}
