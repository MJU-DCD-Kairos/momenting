using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] RectTransform uiHandleRectTransform;
    [SerializeField] Color backgroundActiveColor;
    [SerializeField] Color handleActiveColor;
    private bool tapcheck;
    Image backgroundImage, handleImage;
    public GameObject a,b,c,d,e,f,g;
    Color backgroundDefaultColor, handleDefaultColor;

    Toggle toggle;

    Vector2 handlePosition;
    public GameObject main;

    public void Switch()
    {
        bool isActive = main.activeSelf;

        main.SetActive(!isActive);
        a.SetActive(!isActive);
        b.SetActive(!isActive);
        c.SetActive(!isActive);
        d.SetActive(!isActive);
        e.SetActive(!isActive);
        f.SetActive(!isActive);
        g.SetActive(!isActive);
    }
    void Awake()
    {
        
        toggle = GetComponent<Toggle>();

        handlePosition = uiHandleRectTransform.anchoredPosition;

        backgroundImage = uiHandleRectTransform.parent.GetComponent<Image>();
        handleImage = uiHandleRectTransform.GetComponent<Image>();

        backgroundDefaultColor = backgroundImage.color;
        handleDefaultColor = handleImage.color;

        toggle.onValueChanged.AddListener(OnSwitch);

        if (toggle.isOn)
            
            OnSwitch(true);
    }

    void OnSwitch(bool on)
    {
        //uiHandleRectTransform.anchoredPosition = on ? handlePosition * -1 : handlePosition ; // no anim
        uiHandleRectTransform.DOAnchorPos(on ? handlePosition * -1 : handlePosition, .3f);

        //backgroundImage.color = on ? backgroundActiveColor : backgroundDefaultColor ; // no anim
        backgroundImage.DOColor(on ? backgroundActiveColor : backgroundDefaultColor, .6f);

        //handleImage.color = on ? handleActiveColor : handleDefaultColor ; // no anim
        handleImage.DOColor(on ? handleActiveColor : handleDefaultColor, .4f);
    }
    
   

    void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}