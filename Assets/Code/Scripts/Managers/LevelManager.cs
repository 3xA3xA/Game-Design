using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    public Transform startPoint;
    public Transform[] path1;
    public Transform[] path2;

    public int currency;

    private void Awake()
    {
        main = this;
    }

    public void IncreaseCurrency(int amount) 
    {
        currency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            // Buy Item
            currency -= amount;
            return true;
        }
        else
        {
            Debug.Log("No amount!");
            return false;
        }
    }
}
