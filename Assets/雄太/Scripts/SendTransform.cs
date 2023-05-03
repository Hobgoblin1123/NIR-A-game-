using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendTransform : MonoBehaviour
{
    [SerializeField]
    private Admin_Auto admin_Auto;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        admin_Auto.enemyTransform = this.transform;
    }
}
