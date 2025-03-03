using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartSceneScript : MonoBehaviour
{
    public void SceneChange()
    {
        SceneManager.LoadScene("Scene_Demo");
    }
}
