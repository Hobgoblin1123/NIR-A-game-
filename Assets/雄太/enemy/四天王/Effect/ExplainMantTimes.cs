using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class ExplainMantTimes : MonoBehaviour
{
    [SerializeField]
    private EffekseerEmitter[] effekseers;
    [SerializeField]
    private ParticleSystem[] particleSystems;
    [SerializeField]
    private Collider[] colliders;
    [SerializeField]
    private float colliderOnTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EffectOn(Vector3 posi)
    {
        transform.position = posi;
        foreach (var item in effekseers)
        {
            item.Play();
        }
        foreach (var item in particleSystems)
        {
            item.Play();
        }
        if(colliderOnTime <= 0)
        {
            foreach (var item in colliders)
            {
                item.enabled = false;
            }
        }
        else
        {
            foreach (var item in colliders)
            {
                item.enabled = true;
                StartCoroutine("OnColl");
            }
        }
    }

    public void EffectOff()
    {
         foreach (var item in effekseers)
        {
            item.Stop();
        }
        foreach (var item in particleSystems)
        {
            item.Stop();
        }
        if(colliderOnTime <= 0)
        {
            foreach (var item in colliders)
            {
                item.enabled = false;
            }
        }
    }

    IEnumerator OnColl()
    {
        yield return new WaitForSeconds(colliderOnTime);
        foreach (var item in colliders)
        {
            item.enabled = true;
        }
    }
}
