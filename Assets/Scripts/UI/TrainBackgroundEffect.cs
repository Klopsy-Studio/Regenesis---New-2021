using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainBackgroundEffect : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;
    Material backgroundMaterial;
    [SerializeField] float moveSpeed;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        backgroundMaterial = sprite.material;
    }

    // Update is called once per frame
    void Update()
    {
        backgroundMaterial.mainTextureOffset += new Vector2(moveSpeed * Time.deltaTime, 0);
    }
}
