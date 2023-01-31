using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoardCreator))]
public class BoardCreatorInspector : Editor
{
    public bool growOrShrink = true;
    void AddTileSpawnButton(TileSpriteData[] data, TileClass tileSpriteType)
    {
        foreach(TileSpriteData d in data)
        {
            foreach (SpriteTile S in d.sprites)
            {
                if (S.type == tileSpriteType)
                {
                    foreach (Sprite s in S.Sprites)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(100f);
                        if (GUILayout.Button("Choose " + d.name + " " + s.name, GUILayout.MaxHeight(20f), GUILayout.MaxWidth(200f)))
                        {
                            current.ChangeTileToSpawn(s);
                        }
                        GUILayout.Space(50f);

                        GUILayout.Box(s.texture);


                        GUILayout.EndHorizontal();
                        GUILayout.Space(20f);

                    }
                }
            }
        }
        
        
    }

    void AddPropSpawnButton(GameObject[] newProp)
    {
        foreach (GameObject p in newProp)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(p.GetComponent<Prop>().data.displayName, GUILayout.MaxHeight(20f), GUILayout.MaxWidth(200f)))
            {
                current.ChangePropToSpawn(p);
            }
            GUILayout.Box(p.GetComponent<Prop>().sprite.sprite.texture, GUILayout.MaxHeight(80f), GUILayout.Width(80f));

            GUILayout.EndHorizontal();

        }
    }
    public BoardCreator current
    {
        get
        {
            return (BoardCreator)target;
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(10f);

        switch (current.thingToSpawn)
        {
            case ThingToSpawn.Tiles:
                if (current.spriteToSpawn != null && current.canSpawn)
                {
                    GUI.DrawTexture(current.texturePosition, current.spriteToSpawn.texture);
                    if (!current.makeTilePlayable)
                    {
                        GUI.DrawTexture(current.texturePosition, current.notPlayableTexture);
                    }
                }
                switch (current.TypeOfTile)
                {
                    case TileType.Placeholder:
                        
                        break;
                    case TileType.Desert:
                        break;
                    case TileType.NonPlayable:
                        break;
                    case TileType.City:
                        AddTileSpawnButton(current.spawner.cityTiles, current.classOfSprite);
                        break;
                    default:
                        break;
                }
                break;
            case ThingToSpawn.Props:
                if (current.propToSpawn != null && current.canSpawn)
                {
                    GUI.DrawTexture(current.texturePosition, current.propToSpawn.GetComponent<Prop>().sprite.sprite.texture);
                    if (!current.doesPropOccupySpace)
                    {
                        GUI.DrawTexture(current.texturePosition, current.notPlayableTexture);
                    }
                }
                switch (current.TypeOfProp)
                {
                    case PropType.City:
                        AddPropSpawnButton(current.spawner.cityProps);
                        break;
                    case PropType.Desert:
                        AddPropSpawnButton(current.spawner.desertProps);
                        break;
                    case PropType.Park:
                        AddPropSpawnButton(current.spawner.parkProps);
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
        
        GUILayout.Space(10f);
        if (GUILayout.Button("Clear"))
            current.Clear();
        if (GUILayout.Button("Save"))
            current.Save();
        if (GUILayout.Button("Load"))
            current.Load();
        GUILayout.Space(10f);

        switch (current.thingToSpawn)
        {
            case ThingToSpawn.Tiles:
                if (GUILayout.Button("Spawn Tile"))
                    current.Grow();
                if (GUILayout.Button("Delete Tile"))
                    current.Shrink();
                break;
            case ThingToSpawn.Props:
                if (GUILayout.Button("Spawn Prop"))
                    current.SpawnProp();
                if (GUILayout.Button("Delete Prop"))
                    current.DeleteProp();
                break;
            default:
                break;
        }
        
        GUILayout.Space(10f);
        if (GUILayout.Button("Add Player Spawn"))
            current.AddPlayerSpawnPoint();
        if (GUILayout.Button("Add Enemy Spawn"))
            current.AddEnemySpawnPoint();
        GUILayout.Space(10f);

        if (GUILayout.Button("Clear Player Spawn"))
            current.ClearPlayerSpawnPoints();
        if (GUILayout.Button("Clear Enemy Spawn"))
            current.ClearEnemySpawnPoints();
        
        
        if (GUI.changed)
            current.UpdateMarker();
    }


    private void OnSceneGUI()
    {
        Event e = Event.current;

        switch (e.type)
        {
            case EventType.KeyDown:
            {
                    if (Event.current.keyCode == KeyCode.W)
                    {
                        current.MoveTileSelectionUpwards();
                        current.UpdateMarker();
                        e.Use();
                    }
                    if (Event.current.keyCode == KeyCode.A)
                    {
                        current.MoveTileSelectionLeft();
                        current.UpdateMarker();
                        e.Use();
                    }
                    if (Event.current.keyCode == KeyCode.S)
                    {
                        current.MoveTileSelectionDownwards();
                        current.UpdateMarker();
                        e.Use();
                    }
                    if (Event.current.keyCode == KeyCode.D)
                    {
                        current.MoveTileSelectionRight();
                        current.UpdateMarker();
                        e.Use();
                    }

                    if(Event.current.keyCode == KeyCode.Space && current.canSpawn)
                    {
                        if (growOrShrink)
                        {
                            growOrShrink = false;
                        }
                        else
                        {
                            growOrShrink = true;
                        }
                        e.Use();
                    }

                    if (Event.current.keyCode == KeyCode.E)
                    {
                        if (current.canSpawn)
                        {
                            current.canSpawn = false;
                        }
                        else
                        {
                            current.canSpawn = true;
                        }

                        e.Use();
                    }


                    break;
            }


            case EventType.MouseDown:
            {
                    switch (current.thingToSpawn)
                    {
                        case ThingToSpawn.Tiles:
                            if (growOrShrink && current.canSpawn)
                            {
                                current.Grow();
                                current.Grow();
                                current.Grow();
                                current.Grow();
                                current.UpdateMarker();
                                e.Use();
                                break;
                            }
                            else
                            {
                                if (current.canSpawn)
                                {
                                    current.Shrink();
                                    e.Use();
                                    break;
                                }
                            }
                            break;

                        case ThingToSpawn.Props:

                            if(growOrShrink && current.canSpawn)
                            {
                                current.SpawnProp();
                                e.Use();
                                break;
                            }
                            else
                            {
                                if (current.canSpawn)
                                {
                                    current.DeleteProp();
                                    e.Use();
                                    break;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    

                    break;
                        
            }

            //case EventType.MouseMove:
            //{
            //       current.MoveTileSelection(new Vector2(Event.current.mousePosition.x, Event.current.mousePosition.y));
            //       e.Use();
            //       break;
            //}


        }
    }
}
