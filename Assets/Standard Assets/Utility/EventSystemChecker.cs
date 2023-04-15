using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemChecker : MonoBehaviour
{
    //public GameObject eventSystem;

	// Use this for initialization
	void Awake ()
	{
	    if(!FindObjectOfType<EventSystem>())
        {
            #pragma warning disable CS0618 // 'StandaloneInputModule.forceModuleActive' is obsolete: 'forceModuleActive has been deprecated. There is no need to force the module awake as StandaloneInputModule works for all platforms'
           //Instantiate(eventSystem);
            GameObject obj = new GameObject("EventSystem");
            obj.AddComponent<EventSystem>();
            obj.AddComponent<StandaloneInputModule>().forceModuleActive  = true;
        }
	}
}
