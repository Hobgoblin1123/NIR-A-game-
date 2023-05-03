using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Admin_Auto : MonoBehaviour
{
    [SerializeField]
    private CharaState state;
    [SerializeField]
    private float HPStatus = 100;
    [SerializeField]
    private float AttackStatus = 10;

    public float hp;
    
    [SerializeField]
    public Transform enemyTransform;
    private Animator animator;
    private CharacterController characterController;
    private AudioSource audioSource;

    [SerializeField]
    private Admin_Effect[] admin_Effects;
    [SerializeField]
    private AudioClip[] audioClips;
    [SerializeField]
    public Slider hpSlider;
    [SerializeField]
    private float revivePoint;
    [SerializeField]
    private ReviveArea reviveArea;



    private enum CharaState
    {
        Idle,
        Move,
        Attack,
        Avoidance,
        Damage,
        HighAttack,
        Down
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        SetState(CharaState.Idle);
        hp = HPStatus;
        hpSlider.value = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == CharaState.Down)
        {
            revivePoint += 0.1f*Time.deltaTime;
            return;
        }
        var distance = Vector3.SqrMagnitude(transform.position - enemyTransform.position);
        Quaternion look = new Quaternion(0,0,0,0);
        if(distance > 4)
        {
            if(state != CharaState.Move)SetState(CharaState.Move);
            
        }
        else if (distance <= 4 && (state == CharaState.Idle || state == CharaState.Move))
        {
            SetState(CharaState.Attack);
        }

        if(state == CharaState.Move || state == CharaState.Attack)
        {
            look = Quaternion.LookRotation(enemyTransform.position -transform.position);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation , look ,600*Time.deltaTime);
    }

    private void SetState(CharaState tempState)
    {
        var PostState = state;
        state = tempState;
        if(tempState == CharaState.Idle)
        {
            animator.ResetTrigger("Attack");
            animator.SetFloat("Speed" , 0);
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
        else if(tempState == CharaState.Damage)
        {
            animator.Play("Damage");
        }
        else if(tempState == CharaState.Avoidance)
        {
            if(PostState == CharaState.Move)
            {
                animator.Play("前方回避");
            }
            else
            {
                animator.Play("後方回避");
            }
        }
        else if(tempState == CharaState.Down)
        {
            animator.SetTrigger("Die");
            animator.ResetTrigger("Attack");
            animator.SetFloat("Speed" , 0);
        }
    }
    public void TakeDamage(float damage)
    {
        if(state ==CharaState.Down)return;

        if(Random.Range(0,4) == 0)
        {
            SetState(CharaState.Avoidance);
            return;
        }
        hp -= damage;
        hpSlider.value = hp/HPStatus;

        if(state == CharaState.Avoidance)return;
        SetState(CharaState.Damage);

        if(hp <= 0)
        {
            SetState(CharaState.Down);
            Instantiate(reviveArea,transform.position,transform.rotation,transform);
        }
    }

    public void Revive()
    {
        SetState(CharaState.Idle);
        hpSlider.value = 1;
    }
    public void StateEnd()
    {
        if(state == CharaState.Down)return;
        SetState(CharaState.Idle);
    }
    public void EffectOff()
    {

    }
    public void EffectOn()
    {

    }
    public void FootSound()
    {

    }
    public void AttackStart()
    {

    }
}
