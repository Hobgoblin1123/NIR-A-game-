using UnityEngine;

public class Eff_DamageArea : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float DestroyCoUnt;
    private void Start() 
    {
        animator.SetTrigger("Effect");
        Debug.Log("よんだぁ");
        Destroy(gameObject , DestroyCoUnt);

    }
}