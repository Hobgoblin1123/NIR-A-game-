using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Admin_Chest : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private ParticleSystem particleSystem;
    [SerializeField]
    private Item[] item;

    const float OUT_ITEM_WAIT_TIME = 0.5f;
    private void Start() {
        animator = GetComponent<Animator>();
    }
    public void Open()
    {
        animator.SetTrigger("Open");
        particleSystem.Play();
        StartCoroutine("OutItem");

        
    }

    IEnumerator OutItem()
    {
        yield return new WaitForSeconds(OUT_ITEM_WAIT_TIME);
        foreach (var item in item)
        {
            var o = Instantiate(item , transform.position , transform.rotation , this.transform);
            o.transform.DOLocalMove(new Vector3(Random.Range(-0.5f , 0.5f) , 0 , Random.Range(0.3f , 1)) , 02f).SetEase(Ease.OutCubic);
        }
    }
}
