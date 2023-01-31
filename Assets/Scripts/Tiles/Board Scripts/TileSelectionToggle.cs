using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TileSelectionState
{
    Big, Small
}
public class TileSelectionToggle : MonoBehaviour
{
    [SerializeField] GameObject smallTileSelection;
    [SerializeField] GameObject bigTileSelection;

    bool bigOrSmall; //True Small, False Big
    TileSelectionState tileSelectionState = TileSelectionState.Small;

    public void MakeTileSelectionBig()
    {
        if(tileSelectionState == TileSelectionState.Small)
        {
            smallTileSelection.SetActive(false);
            bigTileSelection.SetActive(true);
            tileSelectionState = TileSelectionState.Big;
        }
    }

    public void MakeTileSelectionSmall()
    {
        if (tileSelectionState == TileSelectionState.Big)
        {
            smallTileSelection.SetActive(true);
            bigTileSelection.SetActive(false);
            tileSelectionState = TileSelectionState.Small;
        }
    }
}
