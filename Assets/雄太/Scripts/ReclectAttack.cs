using UnityEngine;
using System.Collections;


public class ReclectAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider) {
        if(collider.tag == "Enemy" )
        {
            Debug.Log(collider + "に" + Admin.AttackStatus*Move.reflectRate + "だめーじ");
            collider.GetComponent<Admin_EnemyStatus>().TakeDamage(Admin.AttackStatus*Move.reflectRate);//相手にダメージを与えるよ
        }
    }
}
