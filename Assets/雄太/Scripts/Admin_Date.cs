using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Admin_Date : MonoBehaviour
{
    [SerializeField]
    private Move move;
    [SerializeField]
    private Admin admin;
    private float time;
    [SerializeField]
    private float SaveTime = 30;
    [SerializeField]
    private Vector3 FirstCharaPOsition;

    private void Awake() 
    {
        if(PlayerPrefs.GetFloat("posi_x") == FirstCharaPOsition.x)
        {
            PlayerPrefs.SetFloat("posi_X" , FirstCharaPOsition.x);
            PlayerPrefs.SetFloat("posi_y" , FirstCharaPOsition.y);
            PlayerPrefs.SetFloat("posi_z" , FirstCharaPOsition.z);
            PlayerPrefs.SetFloat("LEVEL" , 1);
            PlayerPrefs.SetFloat("EXP" , 0);
            PlayerPrefs.Save();
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
        time += Time.deltaTime;
        if(time > SaveTime)
        {
            SaveDate();
            time = 0;
        }
    }

    public void SaveDate()
    {
        var posi =  move.transform.position;
        PlayerPrefs.SetFloat("posi_X" , posi.x);
        PlayerPrefs.SetFloat("posi_y" , posi.y);
        PlayerPrefs.SetFloat("posi_z" , posi.z);
        PlayerPrefs.SetFloat("EXP", Admin. MainEXP);
        PlayerPrefs.SetInt("LEVEL", Admin.CharaLevel);

        PlayerPrefs.Save();
        Debug.Log("データをセーブしました");
    }
}
