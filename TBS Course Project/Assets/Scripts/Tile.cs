using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    private SpriteRenderer rend;
    public GameObject cursor;
    public enum TerrainType { None, Plains, Forest, Mountains, Sand, Water }
    public TerrainType terrainType;
    public bool randomGeneration;
    public Sprite[] tileGraphics;

    public LayerMask obstacleLayer;

    public Color highlightedColor;
    public bool isTraversable;
    GameMaster gm;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        if (randomGeneration == true)
        {
            int randTile = Random.Range(0, tileGraphics.Length);
            rend.sprite = tileGraphics[randTile];
        }
        else if (terrainType == TerrainType.None)
        {
            rend.sprite = null;
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

    public bool IsClear(Unit.UnitType unitType)
    {
        Collider2D obstacle = Physics2D.OverlapCircle(transform.position, 0.2f, obstacleLayer);
        if (obstacle != null)
        {
            return false;
        }

        if (unitType == Unit.UnitType.Axe && terrainType == TerrainType.Mountains)
        {
            return false;
        }
        else if (unitType == Unit.UnitType.Sword && terrainType == TerrainType.Mountains)
        {
            return false;
        }
        else if (unitType == Unit.UnitType.Bow && terrainType == TerrainType.Mountains)
        {
            return false;
        }
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
    }

    private void OnMouseDown()
    {
        if (isTraversable && gm.selectedUnit != null)
        {
            gm.selectedUnit.Move(transform.position);
        }
    }
}
