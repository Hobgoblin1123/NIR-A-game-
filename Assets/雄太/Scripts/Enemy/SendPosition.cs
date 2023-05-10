using UnityEngine;

public class SendPosition : MonoBehaviour
{
    [SerializeField]
    private Admin_ForHeavenlyKing admin_ForHeavenlyKing;
    private void Start() {
        admin_ForHeavenlyKing.charaTransform = transform;
    }
    // Update is called once per frame
    void Update()
    {
        admin_ForHeavenlyKing.charaTransform = transform;
    }
}
