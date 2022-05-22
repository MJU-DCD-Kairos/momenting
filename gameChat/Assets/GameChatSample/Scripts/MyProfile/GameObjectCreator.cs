using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectCreator : MonoBehaviour
{
    public Button ListBtnKewordAdd;
    private GameObject prefab;
    private void Awake()
    {
        //ÇÁ¸®ÆÕ·Îµå
        var fileName = "List_KwEdit";
        this.prefab = Resources.Load<GameObject>(fileName);
    }
    // Start is called before the first frame update
    void Start()
    {
        this.ListBtnKewordAdd.onClick.AddListener(OnClickListBtnKewordAdd);
    }


    private void OnClickListBtnKewordAdd()
    {
        //Shell

        //Model
        this.CreateModel();
        //Info
    }

    private void CreateModel()
    {
        Instantiate<GameObject>(this.prefab);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
