using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Admin_UI : MonoBehaviour
{
    [SerializeField]
    private Canvas[] canvas;
    private ItemsDialog itemsDialog;
    private InventoryManager inventoryManager;
    [SerializeField]
    private GameObject PauseCanvas;
    [SerializeField]
    private Move move;
    [SerializeField]
    private Text FPS;
    private float time;
    [SerializeField]
    private Admin_Date admin_Date;
    [SerializeField]
    private Toggle toggle;
    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;
    private CinemachinePOV cinemachinePOV;
    [SerializeField]
    private InputField aim_x_speed;
    [SerializeField]
    private InputField aim_y_speed;
    public float AIM_X_speed;
    public float AIM_Y_speed; 
    
    
    // Start is called before the first frame update
    void Start()
    {
        itemsDialog = GetComponentInChildren<ItemsDialog>();
        inventoryManager = GetComponentInChildren<InventoryManager>();
        cinemachinePOV = virtualCamera.GetCinemachineComponent<CinemachinePOV>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        QualitySettings.vSyncCount = 0;
        AIM_X_speed = PlayerPrefs.GetFloat("Aim_X");
        AIM_Y_speed = PlayerPrefs.GetFloat("Aim_Y");
        aim_x_speed.text = AIM_X_speed.ToString();
        aim_y_speed .text = AIM_Y_speed.ToString();
        cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = AIM_X_speed;
        cinemachinePOV.m_VerticalAxis.m_MaxSpeed = AIM_Y_speed;
        foreach (var item in canvas)
        {
            item.enabled = false;
        }
        PauseCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            GetKeyEscape(0);
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            GetKeyEscape(1);
        
        }

        if(Input.GetKeyDown(KeyCode.V))
        {
            GetKeyEscape(2);
        
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            GetKeyEscape(3);
        }

        if(Input.GetKeyDown(KeyCode.LeftAlt))
        {
            ShowCursor();
        }

        if(Input.GetKeyUp(KeyCode.LeftAlt) && PauseCanvas.activeSelf == false)
        {
            HideCursor();
        }

        time += Time.deltaTime;
        if(time > 1)
        {
            FPS.text = 1/Time.deltaTime +  "fps";
            time = 0;
        }
    }

    public void GetKeyEscape(int n)
    {
        Toggle1();

        foreach (var item in canvas)
        {
            if(item.enabled == true)
            {
                item.enabled = false;
            }
        }
        
        if(n == 0)
        {
            canvas[0].enabled = true;
        }
        else if(n != 0)
        {
            ChangeCanvas(n);
        }
        else if(PauseCanvas == false)
        {
            Toggle1();
        }
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ChangeCanvas(int n)
    {
        if(canvas[0].enabled == true)
        {
            canvas[0].enabled = false;
        }
        canvas[n].enabled = true;
        if(n == 1)
        {
            itemsDialog.reflesh();
        }

    }

    public void BackPausePanel()
    {
        foreach (var item in canvas)
        {
            item.enabled = false;
        }
        canvas[0].enabled = true;
    }

    public void ChangeBagPanel()
    {
        if(canvas[0].enabled == true)
        {
            canvas[0].enabled = false;
        }
        canvas[1].enabled = true;
        itemsDialog.reflesh();
    }
    public void ChangeWeaponPanel()
    {
        if(canvas[0].enabled == true)
        {
            canvas[0].enabled = false;
        }
        canvas[2].enabled = true;
    }
    public void ChangeSkillTreePanel()
    {
        if(canvas[0].enabled == true)
        {
            canvas[0].enabled = false;
        }
        canvas[3].enabled = true;
    }

    public void Toggle1(){

        PauseCanvas.SetActive(!PauseCanvas.activeSelf);

        if (PauseCanvas.activeSelf)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            virtualCamera.enabled = false;
        }
        else
        {
            move.LastAttack();
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            virtualCamera.enabled = true;
        }
    }

    public void Save()
    {
        admin_Date.SaveDate();
        admin_Date.SaveDateOther(0);
    }

    public void ChangeAutoSave()
    {
        admin_Date.AutoSave = toggle.isOn;
    }

    public void ChangeFPS(int n)
    {
        Application.targetFrameRate = n;
    }

    public void ChangeAIMSpeed()
    {
        AIM_X_speed = float.Parse(aim_x_speed.text);
        if(AIM_X_speed <= 0)
        {
            AIM_X_speed = 1;
            aim_x_speed.text = "1";
        }
        AIM_Y_speed = float.Parse(aim_y_speed.text);
        if(AIM_Y_speed <= 0)
        {
            AIM_Y_speed = 1;
            aim_y_speed.text = "1";
        }
        
        cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = AIM_X_speed;
        cinemachinePOV.m_VerticalAxis.m_MaxSpeed = AIM_Y_speed;
        admin_Date.SaveDateOther(3);

        Debug.Log("感度を変更");
    }
}