using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Admin_Date : MonoBehaviour
{
    [SerializeField]
    private Move move;
    [SerializeField]
    private Admin admin;
    [SerializeField]
    private RotatoSun rotatoSun;
    [SerializeField]
    private ChangeEquip changeEquip;
    private float time;
    [SerializeField]
    private float SaveTime = 30;
    [SerializeField]
    private Vector3 FirstCharaPOsition;
    [SerializeField]
    public bool AutoSave = true;
    

    private void Awake() 
    {
        if(PlayerPrefs.GetFloat("Sun_x") == 0)
        {
            move.transform.position = FirstCharaPOsition;
            // PlayerPrefs.SetFloat("posi_X" , FirstCharaPOsition.x);
            // PlayerPrefs.SetFloat("posi_y" , FirstCharaPOsition.y);
            // PlayerPrefs.SetFloat("posi_z" , FirstCharaPOsition.z);
            Admin.CharaLevel = 1;
            Admin.MainEXP = 0;
            Admin.MainWeapon = 2;
            Admin.SubWeapon = 0;
            // PlayerPrefs.SetFloat("LEVEL" , 1);
            // PlayerPrefs.SetFloat("EXP" , 0);
            // PlayerPrefs.SetInt("MainW" , 2);
            // PlayerPrefs.SetInt("SubW" , 0);

            rotatoSun.FirstRotate();
            var rote = rotatoSun.transform.rotation.eulerAngles;
            admin.ChangeMainWeapon(2);
            admin.ChangeSubWeapon(0);
            // PlayerPrefs.SetFloat("Sun_x" , rote.x);
            // PlayerPrefs.SetFloat("Sun_y" , rote.y);
            // PlayerPrefs.SetFloat("Sun_z" , rote.z);
            // PlayerPrefs.SetFloat("TimeRote" , rotatoSun.rote);
            // PlayerPrefs.Save();
            SaveDate();
            SaveDateOther(0);
            Debug.Log("初期追加");
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(AutoSave == true)
        {
            time += Time.deltaTime;
            if(time > SaveTime)
            {
                SaveDate();
                
            }
        }
        
    }

    public void SaveDate()
    {
        //キャラクターポジション情報
        var posi =  move.transform.position;
        PlayerPrefs.SetFloat("posi_X" , posi.x);
        PlayerPrefs.SetFloat("posi_y" , posi.y);
        PlayerPrefs.SetFloat("posi_z" , posi.z);
        
        //太陽位置、時間情報
        var rote = rotatoSun.transform.rotation.eulerAngles;
        PlayerPrefs.SetFloat("Sun_x" , rote.x);
        PlayerPrefs.SetFloat("Sun_y" , rote.y);
        PlayerPrefs.SetFloat("Sun_z" , rote.z);
        PlayerPrefs.SetFloat("TimeRote" , rotatoSun.rote);

        PlayerPrefs.Save();
        time = 0;
        Debug.Log("データをセーブしました");
    }


    public static void SaveDateOther(int n)
    {
        if(n == 0 || n == 1)
        {
            //主人公parameter
            PlayerPrefs.SetFloat("EXP", Admin. MainEXP);
            PlayerPrefs.SetInt("LEVEL", Admin.CharaLevel);
        }

        if(n == 0 || n == 2)
        {
            PlayerPrefs.SetInt("MainW" , Admin.MainWeapon);
            PlayerPrefs.SetInt("SubW" , Admin.SubWeapon);
        }

        // if(n == 0 || n == 3)
        // {
        //     PlayerPrefs.SetInt("NowW" , Admin.WeaponNumber);
        // }

        PlayerPrefs.Save();

    }
}
