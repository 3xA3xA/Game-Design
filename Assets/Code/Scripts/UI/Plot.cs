using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject towerObj;
    private int currentTowerIndex = -1; // Добавляем объявление переменной currentTowerIndex
    public Turret turret;
    private Color startColor;

    private void Start()
    {
        startColor = sr.color;
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        int newTowerIndex = Array.IndexOf(BuildManager.main.GetTowers(), towerToBuild);

        if (towerObj != null)
        {
            // Проверка, что новая башня будет уровнем выше текущей
            if (newTowerIndex > currentTowerIndex)
            {
                if (towerToBuild.cost > LevelManager.main.currency) { return; }

                // Удалить старую башню и создать новую башню уровнем выше
                Destroy(towerObj);
                LevelManager.main.SpendCurrency(towerToBuild.cost);

                // Создать новую башню
                PlaceNewTower(towerToBuild, newTowerIndex);
            }
            return;
        }

        if (towerToBuild.cost > LevelManager.main.currency) { return; }

        LevelManager.main.SpendCurrency(towerToBuild.cost);
        PlaceNewTower(towerToBuild, newTowerIndex);
    }

    private void PlaceNewTower(Tower towerToBuild, int towerIndex)
    {
        // Задаем смещение в зависимости от индекса башни
        float yOffset = 0f;
        if (towerIndex == 1) { yOffset = 70f / 100f; }
        else if (towerIndex == 2) { yOffset = 80f / 100f; }

        // Создаем новую позицию с учетом смещения по оси Y
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);

        towerObj = Instantiate(towerToBuild.prefab, newPosition, Quaternion.identity);
        currentTowerIndex = towerIndex; // Обновляем текущий индекс башни
        turret = towerObj.GetComponent<Turret>();
    }
}