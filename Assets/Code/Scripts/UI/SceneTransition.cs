using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public void Transition(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
