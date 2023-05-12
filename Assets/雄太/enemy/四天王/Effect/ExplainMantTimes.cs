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
    private Collider[] col;
    private void Start() {
        foreach (var item in col)
        {
            item.enabled = false;
        }
        StartCoroutine("EFFON");
    }


    IEnumerator EFFON()
    {
        particleSystems[9].Play();
        particleSystems[12].Play();
        particleSystems[11].Play();
        particleSystems[10].Play();
        yield return new WaitForSeconds(0.2f);
        effekseers[9].Play();
        effekseers[12].Play();
        effekseers[11].Play();
        effekseers[10].Play();
        col[12].enabled = true;
        col[11].enabled = true;
        col[10].enabled = true;
        col[9].enabled = true;
        yield return new WaitForSeconds(0.1f);
        particleSystems[5].Play();
        particleSystems[8].Play();
        particleSystems[7].Play();
        particleSystems[6].Play();
        yield return new WaitForSeconds(0.2f);
        effekseers[5].Play();
        effekseers[8].Play();
        effekseers[7].Play();
        effekseers[6].Play();
        col[8].enabled = true;
        col[7].enabled = true;
        col[6].enabled = true;
        col[5].enabled = true;
        yield return new WaitForSeconds(0.1f);
        particleSystems[4].Play();
        particleSystems[3].Play();
        particleSystems[2].Play();
        particleSystems[1].Play();
        yield return new WaitForSeconds(0.2f);
        effekseers[4].Play();
        effekseers[3].Play();
        effekseers[2].Play();
        effekseers[1].Play();
        col[4].enabled = true;
        col[3].enabled = true;
        col[2].enabled = true;
        col[1].enabled = true;
        particleSystems[0].Play();
        yield return new WaitForSeconds(0.2f);
        effekseers[0].Play();
        col[0].enabled = true;


        yield return new WaitForSeconds(1.4f);

        Destroy(this.gameObject);
    }
}
