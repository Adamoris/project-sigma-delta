using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    private SpriteRenderer rend;
    public GameObject cursor;
    public enum TerrainType { None, Plains, Forest, Mountains, Sand, Water }
    public TerrainType terrainType;
    //[DrawIf("terrainType", TerrainType.None, Comp)]
    public bool randomGeneration;
    public Sprite[] tileGraphics;

    //public float hoverAmount;

    // Start is called before the first frame update
    void Start()
    {
        if (randomGeneration == true)
        {
            rend = GetComponent<SpriteRenderer>();
            int randTile = Random.Range(0, tileGraphics.Length);
            rend.sprite = tileGraphics[randTile];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseEnter()
    {
        if (cursor != null)
        {
            cursor.transform.position = transform.position;
        }
    }
}
