using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Admin_ForHeavenlyKing : MonoBehaviour
{
    public EnemyState state;
    private Animator animator;
    private Admin_EnemyStatus admin_EnemyStatus;
    private AudioSource audioSource;
    private CharacterController characterController;
    public int aiLevel = 1;






    private float elapsedTime = 0;
    private Quaternion lookPosition;
    public Transform charaTransform;
    private float distance;
    [SerializeField]
    private float longAttackInterval = 10;
    [SerializeField]
    private float maxWaiteTimeFormAttack = 4;
    [SerializeField]
    private AudioClip[] audioClips;
    [SerializeField]
    private Admin_EnemyEffect[] effect;
    private float longAttackIntervalTime;





    //定数
    const float CAN_ATTACL_DISTAMCE = 10;
    // const float LONG_ATTACK_INTERVAL = ;
    public enum EnemyState
    {
        Idle,
        Move,
        Attack,
        LongAttack,
        Avoidance,
        SPAttack1,
        SPAttack2,
        Die,
        Wait
    }
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        admin_EnemyStatus = GetComponent<Admin_EnemyStatus>();
        audioSource = GetComponent<AudioSource>();
        longAttackIntervalTime -= longAttackInterval;
        DecideNextState();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.SqrMagnitude(transform.position - charaTransform.position);

        if(state == EnemyState.Move)
        {
            lookPosition = Quaternion.LookRotation(transform.position - charaTransform.position);
            if(distance < CAN_ATTACL_DISTAMCE)
            {
                animator.SetFloat("Speed" , 0);
                SetState(EnemyState.Attack);
            }
        }
        else if(state == EnemyState.Wait)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime > 0)
            {
                DecideNextState();
            }
        }

        if(state != EnemyState.LongAttack)
        {
            longAttackIntervalTime += Time.deltaTime;
            if(longAttackIntervalTime > 0 && distance > CAN_ATTACL_DISTAMCE && state == EnemyState.Move && state == EnemyState.Idle && state == EnemyState.Wait)
            {
                SetState(EnemyState.LongAttack);
            } 
        }


        if(state == EnemyState.Wait && distance*2.5f > CAN_ATTACL_DISTAMCE)
        {
            SetState(EnemyState.Move);
        }

        if(aiLevel == 3)
        {
            lookPosition = Quaternion.LookRotation(charaTransform.position);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation , lookPosition , 300*Time.deltaTime);
    }

    public void SetState(EnemyState s)
    {

        Debug.Log("状態変更の関数が呼ばれたよぉ　状態は " + s);
        var PostState = state;
        state = s;
        if(s == EnemyState.Wait)
        {
            if(PostState == EnemyState.Move)
            {
                elapsedTime = 0;
            }
            else if(PostState == EnemyState.Attack || PostState == EnemyState.LongAttack)
            {
                elapsedTime = Random.Range(0 , maxWaiteTimeFormAttack +1);
            }
        }
        else if(s == EnemyState.Move)
        {
            animator.SetFloat("Speed" ,1);
        }
        else if(s == EnemyState.Attack)
        {
            int n = Random.Range(0,100);
            if(n < 50)
            {
                animator.SetTrigger("Attack1");
            }
            else if(n > 75)
            {
                animator.SetTrigger("Attack3");
            }
            else
            {
                animator.SetTrigger("Attack2");
            }
        }
        else if(s ==EnemyState.LongAttack)
        {
            int n = Random.Range(0,100);
            if(n < 50)
            {
                animator.SetTrigger("Attack5");
            }
            else if(n > 75)
            {
                animator.SetTrigger("Attack4");
            }
            else
            {
                animator.SetTrigger("Attack6");
            }
            longAttackIntervalTime = -longAttackInterval;
        }
    }

    private void DecideNextState()
    {
        Debug.Log("次の状態を決めまする");
        if(distance < CAN_ATTACL_DISTAMCE)
        {
            SetState(EnemyState.Attack);
        }
        else if(longAttackIntervalTime > 0)
        {
            SetState(EnemyState.LongAttack);
        }
        else
        {
            SetState(EnemyState.Move);
        }
    }

    public void StateEnd()
    {
        SetState(EnemyState.Wait);
    }

    public void SetLookRotation()
    {
        if(aiLevel == 3)return;

        lookPosition = Quaternion.LookRotation(charaTransform.position);
    }

    public void Sound(int n)
    {
        audioSource.PlayOneShot(audioClips[n]);
    }

    public void EffectOn(AnimationEvent animationEvent)
    {
        effect[animationEvent.intParameter].gameObject.SetActive(true);
        effect[animationEvent.intParameter].EffectStart((int)animationEvent.floatParameter);
    }
    public void EffectOff(int n)
    {
        effect[n].EffectEnd();
        effect[n].gameObject.SetActive(false);
    }
}
