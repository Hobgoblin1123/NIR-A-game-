using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Receiver : MonoBehaviour
{
    [SerializeField]
    private int nextNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {

        if(other.CompareTag("Player") == false)return;

        GetComponentInParent<Admin_tutorial>().Next(nextNumber);
        Destroy(this.gameObject);
    }
}