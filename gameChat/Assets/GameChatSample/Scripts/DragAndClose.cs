using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndClose : MonoBehaviour
{

    float distance = 10;

    void OnMouseDrag()
    {
        print("Drag!!");
        Vector3 mousePosition = new Vector3(Input.mousePosition.x,
        Input.mousePosition.y, distance);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        transform.position = objPosition;
        //transform.position=objPosition2;
    }
}
