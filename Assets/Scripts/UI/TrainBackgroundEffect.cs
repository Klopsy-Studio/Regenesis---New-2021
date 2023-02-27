using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainBackgroundEffect : MonoBehaviour
{
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
        offset += (moveSpeed * Time.deltaTime) / 10f;
        backgroundMaterial.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
