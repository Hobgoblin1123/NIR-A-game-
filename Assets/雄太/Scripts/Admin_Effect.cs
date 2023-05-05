using UnityEngine;
using Effekseer;

public class Admin_Effect : MonoBehaviour
{
    [SerializeField]
    private bool isDamageEffect = true;          //敵にダメージを与えることができるエフェクトなのか定義

    [SerializeField , Header("ダメージ倍率")]
    private float DamageMagnification = 1;        //攻撃力に対してどれくらいの倍率をかけるか定義

    [Space(5)]

    [SerializeField , Tooltip("連続ダメージを発生させるかどうか")]
    private bool ContinueDamage = false;          //連続ダメージを発生させるか定義
    
    [SerializeField , Tooltip("連続ダメージの発生間隔")]
    private float DamageInterval = 0.2f;          //連続ダメージの間隔を定義
    public FirstTransForm[] firstTransForm;       //エフェクトが発生する位置情報を配列化　複数のパターンを保存可能
    [SerializeField]
    private float time;                           //連続攻撃時間を定義
    // [SerializeField]
    private EffekseerEmitter effekseerEmitter;    //アタッチ
    


    // 位置情報にposition,rotation,scaleを設定する
    [System.Serializable]
    public struct FirstTransForm
    {
        public Vector3 position;
        public Vector3 Addrotation;
        public Vector3 scale;
    }

    void Awake()
    {
        effekseerEmitter = GetComponent<EffekseerEmitter>();//取得
        if(isDamageEffect == false)
        {
            Destroy(GetComponent<Collider>());//ダメージエフェクト出ないなら当たり判定消す
        }
    }

    //エフェクトを再生するときに呼ばれる関数
    public void EffectStart(int n)
    {
        var parent = GetComponentInParent<Transform>();//親の位置を取得
        transform.rotation = transform.parent.rotation;//位置をリセット
        transform.localPosition = firstTransForm[n].position;//ローカルポジションを変更
        transform.localScale = firstTransForm[n].scale;//スケール変更
        transform.Rotate(firstTransForm[n].Addrotation);//回転
        time = DamageInterval;
        effekseerEmitter.Play();//エフェクトを再生
    }

    public void EffectEnd()
    {
        effekseerEmitter.Stop();//エフェクトを停止
    }

    // ダメージエフェクトで範囲内に敵がいる場合に実行される関数
    void OnTriggerEnter(Collider collider)
    {
        if(ContinueDamage == true && !collider.CompareTag("Enemy"))return;//敵じゃないなら実行しない

        var damage = Admin.AttackStatus * DamageMagnification;//ダメージ量を計算
        Debug.Log(collider + "に" + damage + "だめーじ");
        collider.GetComponent<Admin_EnemyStatus>().TakeDamage(damage);//相手にダメージを与えるよ
    }

    //ダメージエフェクトで連続ダメージがオンで範囲内に敵がいるときに実行される関数
    void OnTriggerStay(Collider collider)
    {
        if(ContinueDamage == false && !collider.CompareTag("Enemy"))return;//敵じゃなきゃだめ

        time += Time.deltaTime;//時間を増やすよ
        if(time > DamageInterval)//時間になったら攻撃をするよ
        {
            var damage = Admin.AttackStatus*DamageMagnification;//ダメージ量を計算
            collider.GetComponent<Admin_EnemyStatus>().TakeDamage(damage);//相手にダメージを与えるよ
            time = 0;//時間をリセットするよ
        }
    }
}
