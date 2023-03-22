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


    [SerializeField] List<SpriteRenderer> smallSelection;
    

    [Header("Colors")]
    [SerializeField] Color movementColor;
    [SerializeField] Color targetColor;
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

    public void SelectionTarget()
    {
        ChangeSelectorColor(smallSelection, targetColor);
    }
    public void SelectionMovement()
    {
        ChangeSelectorColor(smallSelection, movementColor);
    }
    public void ChangeSelectorColor(List<SpriteRenderer> selectors, Color colorToChange)
    {
        foreach(SpriteRenderer s in selectors)
        {
            s.color = colorToChange; 
        }
    }

}
