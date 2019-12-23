﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    private SpriteRenderer rend;
    public GameObject cursor;
    public enum TerrainType { None, Plains, Forest, Mountain, Sand, Water }
    public TerrainType terrainType;
    public bool randomGeneration;
    public Sprite[] tileGraphics;

    public LayerMask obstacleLayer;

    public Color highlightedColor;
    public bool isTraversable;
    GameMaster gm;

    public Color creatableColor;
    public bool isCreatable;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        if (randomGeneration == true)
        {
            int randTile = Random.Range(0, tileGraphics.Length);
            rend.sprite = tileGraphics[randTile];
            switch (randTile)
            {
                case 0:
                    terrainType = TerrainType.Mountain;
                    break;
                case 1:
                    terrainType = TerrainType.Forest;
                    break;
                case 2:
                    terrainType = TerrainType.Plains;
                    break;
                case 3:
                    terrainType = TerrainType.Water;
                    break;
                case 4:
                    terrainType = TerrainType.Sand;
                    break;
            }
        }
        else if (terrainType == TerrainType.None)
        {
            rend.sprite = null;
        }
        else if (terrainType == TerrainType.Plains)
        {
            rend.sprite = tileGraphics[2];
        }
        else if (terrainType == TerrainType.Forest)
        {
            rend.sprite = tileGraphics[1];
        }
        else if (terrainType == TerrainType.Mountain)
        {
            rend.sprite = tileGraphics[0];
        }
        else if (terrainType == TerrainType.Sand)
        {
            rend.sprite = tileGraphics[4];
        }
        else if (terrainType == TerrainType.Water)
        {
            rend.sprite = tileGraphics[3];
        }

        gm = FindObjectOfType<GameMaster>();

    }

    void OnMouseEnter()
    {
        if (cursor != null)
        {
            cursor.transform.position = transform.position;
        }
    }

    public bool IsClear()
    {
        Collider2D obstacle = Physics2D.OverlapCircle(transform.position, 0.2f, obstacleLayer);
        if (obstacle != null)
        {
            return false;
        }
        if (terrainType == TerrainType.None && randomGeneration == false)
        {
            return false;
        }
        if (terrainType == TerrainType.Mountain || terrainType == TerrainType.Water)
        {
            return false;
        }
        /*
        if (unitType == Unit.UnitType.Archer && (terrainType == TerrainType.None && randomGeneration == false || terrainType == TerrainType.Mountain))
        {
            return false;
        }
        else if (unitType == Unit.UnitType.Bruiser && (terrainType == TerrainType.None && randomGeneration == false || terrainType == TerrainType.Mountain))
        {
            return false;
        }
        else if (unitType == Unit.UnitType.Healer && (terrainType == TerrainType.None && randomGeneration == false || terrainType == TerrainType.Mountain))
        {
            return false;
        }
        else if (unitType == Unit.UnitType.King && (terrainType == TerrainType.None && randomGeneration == false || terrainType == TerrainType.Mountain))
        {
            return false;
        }
        else if (unitType == Unit.UnitType.Soldier && (terrainType == TerrainType.None && randomGeneration == false || terrainType == TerrainType.Mountain))
        {
            return false;
        }
        */
        return true;
    }

    public void Highlight()
    {
        rend.color = highlightedColor;
        isTraversable = true;
    }

    public void Reset()
    {
        rend.color = Color.white;
        isTraversable = false;
        isCreatable = false;
    }

    public void SetCreatable()
    {
        rend.color = creatableColor;
        isCreatable = true;
    }

    private void OnMouseDown()
    {
        if (isTraversable && gm.selectedUnit != null)
        {
            gm.selectedUnit.Move(transform.position);
        }
        else if (isCreatable == true)
        {
            BarrackItem item = Instantiate(gm.purchasedItem, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            gm.ResetTiles();
            Unit unit = item.GetComponent<Unit>();
            if (unit != null)
            {
                foreach (SpriteRenderer sr in unit.GetComponentsInChildren<SpriteRenderer>())
                {
                    sr.color = unit.unitInactive;
                }
                unit.hasMoved = true;
                unit.hasAttacked = true;
            }
        }
    }
}
