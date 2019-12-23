using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMaster : MonoBehaviour
{

    public Unit selectedUnit;

    public int playerTurn = 1;
    private int totalTurns = 1;

    public GameObject selectedUnitSquare;

    public Image playerIndicator;
    public TextMeshProUGUI playerName;
    public Sprite player1Indicator;
    public Sprite player2Indicator;

    public int player1Gold = 100;
    public int player2Gold = 100;

    public TextMeshProUGUI player1GoldText;
    public TextMeshProUGUI player2GoldText;

    public BarrackItem purchasedItem;

    public GameObject statsPanel;
    public Vector2 statsPanelShift;
    public Unit viewedUnit;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI movementtext;
    public TextMeshProUGUI rangeText;


    public void ToggleStatsPanel(Unit unit)
    {
        if (unit.Equals(viewedUnit) == false)
        {
            statsPanel.SetActive(true);
            statsPanel.transform.position = (Vector2)unit.transform.position + statsPanelShift;
            viewedUnit = unit;
            UpdateStatsPanel();
        }
        else
        {
            statsPanel.SetActive(false);
            viewedUnit = null;
        }
    }

    public void UpdateStatsPanel()
    {
        if (viewedUnit != null)
        {
            healthText.text = viewedUnit.health.ToString();
            attackText.text = viewedUnit.attack.ToString();
            defenseText.text = viewedUnit.defense.ToString();
            movementtext.text = viewedUnit.movement.ToString();
            rangeText.text = viewedUnit.attackRange.ToString();
        }
    }

    public void MoveStatsPanel(Unit unit)
    {
        if (unit.Equals(viewedUnit))
        {
            statsPanel.transform.position = (Vector2)unit.transform.position + statsPanelShift;
        }
    }

    public void RemoveStatsPanel(Unit unit)
    {
        if (unit.Equals(viewedUnit))
        {
            statsPanel.SetActive(false);
            viewedUnit = null;
        }
    }

    public void UpdateGoldText()
    {
        player1GoldText.text = player1Gold.ToString();
        player2GoldText.text = player2Gold.ToString();
    }

    void GetGoldIncome(int currentPlayerTurn)
    {
        foreach (Fortress fortress in FindObjectsOfType<Fortress>())
        {
            if (fortress.playerNumber == currentPlayerTurn)
            {
                if (currentPlayerTurn == 1)
                {
                    player1Gold += fortress.goldPerTurn;
                }
                else
                {
                    player2Gold += fortress.goldPerTurn;
                }
            }
        }
        UpdateGoldText();
    }

    public void ResetTiles()
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            tile.Reset();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndTurn();
        }

        if (selectedUnit != null)
        {
            selectedUnitSquare.SetActive(true);
            selectedUnitSquare.transform.position = selectedUnit.transform.position;
        }
        else
        {
            selectedUnitSquare.SetActive(false);
        }
    }

    public void EndTurn()
    {
        totalTurns++;
        if (playerTurn == 1)
        {
            playerTurn = 2;
            playerIndicator.sprite = player2Indicator;
            playerName.text = "Player 2";
        }
        else if (playerTurn == 2)
        {
            playerTurn = 1;
            playerIndicator.sprite = player1Indicator;
            playerName.text = "Player 1";
        }

        if (totalTurns > 2)
        {
            GetGoldIncome(playerTurn);
        }
        

        if (selectedUnit != null)
        {
            selectedUnit.selected = false;
            selectedUnit = null;
        }

        ResetTiles();

        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            unit.hasMoved = false;
            unit.weaponIcon.SetActive(false);
            unit.hasAttacked = false;
            foreach (SpriteRenderer sr in unit.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = Color.white;
            }
        }

        GetComponent<Barrack>().CloseMenu();
    }
}
