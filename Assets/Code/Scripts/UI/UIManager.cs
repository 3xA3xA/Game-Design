using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI waveCountUI;


    public static UIManager main;
    private bool isHoveringUI;

    private void Awake()
    {
        main = this;
    }
    public void SetHoveringState(bool state)
    {
        isHoveringUI = state;
    }
    public bool IsHoveringUI()
    {
        return isHoveringUI;
    }

    private void OnGUI()
    {
        waveCountUI.text = EnemySpawner.main.currentWave.ToString() + " волна.";
    }
}