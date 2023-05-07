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
    // Start is called before the first frame update
    void Start()
    {
        PlayTimeLine(0);
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
                stateNumber = 5;
                StartCoroutine("DelayNext");
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
            move.enabled = true;
        }
        else if(n == 3)
        {
            stateNumber = 4;
        }
        else if(n == 4)
        {
            stateNumber = 6;
        }
    }

    IEnumerable DelayNext()
    {
        yield return new WaitForSecondsRealtime(4);
        PlayTimeLine(3);
        Destroy(statusB.transform.parent.gameObject);
    }
}
