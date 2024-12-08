using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Tower[] towers;
    [SerializeField] private Button[] towerButtons; // Добавим ссылки на кнопки

    private int selectedTower = 0;

    private void Awake()
    {
        main = this;
        UpdateButtonHighlights();
    }

    public Tower GetSelectedTower()
    {
        return towers[selectedTower];
    }

    public Tower[] GetTowers()
    {
        return towers;
    }

    public void SetSelectedTower(int _selectedTower)
    {
        selectedTower = _selectedTower;
        UpdateButtonHighlights(); // Обновляем подсветку кнопок
    }

    private void UpdateButtonHighlights()
    {
        for (int i = 0; i < towerButtons.Length; i++)
        {
            ColorBlock colors = towerButtons[i].colors;

            if (i == selectedTower)
            {
                // Устанавливаем цвет для выбранной кнопки
                colors.normalColor = Color.yellow;
                colors.highlightedColor = Color.yellow;
                colors.pressedColor = Color.yellow;
                colors.selectedColor = Color.yellow;
            }
            else
            {
                // Устанавливаем цвет для невыбранных кнопок
                colors.normalColor = Color.white;
                colors.highlightedColor = Color.white;
                colors.pressedColor = Color.white;
                colors.selectedColor = Color.white;
            }

            towerButtons[i].colors = colors;
        }
    }
}
