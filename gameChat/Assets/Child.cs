using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = GameObject.Find("Userslot").transform;   // �θ�-�ڽ� ���� ����



        transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);            // localScale�� �θ��� ũ�⿡ ���� �� ��Ÿ����(1:����ũ��, 2: 2��ũ��)



        float x = transform.localScale.x;

        float y = transform.localScale.y;

        float z = transform.localScale.z;




    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
