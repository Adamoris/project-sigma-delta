using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool selected;
    GameMaster gm;

    public enum UnitType { None, Sword, Axe, Bow}
    public UnitType unitType;

    public int movement;
    public bool hasMoved;

    public float moveSpeed;

    public int playerNumber;

    public int attackRange;
    List<Unit> enemiesInRange = new List<Unit>();
    public bool hasAttacked;

    public GameObject weaponIcon;

    public int health;
    public int attack;
    public int defense;

    private void Start()
    {
        gm = FindObjectOfType<GameMaster>();
    }

    private void OnMouseDown()
    {
        ResetWeaponIcons();

        if (selected == true)
        {
            selected = false;
            gm.selectedUnit = null;
            gm.ResetTiles();
        }
        else
        {
            if (playerNumber == gm.playerTurn)
            {
                if (gm.selectedUnit != null)
                {
                    gm.selectedUnit.selected = false;
                }

                selected = true;
                gm.selectedUnit = this;

                gm.ResetTiles();
                GetEnemies();
                GetWalkableTiles();
            }
        }

        Collider2D col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.15f);
        Unit unit = col.GetComponent<Unit>();
        if (gm.selectedUnit != null)
        {
            if (gm.selectedUnit.enemiesInRange.Contains(unit) && gm.selectedUnit.hasAttacked == false)
            {
                gm.selectedUnit.Attack(unit);
            }
        }

    }

    void Attack(Unit target)
    {
        hasAttacked = true;

        int damageDealt = attack - target.defense;
        int damageReceived = target.attack - defense;

        if (damageDealt >= 1)
        {
            target.health -= damageDealt;
        }
        if (damageReceived >= 1)
        {
            health -= damageReceived;
        }

        if (target.health <= 0)
        {
            Destroy(target.gameObject);
            GetWalkableTiles();
        }
        if (health <= 0)
        {
            gm.ResetTiles();
            Destroy(gameObject);
        }
    }

    void GetWalkableTiles()
    {
        if (hasMoved == true)
        {
            return;
        }

        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            if (Mathf.Abs(transform.position.x - tile.transform.position.x) + Mathf.Abs(transform.position.y - tile.transform.position.y) <= movement)
            {
                if (tile.IsClear(unitType) == true)
                {
                    tile.Highlight();
                }
            }
        }
    }

    void GetEnemies()
    {
        enemiesInRange.Clear();

        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if (Mathf.Abs(transform.position.x - unit.transform.position.x) + Mathf.Abs(transform.position.y - unit.transform.position.y) <= attackRange)
            {
                if (unit.playerNumber != gm.playerTurn && hasAttacked == false)
                {
                    enemiesInRange.Add(unit);
                    unit.weaponIcon.SetActive(true);
                }
            }
        }
    }

    public void ResetWeaponIcons()
    {
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            unit.weaponIcon.SetActive(false);
        }
    }

    public void Move(Vector2 tilePos)
    {
        gm.ResetTiles();
        StartCoroutine(StartMovement(tilePos));
    }

    IEnumerator StartMovement(Vector2 tilePos)
    {
        while (transform.position.x != tilePos.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(tilePos.x, transform.position.y), moveSpeed * Time.deltaTime);
            yield return null;
        }

        while (transform.position.y != tilePos.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, tilePos.y), moveSpeed * Time.deltaTime);
            yield return null;
        }

        hasMoved = true;
        ResetWeaponIcons();
        GetEnemies();
    }
}
