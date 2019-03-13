using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveAnimator : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        
    }


    public void PlayTravelAnimation()
    {
        animator.SetBool("reachedMaxHeight", true);
    }

    public void PlayCrushAnimation()
    {
        animator.SetBool("startedBringingDown", true);
    }
}
