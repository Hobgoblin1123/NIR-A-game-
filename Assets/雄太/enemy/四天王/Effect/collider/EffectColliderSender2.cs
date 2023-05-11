using UnityEngine;

public class EffectColliderSender2 : MonoBehaviour
{
    //エフェクトによる当たり判定を行うとき、エフェクトが動く場合は子オブジェクトにする当たり判定にこれのスクリプトをつけること
    //そうしなければ、敵にダメージを与えることができません
    private Admin_EnemyEffect admin_Effect;
    // Start is called before the first frame update
    void Start()
    {
        admin_Effect = GetComponentInParent<Admin_EnemyEffect>();
        Debug.Log(admin_Effect);
    }

    // 当たり判定をadmin_effectへ転送する関数
    private void OnTriggerEnter(Collider other) {
        admin_Effect.OnTriggerEnter(other);
    }
    private void OnTriggerStay(Collider other) {
        admin_Effect.OnTriggerStay(other);
    }
}
