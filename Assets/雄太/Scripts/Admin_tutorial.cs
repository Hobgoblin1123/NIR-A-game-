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
    // Start is called before the first frame update
    void Start()
    {
        PlayTimeLine(0);
        move.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayTimeLine(int n)
    {
        director.Play(timeline[n]);
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
    }
}
