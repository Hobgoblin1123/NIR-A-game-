using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;


public class Move : MonoBehaviour
{


	private CharacterController characterController;
	[HideInInspector]
	public Vector3 velocity;
	private Animator animator;
	private float Speed;
	// public MyWeapon weapon;
    public MyState state;
	private Quaternion targetRotation;
	private Admin admin;
	// public bool InvincibleFlag;
	private AudioSource audioSource;
	[SerializeField]
	private AudioClip[] sound;
	[SerializeField]
	private Admin_Effect[] effects;
	// HP関連

	private  float charamaxHp;	// 敵キャラのHP最大値を100とする
    private float charahp;  //HP
    [SerializeField]
    private Slider slider;	// シーンに配置したSlider格納用
    private float DefencePoint;
	[SerializeField]
	private float lockInterval;
	[SerializeField]
	private float physicalStrength = 15;
	private Slider physicalStrengthSlider;
	[SerializeField]
	private Canvas staminaCanvas;

	[SerializeField]
	private CanvasGroup AvoidanceDisplayEffect;
	public float EffectVolum = 0.4f;
	private int animSpeedHash;
	public bool AutoRunFlag = true;
	[SerializeField]
	private Camera mainCamera;
	[SerializeField]
	private FixedJoystick fixedJoystick;
	public bool isWorkeingMobilePlatform;

	void Awake()
    {
		var posi = new Vector3(0,0,0);
		posi.x = PlayerPrefs.GetFloat("posi_X");
		posi.y = PlayerPrefs.GetFloat("posi_y");
		posi.z = PlayerPrefs.GetFloat("posi_z");
		transform.position = posi;
		//初期化
		targetRotation = transform.rotation;
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) 
		{
			isWorkeingMobilePlatform = true;
		}
		else if (Application.platform == RuntimePlatform.WindowsPlayer) 
		{
			isWorkeingMobilePlatform = false;
		}
	}


	public enum MyState
   {
	Normal,
	Damage,
	Attack,
	SPAttack,
	JUAttack,
	Jump,
	TalkEvent,
	Avoidance
    }

	// public enum MyWeapon
    // {
	// 	noweapon,
	// 	Sword,
	// 	Spia,
	// 	Gun
    // }
	// Use this for initialization

	void Start()
	{
		characterController = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
		admin = GetComponentInChildren<Admin>();
		audioSource = GetComponent<AudioSource>();
		physicalStrengthSlider = staminaCanvas.GetComponentInChildren<Slider>();
		animSpeedHash = Animator.StringToHash("2Speed");
		charamaxHp = Admin.HPStatus;
        slider.maxValue = charamaxHp;    // Sliderの最大値を敵キャラのHP最大値と合わせる
        charahp = charamaxHp;      // 初期状態はHP満タン
        slider.value = charahp;	// Sliderの初期状態を設定（HP満タン）
        DefencePoint = Admin.DefenceStatus;
		AvoidanceDisplayEffect.alpha = 0;
		physicalStrengthSlider.value = 1;
	}
    public void TakeDamage(float damage)
    {
		if(state != MyState.Avoidance && state != MyState.JUAttack)
		{
			SetState(MyState.Damage);
			charahp -= damage * (1 - DefencePoint);
            slider.value = charahp;	// Sliderに現在HPを適用
            Debug.Log("主人公HP = " + slider.value);
		}
		else if(state != MyState.JUAttack)
		{
			Time.timeScale = 0.2f;
			StartCoroutine(ResetTimeScale());
			effects[3].gameObject.SetActive(true);
			effects[3].EffectStart(0);
			SetState(MyState.JUAttack);
			audioSource.volume = EffectVolum;
			audioSource.PlayOneShot(sound[11]);
			physicalStrength += 3;
			physicalStrengthSlider.value = physicalStrength/5;
			Debug.Log("ジャスト回避");
		}

		if (slider.value <= 0)
        {
            Debug.Log("死亡");
        }
	}

	IEnumerator ResetTimeScale()
	{
		var x = new WaitForSecondsRealtime(0.01f);
		var y = new WaitForSecondsRealtime(0.05f);
		var z = new WaitForSecondsRealtime(0.15f);
		var q = new WaitForSecondsRealtime(0.02f);
		float n = 0;
		while(n<1)
		{
			n = n + 0.3f;
			AvoidanceDisplayEffect.alpha = n;
			yield return x;
		}
		yield return y;
		Time.timeScale = 1;
		yield return z;
		while(n>0)
		{
			n = n - 0.1f;
			AvoidanceDisplayEffect.alpha = n;
			yield return q;
		}
		yield return new WaitForSecondsRealtime(1);
		effects[3].EffectEnd();
		effects[3].gameObject.SetActive(false);
	}

	public void LevelUP()
    {
        charamaxHp = Admin.HPStatus;
		DefencePoint = Admin.DefenceStatus;
        slider.maxValue = charamaxHp;    // Sliderの最大値を敵キャラのHP最大値と合わせる
        charahp = charamaxHp;      // 初期状態はHP満タン
        slider.value = charahp;	// Sliderの初期状態を設定（HP満タン）
        Debug.Log("レベルアップ　現在HPは"　+ charamaxHp);
    }

	// Update is called once per frame
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.H))
		{
			SetState(MyState.JUAttack);

		}

		if(physicalStrength < 15)
		{
			physicalStrength += Time.deltaTime;
			physicalStrengthSlider.value = physicalStrength/15;
		}
		else
		{
			staminaCanvas.enabled  = false;
		}

		if (state == MyState.Normal)
		{
			var horizontal = 0f;
			var vertical = 0f;
			if(isWorkeingMobilePlatform == false)
			{
				horizontal = Input.GetAxis("Horizontal");
				vertical = Input.GetAxis("Vertical");
			}
			else
			{
				horizontal = fixedJoystick.Horizontal;
				vertical = fixedJoystick.Vertical;
			}
			

			var horizontalRotation = Quaternion.AngleAxis( mainCamera.transform.eulerAngles.y, Vector3.up);
			velocity = horizontalRotation * new Vector3(horizontal, 0, vertical).normalized;
			// var speed = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;

			if (velocity.magnitude > 0.5f)
			{
				targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
			}

			
			if(AutoRunFlag == false)
			{
				if(Input.GetKey(KeyCode.LeftShift))
				{
					Speed = velocity.magnitude + velocity.magnitude;
				}
				else
				{
					Speed = velocity.magnitude;
				}
			}
			else
			{
				if(Input.GetKey(KeyCode.LeftShift))
				{
					Speed = velocity.magnitude;
				}
				else
				{
					Speed = velocity.magnitude + velocity.magnitude;
				}
			}

			// if(horizontal < 0.1f && vertical < 0.1f && animator.GetFloat(animSpeedHash) > 1.8f)
			// {
			// 	animator.SetTrigger("stop");
			// 	animator.SetFloat(animSpeedHash,0);
			// }
			
			// 移動速度をAnimatorに反映
			animator.SetFloat(animSpeedHash, Speed , 0.3f , Time.deltaTime);
			animator.SetBool(Admin.WeaponNumber + "Idle", true);

			// if(Input.GetButtonDown("Jump"))
			// {
			// 	SetState(MyState.Jump);
			// }
			
			// if(Input.GetKeyDown("f") && Admin.WeaponNumber == 1 && state == MyState.Normal)
			// {
			// 	SetState(MyState.SPAttack);
			// }
		}
		

		if(state == MyState.Normal || state == MyState.Avoidance)
		{
			if((Admin.WeaponNumber != 0) && (Admin.WeaponNumber != 3))
			{

				if(EventSystem.current.IsPointerOverGameObject())
				{
                    return;
                }
			}			
		}

		if(Input.GetButtonDown("Fire1"))GetInput(1);

		if(Input.GetMouseButtonDown(1))GetInput(0);

		if(state == MyState.TalkEvent)
		{
			Vector3 lockenemy = -transform.position +new Vector3(admin.targetTransform.position.x ,transform.position.y , admin.targetTransform.position.z);
			targetRotation = Quaternion.LookRotation(lockenemy);
		}

		if(admin.LockOn == true)
		{
			if(state == MyState.Attack || state == MyState.SPAttack || state == MyState.JUAttack)
			{
				Vector3 lockenemy = new Vector3(admin.LockEnemy.transform.position.x ,transform.position.y , admin.LockEnemy.transform.position.z) - transform.position;
				targetRotation = Quaternion.LookRotation(lockenemy);
			}

			if(Input.GetKeyDown("q"))
			{
				admin.LockOff();
			}
		}
		transform.rotation = Quaternion.RotateTowards(transform.rotation , targetRotation , 600 * Time.deltaTime);

		if(admin.LockEnemy == true)
		{
			lockInterval += Time.deltaTime;
		}
	}

	public void GetInput(int n)
	{
		if(state != MyState.TalkEvent)
		{
			if(state != MyState.JUAttack)
			{
				if(n == 0 && physicalStrength > 2f && state != MyState.Damage && state !=MyState.Avoidance)
				{
					SetState(MyState.Avoidance);
					physicalStrength -= 3;
					physicalStrengthSlider.value = physicalStrength/15;
					staminaCanvas.enabled = true;
				}

				if(n == 1 && state != MyState.Avoidance)
				{
					SetState(MyState.Attack);
				}
			
			}
		}
	}
	
	public void SetState(MyState tempState)
	{
		if (tempState == MyState.Normal)
		{
			state = MyState.Normal;
		}
		else if (tempState == MyState.Attack)
		{
			state = MyState.Attack;
			animator.SetTrigger(Admin.WeaponNumber + "Attack");
			if(admin.LockOn == false)
			{
				admin.LockOnEnemy();
			}
			else if(lockInterval > 3)
			{
				admin.SecondLockOn();
				lockInterval = 0;
			}
		}
		else if (tempState == MyState.SPAttack)
		{
			state = MyState.SPAttack;
			animator.SetTrigger(Admin.WeaponNumber + "SPAttack");
			if(admin.LockOn == false)
			{
				admin.LockOnEnemy();
			}	  
			else if(lockInterval > 3)
			{
				admin.SecondLockOn();
				lockInterval = 0;
			}
		}
		else if(tempState == MyState.JUAttack)
		{
			state = MyState.JUAttack;
			if(admin.LockOn == false)
			{
				admin.LockOnEnemy();
			}
			animator.SetTrigger(Admin.WeaponNumber + "JUAttack");
		}
		else if(tempState == MyState.Jump)
		{
			animator.SetTrigger("Jump");
		}
		else if (tempState == MyState.Avoidance)
		{
			state = MyState.Avoidance;
			if(Speed > 0.6)
			{
				animator.Play("回避.前方回避1" , 0,0.1f);
				// InvincibleFlag = true;
			}
			else
			{
				animator.Play("回避.後方回避1" , 0,0.35f);
				// InvincibleFlag = true;
			}
			ResetEffect();
		}
		else if(tempState == MyState.Damage)
		{
			velocity = Vector3.zero;
		    animator.Play("Damage");
		    animator.ResetTrigger(Admin.WeaponNumber + "Attack");
			ResetEffect();

		}
		else if(tempState == MyState.TalkEvent)
		{
			state = MyState.TalkEvent;
		}
	}

	// 以下　processcharaAnimaEvent
	void AttackStart(int n)
	{
		audioSource.volume = EffectVolum;
		audioSource.PlayOneShot(sound[n]);
	}

	void FootSound(int n)
	{
		if(n == 0)
		{
			audioSource.volume = 0.4f;
			audioSource.PlayOneShot(sound[Random.Range(12 , 16)]);
		}
	}

	public void LastAttack()
	{
		animator.ResetTrigger(Admin.WeaponNumber + "Attack");
        animator.ResetTrigger(Admin.WeaponNumber + "SPAttack");
        Debug.Log("ラストアタック");
	}

	public void StateEnd()
	{
		if(state != MyState.JUAttack)
		{
			SetState(MyState.Normal);
		}
	}

	// void EndDamage()
	// {
	// 	SetState(MyState.Normal);
	// }

	void JUAttackEnd()
	{
		SetState(MyState.Normal);
	}

	void Effect(AnimationEvent animationEvent)
	{
		GameObject EffectObj = (Instantiate(animationEvent.objectReferenceParameter,transform.position,transform.rotation,this.transform)as GameObject);
		EffectObj.GetComponent<AttackByEffect>().DamageMagnification = animationEvent.floatParameter;
	}

	void EffectOn(AnimationEvent animationEvent)
	{
		effects[animationEvent.intParameter].gameObject.SetActive(true);
		effects[animationEvent.intParameter].EffectStart((int)animationEvent.floatParameter);
	}

	void EffectOff(int n)
	{
		effects[n].EffectEnd();
		effects[n].gameObject.SetActive(false);
	}

	public void ResetEffect()
	{
		foreach (var item in effects) 
		{
			if(item.gameObject.activeSelf == true)
			{
				item.gameObject.SetActive(false);
				item.EffectEnd();
			}
		}
	}
}

