using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderEvent : RealTimeEvents
{
    //DEPRECATED

    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject thunderExperimental;
    Dictionary<Point, Tile> dic = new Dictionary<Point, Tile>();
    List<Point> points = new List<Point>();
    List<Tile> tiles = new List<Tile>();
    List<Tile> randomTile = new List<Tile>();

    List<GameObject> thunderPrefabs = new List<GameObject>();
    Color redColor = Color.red;
    Color defaultColor = new Color(1, 1, 1, 1);

    List<GameObject> particleEffect1 = new List<GameObject>();
    List<GameObject> particleEffect2 = new List<GameObject>();


    [SerializeField] float timeUntilHit;
    public override void ApplyEffect()
    {
        //timelineFull += Time.deltaTime;
        //if (timelineFull >= rate)
        //{
        //    timelineFull = 0;
        //    SelectTiles(Board);
        //    //StartCoroutine(MoveTowardTile());
        //    StartCoroutine(AttackOnTile());
        //}
       
       
    }

    void SelectTiles(Board board)
    {
        //RECOGER TODAS LAS TILES
        dic = board.playableTiles;
        //LIMPIAR LAS TILES ROJAS

        if(particleEffect1.Count != 0)
        {
            foreach (var x in particleEffect1)
            {
                Destroy(x);

            }

        }
        randomTile.Clear();
        thunderPrefabs.Clear();
        foreach (var i in dic.Values)
        {
            tiles.Add(i);
        }
        //Seleccionar Tiles aleatorias

        
        for (int i = 0; i < 3; i++)
        {
            int randomNum = Random.Range(0, tiles.Count);
            randomTile.Add(tiles[randomNum]);
        }

    
        foreach (var x in randomTile)
        {
            GameObject i = Instantiate(particleEffects.fireEffect, x.transform.position, x.transform.rotation) as GameObject;

            particleEffect1.Add(i);
           
        }
    }


   



}
