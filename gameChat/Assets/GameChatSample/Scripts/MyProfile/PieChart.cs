using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PieChart : MonoBehaviour
{
    public ListProfileKewordManager manager;
    public Image[] imagesPieChart;
    public float[] values;

    // Start is called before the first frame update
    void Start()
    {
        values[0] = manager.ratioList[0];
        values[1] = manager.ratioList[1];
        values[2] = manager.ratioList[2];
        values[3] = manager.ratioList[3];
        values[4] = manager.ratioList[4];

        SetValues(values);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetValues(float[] valuesToSet)
    {
        float totalValues = 0;
        for (int i = 0; i < imagesPieChart.Length; i++)
        {
            totalValues += FindPercentage(valuesToSet, i);
            //float[] percentValue[i] = FindPercentage(valuesToSet, i);
            imagesPieChart[i].fillAmount = totalValues;
        }
    }

    private float FindPercentage(float[] valueToSet, int index)
    {
        float totalAmount = 0;
        for (int i = 0; i < valueToSet.Length; i++)
        {
            totalAmount += valueToSet[i];
        }

        return valueToSet[index] / totalAmount;
    }
}
