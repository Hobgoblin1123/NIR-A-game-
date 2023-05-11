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
    private float longAttackInterval = 10;
    [SerializeField]
    private AudioClip[] audioClips;
    [SerializeField]
    private Admin_EnemyEffect[] effect;
    private float longAttackIntervalTime;
    private int damageCount;
    private int takeDistanceDamageCount = 10;
    [SerializeField]
    private Transform[] takeDistancePosition;
    private bool isHalfHP = false;
    private bool SPAttack;





    //定数
    const float CAN_ATTACL_DISTAMCE = 18;
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
        SPAttackEnd,
        Die,
        Wait
    }

    void Awake()
    {
        admin_EnemyStatus = GetComponent<Admin_EnemyStatus>();
        if(aiLevel == 1)
        {
            longAttackInterval = 20;
            takeDistanceDamageCount = 80;
            admin_EnemyStatus.EnemyLevel = 40;
            admin_EnemyStatus.RiseAttackStatus = 20;
        }
        else if(aiLevel == 2)
        {
            longAttackInterval = 8;
            takeDistanceDamageCount = 60;
            admin_EnemyStatus.EnemyLevel = 50;
            admin_EnemyStatus.RiseAttackStatus = 10;
        }
        else if( aiLevel == 3)
        {
            longAttackInterval = 3;
            takeDistanceDamageCount = 20;
            admin_EnemyStatus.EnemyLevel = 60;
            admin_EnemyStatus.RiseAttackStatus = 30;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        
        audioSource = GetComponent<AudioSource>();
        
        longAttackIntervalTime -= longAttackInterval;
        SetState(EnemyState.Move);
        SPAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.SqrMagnitude(transform.position - charaTransform.position);

        if(state == EnemyState.Move)
        {
            lookPosition = Quaternion.LookRotation(charaTransform.position - transform.position).normalized ;
            transform.LookAt(charaTransform);
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
            if(longAttackIntervalTime > 0 && distance > CAN_ATTACL_DISTAMCE && (state == EnemyState.Move || state == EnemyState.Idle || state == EnemyState.Wait))
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
            lookPosition = Quaternion.LookRotation(charaTransform.position - transform.position).normalized;
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation , lookPosition  , 600*Time.deltaTime);
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
                int n = aiLevel >= 3? 0:3;
                elapsedTime = Random.Range(4 - aiLevel , n +5-aiLevel);
            }
        }
        else if(s == EnemyState.Move)
        {
            animator.SetFloat("Speed" ,1);
        }
        else if(s == EnemyState.Attack)
        {
            animator.SetFloat("Speed" ,0);
            lookPosition = Quaternion.LookRotation(charaTransform.position - transform.position).normalized;
            int n = Random.Range(0,100);
            if(isHalfHP == false)
            {
                if(n < 40)
                {
                    animator.SetTrigger("Attack1");
                }
                else if(n > 60)
                {
                    animator.SetTrigger("Attack3");
                }
                else
                {
                    animator.SetTrigger("Attack2");
                }
            }
            else
            {
                if(n < 30)
                {
                    animator.SetTrigger("Attack1");
                }
                else if(n >= 30 && n < 50)
                {
                    animator.SetTrigger("Attack3");
                }
                else if(n > 90)
                {
                    animator.SetTrigger("Attack2");
                }
                else
                {
                    animator.SetTrigger("Attack7");
                }
            }
            
        }
        else if(s ==EnemyState.LongAttack)
        {
            animator.SetFloat("Speed" ,0);
            lookPosition = Quaternion.LookRotation(charaTransform.position - transform.position).normalized;
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
        else if(s == EnemyState.SPAttackEnd)
        {
            if(aiLevel < 3)
            {
                animator.SetFloat("Speed" , 0);
                animator.SetTrigger("Attack7");
                return;
            }

            animator.SetFloat("Speed" , 0);
            animator.SetTrigger("Attack8");
        }
    }

    public void Damage(float hp)
    {
        damageCount ++;
        var n = hp/admin_EnemyStatus.HPStatus;
        if(damageCount > takeDistanceDamageCount)
        {
            TakeDistance();
            damageCount = 0;
        }
        if(n <= 0.5f && isHalfHP == false)
        {
            isHalfHP = true;
            animator.SetFloat("Speed",0);
            animator.SetTrigger("Attack7");
        }
        else if(n <= 0.15 && SPAttack == false)
        {
            SPAttack = true;
            SetState(EnemyState.SPAttackEnd);
        }
        if(hp <= 0)
        {
            SetState(EnemyState.Die);
        }
    }

    private void TakeDistance()
    {
        transform.position = takeDistancePosition[Random.Range(0 , takeDistancePosition.Length)].position;
        effect[14].gameObject.SetActive(true);
        effect[14].EffectStart(0);
        SetState(EnemyState.LongAttack);
    }

    private void DecideNextState()
    {
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

        lookPosition = Quaternion.LookRotation(charaTransform.position - transform.position).normalized;
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
