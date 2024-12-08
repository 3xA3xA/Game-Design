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

    //private void OnMouseDown()
    //{
    //    if (towerObj != null) return;

    //    Tower towerToBuild = BuildManager.main.GetSelectedTower();


    //    if (towerToBuild.cost > LevelManager.main.currency)
    //    {
    //        return;
    //    }

    //    LevelManager.main.SpendCurrency(towerToBuild.cost);

    //    towerObj = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
    //    turret = towerObj.GetComponent<Turret>();
    //}

    private void OnMouseDown()
    {
        if (towerObj != null) return;

        Tower towerToBuild = BuildManager.main.GetSelectedTower();

        if (towerToBuild.cost > LevelManager.main.currency)
        {
            return;
        }

        LevelManager.main.SpendCurrency(towerToBuild.cost);

        // ѕолучаем индекс выбранной башни
        int towerIndex = Array.IndexOf(BuildManager.main.GetTowers(), towerToBuild);

        // «адаем смещение в зависимости от индекса башни
        float yOffset = 0f;
        if (towerIndex == 1)
        {
            yOffset = 70f / 100f; // 20 пикселей
        }
        else if (towerIndex == 2)
        {
            yOffset = 80f / 100f; // 40 пикселей
        }

        // —оздаем новую позицию с учетом смещени€ по оси Y
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);

        towerObj = Instantiate(towerToBuild.prefab, newPosition, Quaternion.identity);
        turret = towerObj.GetComponent<Turret>();
    }
}
