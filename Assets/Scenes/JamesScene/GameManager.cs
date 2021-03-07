using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneIndices
{
    TitleScreen = 1,
    MenuTest = 2,
    ComputeTest = 3,
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        SceneManager.LoadSceneAsync((int)SceneIndices.TitleScreen, LoadSceneMode.Additive);
    }

    public void LoadGame()
    {
        SceneManager.UnloadSceneAsync((int)SceneIndices.TitleScreen);
        SceneManager.LoadSceneAsync((int)SceneIndices.ComputeTest, LoadSceneMode.Additive);
    }
}
