using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerAnimationHandler : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnim(float value)
    {
        animator.SetFloat("Move", value);
    }
}
