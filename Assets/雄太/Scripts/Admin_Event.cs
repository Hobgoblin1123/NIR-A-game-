using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;
using UnityEngine.UI;

public class Admin_Event : MonoBehaviour
{
    public int EventNumber = 0;
    [SerializeField]
    private PlayableDirector director;
    [SerializeField]
    private TimelineAsset[] timeline;
    [SerializeField]
    private Move move;
    [SerializeField]
    private Admin admin;
    [SerializeField]
    private GameObject Really;
    public int level;
    public int mainw;
    public int subw;
    private int naww;
    public bool withFight;
    [SerializeField]
    private GameObject HIROIN;
    [SerializeField]
    private GameObject BOSS;
    [SerializeField]
    private Text time;
    private float ntime;
    [SerializeField]
    private GameObject ENDPanel;
    
    // Start is called before the first frame update
    void Start()
    {
        director.Play(timeline[0]);
        admin.enabled = false;
        move.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(EventNumber == 5)
        {
            ntime += Time.deltaTime;
            time.text = ntime.ToString();
        }
    }

    public void NextEvent(int n)
    {
        EventNumber = n;
        if(n == 0)
        {
            admin.enabled = true;
            move.enabled = true;
        }
        else if(n == 1)
        {
            director.Play(timeline[1]);
            
            move.SetState(Move.MyState.Normal);
            move.enabled = false;
            admin.enabled = false;
        }
        if(n == 2)
        {
            director.Pause();
            
            move.SetState(Move.MyState.Normal);
            move.enabled = false;
            admin.enabled = false;
            // Cursor.lockState = CursorLockMode.None;
            // Cursor.visible = true;
        }
        if(n == 3)
        {
            director.Resume();
            // Cursor.lockState = CursorLockMode.Locked;
            // Cursor.visible = false;
            if(mainw == 0)mainw = 2;
            else if(subw == 0)subw = 2;
        }
        if(n == 4)
        {
            move.enabled = true;
        }
        if(n == 5)
        {
            move.enabled = true;
            admin.enabled = true;
            admin.ChangeMainWeapon(mainw);
            admin.ChangeSubWeapon(subw);
            move.transform.position = new Vector3(-25,0.4f,41);
            move.transform.eulerAngles = new Vector3(0,180,0);
            if(withFight == true)
            {
                HIROIN.SetActive(true);
            }
            BOSS.SetActive(true);
            ntime = 0;
        }
    }

    public void OpneWebPage()
    {
        Application.OpenURL("https://nir-a.sakura.ne.jp/contact.html");
    }

    public void SetLevel(int n)
    {
        level = n;
    }

    public void weapon(int n)
    {
        naww = n;
    }

    public void Decidew(int n)
    {
        if(n == 1)
        {
            mainw = naww;
        }
        else if(n == 2)
        {
            subw = naww;
        }

        if(mainw != 0 && subw !=0)
        {
            Really.SetActive(true);
        }
    }
    public void WithFight(int n)
    {
        if(n == 1)
        {
            withFight = true;
        }
        else if(n == 2)
        {
            withFight = false;
        }
    }

    public void DieEnemy()
    {
        EventNumber = 6;
        ENDPanel.SetActive(true);
    }
}
