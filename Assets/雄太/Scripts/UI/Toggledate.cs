using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Toggledate : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private RectTransform handle;
    private Admin_UI admin_UI;
    [SerializeField]
    private int ThisToggleNumber;

    /// <summary>
    /// トグルの値
    /// </summary>
    public bool Value;
    
    
    private static readonly Color OFF_BG_COLOR = new Color(0.92f, 0.92f, 0.92f);
    private static readonly Color ON_BG_COLOR = new Color(0.2f, 0.84f, 0.3f);

    
    private void Start()
    {
        admin_UI = GetComponentInParent<Admin_UI>();
    }

    /// <summary>
    /// トグルのボタンアクションに設定しておく
    /// </summary>
    public void SwitchToggle()
    {
        Value = !Value;
        
        
        admin_UI.ChangeToggle(ThisToggleNumber);
        StartCoroutine("MoveHandle");
    }

    IEnumerator  MoveHandle()
    {
        var n = Value? 5.4f:-5.4f;
        

        for (int i = 0; i < 14; i++)
        {
            handle.position = handle.position + new Vector3(n ,0,0);
            yield return new WaitForSecondsRealtime(0.02f);
        }
        backgroundImage.color = Value? ON_BG_COLOR : OFF_BG_COLOR;
    }

    public void ToggleOn()
    {
        Value = true;
        handle.localPosition = new Vector3(55,0,0);
        backgroundImage.color = ON_BG_COLOR;
    }
}