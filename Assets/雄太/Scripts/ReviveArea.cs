using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveArea : MonoBehaviour
{
    private Admin_Auto admin_Auto;
    [SerializeField]
    private float revivePoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        admin_Auto = GetComponentInParent<Admin_Auto>();
    }

    // Update is called once per frame
    void Update()
    {
        if(revivePoint > 0)
        {
            revivePoint -= 0.07f*Time.deltaTime;
            admin_Auto.hpSlider.value = revivePoint;
        }
        if(revivePoint >= 1)
        {
            admin_Auto.Revive();
            Destroy(this.gameObject);
            //復活エフェクトをつける
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if(!collider.CompareTag("Player"))return;
        revivePoint += 0.2f*Time.deltaTime;
    }

}
