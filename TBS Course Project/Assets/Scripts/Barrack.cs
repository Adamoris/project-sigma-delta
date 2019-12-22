using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barrack : MonoBehaviour
{
    public GameObject openButton;
    public GameObject closeButton;

    public GameObject barracksMenu;

    GameMaster gm;

    private void Start()
    {
        gm = GetComponent<GameMaster>();
    }

    public void ToggleMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf);
        openButton.SetActive(!openButton.activeSelf);
        closeButton.SetActive(!closeButton.activeSelf);
    }

    public void CloseMenu()
    {
        barracksMenu.SetActive(false);
        openButton.SetActive(true);
        closeButton.SetActive(false);
    }

    public void BuyItem(BarrackItem item)
    {
        if (gm.playerTurn == 1 && item.cost <= gm.player1Gold)
        {
            gm.player1Gold -= item.cost;
            ToggleMenu(barracksMenu);
        }
        else if (gm.playerTurn == 2 && item.cost <= gm.player2Gold)
        {
            gm.player2Gold -= item.cost;
            ToggleMenu(barracksMenu);
        }
        else
        {
            print("Not enough minerals.");
            return;
        }

        gm.UpdateGoldText();

        gm.purchasedItem = item;

        if (gm.selectedUnit != null)
        {
            gm.selectedUnit.selected = false;
            gm.selectedUnit = null;
        }

        GetCreatableTiles();
    }

    void GetCreatableTiles()
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            if (tile.IsClear())
            {
                tile.SetCreatable();
            }
        }
    }
}
