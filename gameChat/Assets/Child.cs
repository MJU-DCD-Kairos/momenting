using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = GameObject.Find("Userslot").transform;   // 부모-자식 관계 설정



        transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);            // localScale은 부모의 크기에 대한 비를 나타낸다(1:동일크기, 2: 2배크기)



        float x = transform.localScale.x;

        float y = transform.localScale.y;

        float z = transform.localScale.z;




    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
