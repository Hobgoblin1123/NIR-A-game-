using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Admin_Chest : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private ParticleSystem particleSystem;
    private void Start() {
        animator = GetComponent<Animator>();
    }
    public void Open()
    {
        animator.SetTrigger("Open");
        particleSystem.Play();
    }
}
