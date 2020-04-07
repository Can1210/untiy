using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Scene loadScene = SceneManager.GetActiveScene();
            //シーンの読み直し
            SceneManager.LoadScene(loadScene.name);
        }
    }
}
