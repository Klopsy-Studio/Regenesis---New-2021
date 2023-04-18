using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCursor : MonoBehaviour
{
    public static GameCursor instance;

    [SerializeField] Texture2D regularCursor;
    [SerializeField] Texture2D handCursor;

    Vector2 cursorHotspot;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        cursorHotspot = new Vector2(regularCursor.width / 2, regularCursor.height / 2);
    }

    public void SetRegularCursor()
    {
        Cursor.SetCursor(regularCursor, cursorHotspot, CursorMode.Auto);
    }
    public void SetHandCursor()
    {
        Cursor.SetCursor(handCursor, cursorHotspot, CursorMode.Auto);

    }
}
