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
    
    // Start is called before the first frame update
    void Start()
    {
        director.Play(timeline[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextEvent(int n)
    {
        EventNumber = n;
        if(n == 1)
        {
            director.Play(timeline[1]);
        }
    }

    public void OpneWebPage()
    {
        Application.OpenURL("https://nir-a.sakura.ne.jp/contact.html");
    }
}
