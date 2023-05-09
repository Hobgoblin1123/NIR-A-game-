using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Admin_tutorial : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector director;
    [SerializeField]
    private TimelineAsset[] timeline;
    [SerializeField]
    private Move move;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Admin_Enemy_G admin_Enemy_G;
    [SerializeField]
    private Admin_EnemyStatus statusA;
    [SerializeField]
    private Admin_EnemyStatus statusB;
    [SerializeField]
    private int stateNumber;
    [SerializeField]
    private GameObject chara;
    // Start is called before the first frame update
    void Start()
    {
        PlayTimeLine(0);
        StartCoroutine("Off");
        
    }

    IEnumerator Off()
    {
        yield return new WaitForSeconds(0.4f);
        move.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(stateNumber == 1)
        {
            admin_Enemy_G.SetState(Admin_Enemy_G.EnemyState.Wait);
            admin_Enemy_G.invincibleFlag = true;
            if(statusA.HP <=0)
            {
                PlayTimeLine(2);
                stateNumber = 3;
                Destroy(statusA.gameObject);
            }
        }
        else if(stateNumber == 4)
        {
            if(move.state == Move.MyState.JUAttack)
            {
                Debug.Log("ジャスト回避はいりましたぁぁぁぁぁぁ");
                stateNumber = 5;
                Destroy(statusB.transform.parent.gameObject,3);
                PlayTimeLine(3);
            }
        }
        else if(stateNumber == 6)
        {

        }
    }
    public void PlayTimeLine(int n)
    {
        director.Play(timeline[n]);
        move.enabled = false;
        animator.SetFloat("2Speed",0);
    }

    public void Next(int n)
    {
        if(n == 0)
        {
            move.enabled = true;
        }
        else if(n == 1)
        {
            PlayTimeLine(1);
            move.enabled = false;
            animator.SetFloat("2Speed" , 0);
            move.gameObject.transform.eulerAngles = new Vector3(0 , 180 , 0);
        }
        else if(n == 2)
        {
            stateNumber = 1;
            statusA.HP = 20;
            move.enabled = true;
            Admin.AttackStatus = 10;
        }
        else if(n == 3)
        {
            stateNumber = 4;
            move.enabled = true;
        }
        else if(n == 4)
        {
            stateNumber = 6;
            move.enabled = true;
        }
    }

    IEnumerable DelayNext()
    {
        yield return new WaitForSecondsRealtime(4);
        PlayTimeLine(3);
        Destroy(statusB.transform.parent.gameObject);
    }

    public void SetCharaPosition(int n)
    {
        Debug.Log("そのにりり");
        if(n == 0)
        {
            chara.transform.position = new Vector3(1,3,-21);
            Debug.Log("キャラ移動");
        }
    }
}