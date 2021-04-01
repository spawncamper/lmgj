using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("animator is null");
        }
    }

    // called by animator?
    public void IdleAnimation()
    {

    }

    // called by Animator?
    void RunningAnimation()
    {

    }
}
