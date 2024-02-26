using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerController : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void ResetTrigger(string triggerName)
    {
        animator.ResetTrigger(triggerName);
    }
}

    
