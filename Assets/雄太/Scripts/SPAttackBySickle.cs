using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPAttackBySickle : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> enemy;
    [SerializeField]
    private Admin_Effect effect;
    [SerializeField]
    private float AttackStartTime = 2f;
    [SerializeField]
    private float EffectDestroyTime = 3f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Attack");
    }

    // Update is called once per frame
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(AttackStartTime);
        foreach (var item in enemy)
        {
            var g =  Instantiate(effect , item.transform.position , item.transform.rotation , this.transform);
            g.GetComponent<Admin_Effect>().EffectStart( -1);
            Destroy(g , EffectDestroyTime);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(enemy.Contains(other.gameObject) == false)
        {
            enemy.Add(other.gameObject);
        }
    }
}
