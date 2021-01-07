using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTrap : MonoBehaviour
{

    public bool IsActive;
    bool ActivationEnd;
    public float RotateSpeed;

    Animation anim;
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animation>();
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsActive)
        {
            animator.SetBool("IsActive", true);
            if (ActivationEnd)
            {
                transform.Rotate(0, RotateSpeed, 0, Space.Self);
            }
        }
        else
        {
            animator.SetBool("IsActive", false);
            ActivationEnd = false;
        }
    }

    public void Rotate()
    {
        ActivationEnd = true;
    }
        
}
