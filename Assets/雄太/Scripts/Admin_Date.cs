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
            PlayerPrefs.SetFloat("Aim_X" , admin_UI.AIM_X_speed);
            PlayerPrefs.SetFloat("Aim_Y" , admin_UI.AIM_Y_speed);
        }

        PlayerPrefs.Save();
    }

    public void ChangeShadowBool()
    {
        if(Mainlight.shadows == LightShadows.None)
        {
            Mainlight.shadows = LightShadows.Soft;
        }
        else if(Mainlight.shadows == LightShadows.Soft)
        {
            Mainlight.shadows = LightShadows.None;
        }
    }
}
