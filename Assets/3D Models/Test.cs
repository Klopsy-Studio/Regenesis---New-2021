using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Texture sprite;
    public Material material;

    private void Update()
    {
        material.SetTexture("_mainTex", sprite);
    }
}
