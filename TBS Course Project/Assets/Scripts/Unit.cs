﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Unit : MonoBehaviour
{
    public bool selected;
    GameMaster gm;

    public enum UnitType { None, Archer, Bruiser, Healer, King, Soldier }
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

    public DamageIndicator damageIndicator;
    public GameObject deathEffect;

    private Animator camAnim;
    private AudioSource source;
    public AudioClip selectedSound;
    public AudioClip attackSound;

    public Color unitInactive;

    public TextMeshProUGUI kingHealth;

    public GameObject victoryPanel;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        gm = FindObjectOfType<GameMaster>();
        camAnim = Camera.main.GetComponent<Animator>();
        UpdateKingHealth();
    }

    public void UpdateKingHealth()
    {
        if (unitType == UnitType.King)
        {
            kingHealth.text = health.ToString();
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            gm.ToggleStatsPanel(this);
        }
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

                source.clip = selectedSound;
                source.Play();

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
        camAnim.SetTrigger("shake");

        hasAttacked = true;
        hasMoved = true;

        int damageDealt = attack - target.defense;
        int damageReceived = target.attack - defense;

        if (damageDealt >= 1)
        {
            DamageIndicator instance = Instantiate(damageIndicator, target.transform.position, Quaternion.identity);
            instance.Setup(damageDealt);
            target.health -= damageDealt;
            target.UpdateKingHealth();
        }

        if (transform.tag == "Archer" && target.tag != "Archer")
        {
            if (Mathf.Abs(transform.position.x - target.transform.position.x) + Mathf.Abs(transform.position.y - target.transform.position.y) <= 1)
            {
                if (damageReceived >= 1)
                {
                    DamageIndicator instance = Instantiate(damageIndicator, transform.position, Quaternion.identity);
                    instance.Setup(damageReceived);
                    health -= damageReceived;
                    UpdateKingHealth();
                }
            }
        }
        else
        {
            if (damageReceived >= 1)
            {
                DamageIndicator instance = Instantiate(damageIndicator, transform.position, Quaternion.identity);
                instance.Setup(damageReceived);
                health -= damageReceived;
                UpdateKingHealth();
            }
        }
        

        if (target.health <= 0)
        {
            if (target.unitType == UnitType.King)
            {
                target.victoryPanel.SetActive(true);
            }
            Instantiate(deathEffect, target.transform.position, Quaternion.identity);
            Destroy(target.gameObject);
            GetWalkableTiles();
            gm.RemoveStatsPanel(target);
        }
        if (health <= 0)
        {
            /*
            if (unitType == UnitType.King)
            {
                Debug.Log("asdf");
                victoryPanel.SetActive(true);
            }
            */
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            gm.ResetTiles();
            gm.RemoveStatsPanel(this);
            Destroy(gameObject);
        }

        gm.UpdateStatsPanel();

        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = unitInactive;
        }
        selected = false;
        gm.selectedUnit = null;
        gm.ResetTiles();
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
                if (tile.IsClear())
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

        if (hasMoved == true && enemiesInRange.Count == 0)
        {
            foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
            {
                sr.color = unitInactive;
            }
            selected = false;
            gm.selectedUnit = null;
            gm.ResetTiles();
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
        gm.MoveStatsPanel(this);
    }
}
