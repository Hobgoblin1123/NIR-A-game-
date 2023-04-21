using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Fungus;

public class Admin : MonoBehaviour
{
    [Header("ステータス") , Tooltip("主人公のステータス　　レベル以外は設定しても意味ないよ")]
        public static int CharaLevel;          //０は武器なし
        public static float MainEXP;          //１は槍
        public static float AttackStatus;     //２は剣
        public static float HPStatus;         //３は銃
        public static float DefenceStatus;

    [Space(8)]

    [Header("設定オブジェクト") , Tooltip("このスクリプトで使うゲームオブジェクト、プレファブを格納する所")]
        [SerializeField]
        private Text[] Statust_UI;
        [SerializeField]
        private InputField aim_y_speed;
        [SerializeField]
        private InputField aim_x_speed;
        [SerializeField]
        private Slider VolumeSlider;
        [SerializeField]
        private Transform TalkButtonPanel;
        [SerializeField]
        ChangeEquip changeEquip;
        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;
        [SerializeField]
        private Animator animator; 
        // [SerializeField]
        // private Slider EnemyHP;
        [SerializeField]
        private RectTransform SelectEdge;


    [Space(8)]


    [Header("設定変数") , Tooltip("変数を設定する所")]
        [SerializeField]
        private AudioClip[] BGM;
        [SerializeField]
        public float LockDistance = 10;
        public float AIM_X_Speed = 8;
        public float AIM_Y_speed = 8;
        public static int MainWeapon = 2;
        public static int SubWeapon = 1;  
        public float LockOffDistance;

    [Space(8)]

    [Header("状態表示") , Tooltip("状態が表示される　それ以上でもそれ以下でもない")]
        
        public bool LockOn;
        public GameObject LockEnemy;
        public bool InTalkFlag;
        public List<TalkEvent> talkObject;
        public TalkEvent TalkTargetObj; 
        public static int WeaponNumber;
        


    private int LevelUPEXP;
    [SerializeField]
    private Move characterScript;     
    private AudioSource audioSource;
    
    private SearchEnemy searchEnemy; 
    private CinemachinePOV cinemachinePOV;
    private Flowchart flowchart;
    [HideInInspector]
    public Transform targetTransform; 
    // private Canvas EnemyHPCanvas;
    [SerializeField]
    private int SelectEdNumber;

    // Start is called before the first frame update
    void Awake()
    {
        CharaLevel = PlayerPrefs.GetInt("LEVEL");
        MainEXP = PlayerPrefs.GetFloat("EXP");
        MainWeapon = PlayerPrefs.GetInt("MainW");
        SubWeapon = PlayerPrefs.GetInt("SubW");
        HPStatus = 80 + CharaLevel*20;
        AttackStatus = 8 + CharaLevel*2;
        LevelUPEXP = 10 * CharaLevel * CharaLevel + 10 * CharaLevel;
        DefenceStatus = CharaLevel * 0.005f;
        WeaponNumber = MainWeapon;
        cinemachinePOV = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        characterScript = GetComponentInParent<Move>();
    }

    // Update is called once per frame
    void Start()
    {
        flowchart = GetComponentInChildren<Flowchart>();
        audioSource = GetComponent<AudioSource>();
        searchEnemy = GetComponentInChildren<SearchEnemy>();
        // EnemyHPCanvas = EnemyHP.GetComponentInParent<Canvas>();
        
        Statust_UI[0].text = string.Format("MaxHP " + HPStatus );
        Statust_UI[1].text = string.Format("Attack Power " + AttackStatus);
        Statust_UI[2].text = string.Format("Defence Power " + DefenceStatus);
        Statust_UI[3].text = string.Format("All EXP " + MainEXP);
        Statust_UI[4].text = string.Format("次レベル必要総経験値　"　+ LevelUPEXP);
        Statust_UI[5].text = string.Format("Level  " + CharaLevel);

        audioSource.clip = BGM[4];
        audioSource.Play();

        LockOn = false;
        //cinemachineのAIMのSpeedの値を初期値に設定
        cinemachinePOV.m_HorizontalAxis.m_MaxSpeed  = AIM_X_Speed;
        cinemachinePOV.m_VerticalAxis.m_MaxSpeed = AIM_Y_speed;
        // EnemyHP.maxValue = 1;
        // EnemyHP.minValue = 0;
        // EnemyHP.value = 1;
    }

    void Update()
    {

        if(Input.GetKeyDown("f"))
        {
            Select();
        }
        
        if(InTalkFlag == true && characterScript.state != Move.MyState.TalkEvent)
        {
            var n = -Input.GetAxis("Mouse ScrollWheel")*10;
            if(SelectEdNumber <= talkObject.Count - 1 && 0<=SelectEdNumber) 
            {
                SelectEdNumber += (int)n;
            }
            
            if(SelectEdNumber < 0)
            {
                SelectEdNumber = 0;
            }           
            else if(SelectEdNumber > talkObject.Count-1)  // -250 -285 35間隔
            {
                SelectEdNumber = talkObject.Count - 1;
            }
            SelectEdge.anchoredPosition = new Vector3(160 , 15.5f - 35 *SelectEdNumber ,0);
        }
    }


    public void ReturnAccess()
    {    
        
        animator.SetFloat("0Speed" , 0);

        animator.ResetTrigger("1Attack");
        animator.ResetTrigger("2Attack");
        animator.SetFloat("1Speed", 0);
        animator.SetFloat("3Speed", 0);
        animator.SetFloat("2Speed", 0);
        animator.SetBool("1Idle", false);
        animator.SetBool("2Idle", false);
        characterScript.SetState(Move.MyState.Normal); 
        
    }

    public void TakeEXP(float EXP)
    {
        MainEXP += EXP;
        Debug.Log("EXP = " + EXP);


        while(MainEXP >= LevelUPEXP)
        {
            CharaLevel++;
            HPStatus = 80 + CharaLevel*20;
            LevelUPEXP = 10 * CharaLevel * CharaLevel + 10 * CharaLevel;
            AttackStatus = 8 + CharaLevel*2;
            DefenceStatus = CharaLevel * 0.005f;
            characterScript.LevelUP();
            Debug.Log("キャラレベル　　" + CharaLevel);
        }


        Admin_Date.SaveDateOther(1);
        //UIにステータスを表示する
        Statust_UI[0].text = string.Format("MaxHP " + HPStatus );
        Statust_UI[1].text = string.Format("Attack Power " + AttackStatus);
        Statust_UI[2].text = string.Format("Defence Power " + DefenceStatus);
        Statust_UI[3].text = string.Format("All EXP " + MainEXP);
        Statust_UI[4].text = string.Format("次レベル必要総経験値　"　+ LevelUPEXP);
        Statust_UI[5].text = string.Format("Level  " + CharaLevel);
    }

    public void ChangeBGM(int BGMnumber)
    {
        if(audioSource.clip != BGM[BGMnumber])
        {
            audioSource.clip = BGM[BGMnumber];
            audioSource.Play();            
        }

    }

    public void Volum()
    {
        audioSource.volume = VolumeSlider.value;
    }

    public void LockOnEnemy()
    {
        // LockEnemy = searchEnemy.Serch();
        LockEnemy = searchEnemy.Serch();
        if(LockEnemy == null) return;
        

        float dis = Vector3.SqrMagnitude(LockEnemy.transform.position-this.transform.position);
        var adminEnemy = LockEnemy.GetComponent<Admin_EnemyStatus>();
        if(dis <= LockDistance*LockDistance && adminEnemy.TalkFalg() == false && adminEnemy.HP > 0)
        {
            adminEnemy.LockedOn();
            LockOn = true;
            // EnemyHPCanvas.enabled = true;
            // EnemyHP.value = adminEnemy.NowHPRatio;
        }
        else
        {
            LockEnemy = null;
        }
    }

    public void SecondLockOn()
    {
        
        // Ray ray = new Ray(transform.position , characterScript.velocity);
        RaycastHit hit;
        // Debug.Log("あぁぁぁぁぁあ");
        // Debug.DrawRay(transform.position, ray.direction * 10, Color.red, 5);
        if(Physics.BoxCast(transform.position , new Vector3(2,0.5f,0) , transform.forward , out hit , Quaternion.identity , 4)&& hit.collider.tag == "Enemy")
        {
            if(hit.collider.gameObject == LockEnemy) return;
            var s =  hit.transform.GetComponent<Admin_EnemyStatus>();
            s.LockedOff();
            if(s.TalkFalg() == true) return;
            LockEnemy = s.gameObject;
            s.LockedOn();
            // EnemyHPCanvas.enabled = true;
            // EnemyHP.value = s.NowHPRatio;
            //  hit.transform.gawmeObject;
        }
        
    }

    public void LockOff()
    {
        if(LockEnemy != null)
        {
            LockEnemy.GetComponent<Admin_EnemyStatus>().LockedOff();
        }
       
        LockOn = false;
        LockEnemy = null;
        // EnemyHPCanvas.enabled = false;
    }


    public void ChangeMainWeapon(int number)
    {
        MainWeapon = number;
        ReturnAccess();
        changeEquip.ChangeWeapon();
        Admin_Date.SaveDateOther(2);
    }

    public void ChangeSubWeapon(int number)
    {
        SubWeapon = number;
        ReturnAccess();
        changeEquip.ChangeWeapon();
        Admin_Date.SaveDateOther(2);
    }

    public void ChangeAIMSpeed()
    {
        // string Text 
        AIM_X_Speed = float.Parse(aim_x_speed.text);
        AIM_Y_speed = float.Parse(aim_y_speed.text);
        Debug.Log(AIM_X_Speed);
        cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = AIM_X_Speed;
        cinemachinePOV.m_VerticalAxis.m_MaxSpeed = AIM_Y_speed;
    }


    public void Talk(int n , TalkEvent talkEvent)
    {
        flowchart.SendFungusMessage("Talk" + n);
        targetTransform = talkEvent.transform;
        TalkTargetObj = talkEvent;
        characterScript.SetState(Move.MyState.TalkEvent);
        TalkButtonPanel.gameObject.SetActive(false);
        SelectEdge.gameObject.SetActive(false);
    }

    public  void TalkEnd()
    {
        characterScript.SetState(Move.MyState.Normal);
        TalkTargetObj.TalkOff();
        TalkTargetObj = null;
        targetTransform = null;
        TalkButtonPanel.gameObject.SetActive(true);
        SelectEdge.gameObject.SetActive(true);
    }

    public void InTalkRange(TalkEvent obj)
    {
        talkObject.Add(obj);
        RefreshTalkButton();
        if(InTalkFlag == false)
        {
            InTalkFlag =true;
            SelectEdge.gameObject.SetActive(true);
            SelectEdNumber = 0;
        }
    }

    public void OutTalkRange(TalkEvent obj)
    {
        talkObject.Remove(obj);
        RefreshTalkButton();
        if(talkObject.Count == 0)
        {
            InTalkFlag =false;
            SelectEdge.gameObject.SetActive(false);
        }
    }

    public void RefreshTalkButton()
    {
        foreach(Transform child in TalkButtonPanel.transform)
        {
            Destroy(child.gameObject);
        }
        var Count = talkObject.Count;
        for (int i = 0; i < Count; i++)
        {
            Instantiate(talkObject[i].talkStartButton.gameObject, new Vector3(0,0,0), Quaternion.Euler(0,0,0), TalkButtonPanel);
        }
    }

    private void Select()
    {
        if(InTalkFlag == true)
        {
            talkObject[SelectEdNumber].TalkStart();
        }
        
    }


    void OnTriggerStay(Collider col) 
    {
        if(col.tag == "Appear")
        {
            
            var appearScript = col.GetComponent<AppearScript>();
            if(appearScript.encountCharaFlag == false)
            {
                appearScript.CharacterConnect();
            }
            else
            {
                appearScript.CharacterPosition = transform.parent.transform.position;
            }
        }
    }

    void OnTriggerExit(Collider col) 
    {
        if(col.tag == "Appear")
        {
            Debug.Log("きえたーーーー");
            col.GetComponent<AppearScript>().CharacterDisconnect();
        }
    }
    // public void ChangeEnemyHP(float f)
    // {
    //     EnemyHP.value = f;
    // }

    public void ChangeAutoRun()
    {
        characterScript.AutoRunFlag = !characterScript.AutoRunFlag;
    }
}