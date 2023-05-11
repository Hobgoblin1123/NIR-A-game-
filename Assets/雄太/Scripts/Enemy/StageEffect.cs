using System.Collections;
using UnityEngine;
using Effekseer;
using DG.Tweening;

public class StageEffect : MonoBehaviour
{
    
    [SerializeField , Tooltip("連続ダメージの発生間隔")]
    private float DamageInterval = 0.5f;
    [SerializeField]
    private float damage = 10;
    private float time1;
    private int count;
    [SerializeField]
    private float colliderOffCount;
    private Collider Collider;
    private void Start() {
        Collider = GetComponent<Collider>();
    }


    void OnTriggerStay(Collider collider)
    {
        
        if(collider.CompareTag("Player"))
        {
            time1 += Time.deltaTime;
            
            if(time1 > DamageInterval)
            {
                collider.GetComponent<Move>().TakeDamage(damage);
                time1 = 0;
                count += 1;
                Debug.Log("連続攻撃");
                if(count > colliderOffCount)
                {
                    collider.enabled = false;
                    count = 0;
                    StartCoroutine("ColOn");
                }
            }
        }
    }

    IEnumerator ColOn()
    {
        yield return new WaitForSeconds(3f);
        Collider.enabled = true;
    }


}
