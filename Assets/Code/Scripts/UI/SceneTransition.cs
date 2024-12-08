using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public void Transition(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        // В редакторе Unity остановить игру
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // В запущенной игре завершить приложение
        Application.Quit();
#endif
    }
}