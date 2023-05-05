using UnityEngine;

public class alarmUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform rectTransform;//自身のUIトランスフォームを取得
private void Start() {
    rectTransform.localPosition = new Vector3(0,0,0);//ローカルポジションを中央にする
    Destroy(rectTransform.gameObject,3);//３秒後に削除する
}}
