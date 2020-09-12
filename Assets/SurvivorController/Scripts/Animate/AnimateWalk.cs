using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateWalk : MonoBehaviour
{

    private Animator animator;

    private bool isWalking = false;
    private bool isSprinting = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {

    }

    public void setWalking(bool walk)
    {
        if (walk && !isWalking)
        {
            animator.SetBool("isWalking", true);
            isWalking = true;
        }
        if (!walk && isWalking)
        {
            animator.SetBool("isWalking", false);
            isWalking = false;
        }
    }

    public void setSprinting(bool sprint)
    {
        if (sprint && !isSprinting)
        {
            animator.SetBool("isSprinting", true);
            isSprinting = true;
        }
        if (!sprint && isSprinting)
        {
            animator.SetBool("isSprinting", false);
            isSprinting = false;
        }
    }

}