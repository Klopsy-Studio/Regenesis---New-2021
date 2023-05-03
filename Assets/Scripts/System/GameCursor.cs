using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCursor : MonoBehaviour
{
    public static GameCursor instance;

    [SerializeField] Image cursorSprite;
    [SerializeField] Sprite regularCursor;
    [SerializeField] Sprite handCursor;

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
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Confined;
        SetRegularCursor();
    }
    public void LateUpdate()
    {
        //Cursor.visible = false;
        Vector2 cursorPos = Input.mousePosition;
        cursorSprite.rectTransform.position = cursorPos;
    }

    public void SetRegularCursor()
    {
        cursorSprite.sprite = regularCursor;
        cursorSprite.SetNativeSize();
    }
    public void SetHandCursor()
    {
        cursorSprite.sprite = handCursor;
        cursorSprite.SetNativeSize();
    }
}
