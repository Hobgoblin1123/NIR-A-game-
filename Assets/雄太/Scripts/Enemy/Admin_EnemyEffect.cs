using System.Collections;
using UnityEngine;
using Effekseer;
using DG.Tweening;

public class Admin_EnemyEffect : MonoBehaviour
{
    [SerializeField]
    private bool isDamageEffect = true;
    [SerializeField]
    private bool canMoveCollider = false;        //コライダーが移動する処理をする場合はここにチェック
    [SerializeField]
    private ParticleSystem effect;               //ParticleSystemを使ってエフェクトを再生するときはアタッチする

    [SerializeField , Header("ダメージ倍率")]
    private float DamageMagnification = 1;

    [Space(5)]

    [SerializeField , Tooltip("連続ダメージを発生させるかどうか")]
    private bool ContinueDamage = false;
    
    [SerializeField , Tooltip("連続ダメージの発生間隔")]
    private float DamageInterval = 0.2f;
    public FirstTransForm[] firstTransForm;
    [SerializeField]
    private float time;
    private EffekseerEmitter effekseerEmitter;
    [SerializeField]
    private GameObject colliderObj;               //コライダーを動かく場合、動かすコライダーがついたprefabがアタッチされる
    [SerializeField]
    private Vector3 colliderMoveTransform = Vector3.zero;//　コライダーが移動する移動先のlocalPosition
    [SerializeField]
    private float colliderMoveTime = 0f;//コライダーが動く場合、移動する時間

    float damage;
    [SerializeField]
    private bool SPDamage;
    private bool emitterFlag = true;
    private Collider coll;
    [SerializeField]
    private float ColliderONTime = 0;
    private float ColliderOFFTime;
    [SerializeField]
    private bool SPEffect = false;
    [SerializeField]
    private Admin_EnemyEffect[] SPEffects;
    [SerializeField]
    private Admin_EnemyEffect[] SPEffects2;

    [System.Serializable]
    public struct FirstTransForm
    {
        public Vector3 position;
        public Vector3 Addrotation;
        public Vector3 scale;
    }

    void Awake()
    {
        if(SPEffect == true)return;
        if(GetComponent<EffekseerEmitter>())
        {
            effekseerEmitter = GetComponent<EffekseerEmitter>();
            emitterFlag = true;
        }
        else
        {
            emitterFlag = false;
        }
        if(isDamageEffect == false)
        {
            Destroy(GetComponent<Collider>());
        }
    }

    void Start()
    {
        if(SPEffect == true)return;
        damage = GetComponentInParent<Admin_EnemyStatus>().AttackStatus * DamageMagnification;
        coll = GetComponent<Collider>();
        if(ColliderONTime > 0)
        {
            coll.enabled = false;
            StartCoroutine(ColliderOn());
        }
    }

    IEnumerator ColliderOn()
    {
        var x = new WaitForSeconds(ColliderONTime);
        yield return x;
        coll.enabled = true;
    }
    
    public void EffectStart(int n)
    {
        if(SPEffect == true)
        {
            SPEffects[n].gameObject.SetActive(true);
            SPEffects[n].EffectStart(0);
            SPEffects2[n].gameObject.SetActive(true);
            SPEffects2[n].EffectStart(0);
            return;
        }

        var parent = GetComponentInParent<Transform>();
        transform.rotation = transform.parent.rotation;
        transform.localPosition = firstTransForm[n].position;
        transform.localScale = firstTransForm[n].scale;
        transform.Rotate(firstTransForm[n].Addrotation);
        time = DamageInterval;
        if(emitterFlag == true)
        {
            effekseerEmitter.Play();
        }
        else
        {
            effect.Play();
        }

        if(canMoveCollider == true)
        {
            var g = Instantiate(colliderObj,transform.position,transform.rotation,this.transform);
            g.transform.DOLocalMove(colliderMoveTransform , colliderMoveTime);
            Destroy(g,colliderMoveTime);
        }
    }

    public void EffectEnd()
    {
        if(SPEffect == true)
        {
            foreach (var item in SPEffects)
            {
                item.EffectEnd();
                item.gameObject.SetActive(false);
                return;
            }
            foreach (var item in SPEffects2)
            {
                item.EffectEnd();
                item.gameObject.SetActive(false);
                return;
            }
        }
        if(emitterFlag == true)
        {
            effekseerEmitter.Stop();
        }
        else
        {
            effect.Stop();
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if(ContinueDamage == true)return;

        if(collider.CompareTag("Player"))
        {
            var c = collider.GetComponent<Move>();
            if(SPDamage == false)
            {
                c.TakeDamage(damage);
                Debug.Log(collider + "に" + damage + "だめーじ");
            }
            else
            {
                var dame = c.charahp*0.9f;
                c.TakeDamage(dame);
                Debug.Log(("残りHPの9割のダメージを与えるよん"));
            }
            
        }
        if(collider.CompareTag("AutoChara"))
        {
            collider.GetComponent<Admin_Auto>().TakeDamage(damage);
        }
    }

    public void OnTriggerStay(Collider collider)
    {
        if(ContinueDamage == false)return;

        if(collider.CompareTag("Player"))
        {
            time += Time.deltaTime;
            
            if(time > DamageInterval)
            {
                collider.GetComponent<Move>().TakeDamage(damage);
                time = 0;
                Debug.Log("連続攻撃");
            }
        }
        if(collider.CompareTag("AutoChara"))
        {
            if(time > DamageInterval)
            {
                collider.GetComponent<Admin_Auto>().TakeDamage(damage);
            }
        }
    }
}
