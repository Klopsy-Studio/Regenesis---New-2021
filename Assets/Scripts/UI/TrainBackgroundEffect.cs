using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainBackgroundEffect : MonoBehaviour
{
    [SerializeField] TilingDirection direction;
    [SerializeField] SpriteRenderer image;
    Material backgroundMaterial;

    [Range(0f, 5f)]
    [SerializeField] float moveSpeed;
    float offset;

    void Start()
    {
        backgroundMaterial = image.material;
    }

    // Update is called once per frame
    void Update()
    {
        switch (direction)
        {
            case TilingDirection.Left:
                offset -= (moveSpeed * Time.deltaTime) / 10f;
                break;
            case TilingDirection.Right:
                offset += (moveSpeed * Time.deltaTime) / 10f;
                break;
            default:
                break;
        }
        backgroundMaterial.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}

public enum TilingDirection
{
    Left, Right,
}
