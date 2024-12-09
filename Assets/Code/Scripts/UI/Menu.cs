using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;

    private void OnGUI()
    {
        if (currencyUI != null)
        {
            currencyUI.text = LevelManager.main.currency.ToString();
        }
    }
}



