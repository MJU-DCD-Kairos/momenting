using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string SceneName;

    public void changeScene()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneName);
        }
    }
}
