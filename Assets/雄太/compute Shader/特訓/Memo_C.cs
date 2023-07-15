using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class Memo_C : MonoBehaviour
{
    [SerializeField]
    private ComputeShader computeShader;
    [SerializeField]
    private Transform moveObj;

    private ComputeBuffer buffer;
    private Vector3 center = Vector3.zero;

    public int m =1;
    // Start is called before the first frame update
    void Start()
    {
        buffer = new ComputeBuffer(1,Marshal.SizeOf(typeof(Vector2)));
        computeShader.SetBuffer(computeShader.FindKernel("CSMain") ,"ResultBuffer" , buffer);

    }

    // Update is called once per frame
    void Update()
    {
        computeShader.SetFloats("position" , center.x , center.y);
        computeShader.SetFloat("time" , Time.time);
        
        computeShader.Dispatch(0,m,1,1);


        var date = new float[2];
        buffer.GetData(date);

        Vector2 pos = moveObj.transform.localPosition;
        pos.x = date[0];
        pos.y = date[1];

        moveObj.transform.localPosition = pos;
    }

    private void OnDestroy() 
    {
        buffer.Release();
    }
}
