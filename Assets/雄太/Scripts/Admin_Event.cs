using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

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
    private GameObject Really;
    public int level;
    public int mainw;
    public int subw;
    private int naww;
    public bool withFight;
    [SerializeField]
    private GameObject HIROIN;
    
    // Start is called before the first frame update
    void Start()
    {
        director.Play(timeline[0]);
        move.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextEvent(int n)
    {
        EventNumber = n;
        if(n == 0)
        {
            move.enabled = true;
        }
        else if(n == 1)
        {
            director.Play(timeline[1]);
            
            move.SetState(Move.MyState.Normal);
            move.enabled = false;
        }
        if(n == 2)
        {
            director.Pause();
            
            move.SetState(Move.MyState.Normal);
            move.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if(n == 3)
        {
            director.Resume();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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
            move.transform.position = new Vector3(-25,0.4f,41);
            move.transform.eulerAngles = new Vector3(0,180,0);
            if(withFight == true)
            {
                HIROIN.SetActive(true);
            }
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
}
