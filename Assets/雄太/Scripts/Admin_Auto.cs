using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Admin_Auto : MonoBehaviour
{
    [SerializeField]
    private CharaState state;
    [SerializeField]
    private float HPStatus = 100;
    [SerializeField]
    private float AttackStart = 10;
    
    [SerializeField]
    public Transform enemyTransform;

    private Animator animator;
    private CharacterController characterController;
    private AudioSource audioSource;

    [SerializeField]
    private Admin_Effect[] admin_Effects;
    [SerializeField]
    private AudioClip[] audioClips;



    private enum CharaState
    {
        Idle,
        Move,
        Attack,
        Avoidance,
        Damage,
        HighAttack
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        SetState(CharaState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        var distance = Vector3.SqrMagnitude(transform.position - enemyTransform.position);
        Quaternion look = new Quaternion(0,0,0,0);
        if(distance > 4)
        {
            if(state != CharaState.Move)SetState(CharaState.Move);
            
        }
        else if (distance <= 4)
        {
            SetState(CharaState.Attack);
        }



        if(state == CharaState.Move || state == CharaState.Attack)
        {
            look = Quaternion.LookRotation(-enemyTransform.position);
        }
        transform.LookAt(enemyTransform);
        Debug.Log(enemyTransform.position);

        // transform.rotation = Quaternion.RotateTowards(transform.rotation , look ,300*Time.deltaTime);
    }

    private void SetState(CharaState tempState)
    {
        state = tempState;
        if(tempState == CharaState.Idle)
        {

        }
        else if(tempState ==  CharaState.Move)
        {
            animator.SetFloat("Speed" , 1);

        }
        else if(tempState == CharaState.Attack)
        {
            animator.SetTrigger("Attack");
            animator.SetFloat("Speed" , 0);
        }
    }
}
