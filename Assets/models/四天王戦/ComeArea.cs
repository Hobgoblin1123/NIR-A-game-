using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComeArea : MonoBehaviour
{
    [SerializeField]
    private Admin_Event admin_Event;
    [SerializeField]
    private int NextEvent = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        admin_Event.NextEvent(NextEvent);
    }
    
        
}