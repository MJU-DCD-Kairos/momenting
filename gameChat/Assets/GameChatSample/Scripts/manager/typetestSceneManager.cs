using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class typetestSceneManager : MonoBehaviour
{
    //��ũ��Ʈ �޾ƿ������� Ÿ�� ���� ����
    gameSceneManager gSM;
    public Button backToProfile_normal;
    public Button backToProfile_result;

    // Start is called before the first frame update
    void Start()
    {
        //don't destroy�� ����� �Ѿ�� ���Ӿ��Ŵ����� ��ũ��Ʈ�� ������ ����
        gSM = GameObject.Find("GameSceneManager").GetComponent<gameSceneManager>();

        //��ư�� gSM�� �ε�� �Լ� �����ʸ� �߰���
        backToProfile_normal.onClick.AddListener(gSM.LoadScene_MyProfile_Sample3);
        backToProfile_result.onClick.AddListener(gSM.LoadScene_MyProfile_Sample3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
