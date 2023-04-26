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
    [SerializeField]
    private Admin_UI admin_UI;
    [SerializeField]
    private Light Mainlight;
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
            Admin.CharaLevel = 1;
            Admin.MainEXP = 0;

            rotatoSun.FirstRotate();
            var rote = rotatoSun.transform.rotation.eulerAngles;
            admin.ChangeMainWeapon(2);
            admin.ChangeSubWeapon(3);
            admin_UI.AIM_X_speed = 8;
            admin_UI.AIM_Y_speed = 8;
            admin_UI.NawFPS = 60;
            admin_UI.FullScreen = true;
            admin_UI.shadows = false;
            admin_UI.ScreenResolution = 1;
            admin_UI.MasterVolume = 1;
            admin_UI.BGMVolume = 1;
            admin_UI.SEVolume = 1;

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


    public void SaveDateOther(int n)
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

        if(n == 0 || n == 3)
        {
            PlayerPrefs.SetInt("FPS" , admin_UI.NawFPS);
        }

        if(n == 0 || n == 4)
        {
            PlayerPrefs.SetFloat("Aim_X" , admin_UI.AIM_X_speed);
            PlayerPrefs.SetFloat("Aim_Y" , admin_UI.AIM_Y_speed);
        }
        if(n == 0 || n == 5)
        {
            var i = 0;
            if(admin_UI.FullScreen == true)
            {
                i = 0;
            }
            else
            {
                i = 1;
            }
            PlayerPrefs.SetInt("FullScreen" ,i);
        }
        if(n == 0 || n == 6)
        {
            PlayerPrefs.SetInt("ScreenResolution" , admin_UI.ScreenResolution);
        }
        if(n == 0 || n == 7)
        {
            var s = 0;
            if(admin_UI.shadows == true)
            {
                s = 0;
            }
            else
            {
                s = 1;
            }
            PlayerPrefs.SetInt("Shadows" , s);
        }

        if(n == 0 || n == 8)
        {
            PlayerPrefs.SetInt("BGM" , admin_UI.BGMnumber);
        }
        if(n == 0 || n == 9)
        {
            PlayerPrefs.SetFloat("MVolume" , admin_UI.MasterVolume);
            PlayerPrefs.SetFloat("BGMVolume" , admin_UI.BGMVolume);
            PlayerPrefs.SetFloat("SEVolume" , admin_UI.SEVolume);
        }
        if(n == 0 || n == 10)
        {
            PlayerPrefs.SetInt("AutoRun" , move.AutoRunFlag? 0:1);
        }

        PlayerPrefs.Save();
        Debug.Log("個別情報を保存しました");
    }
}
