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
        // � ��������� Unity ���������� ����
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // � ���������� ���� ��������� ����������
        Application.Quit();
#endif
    }
}