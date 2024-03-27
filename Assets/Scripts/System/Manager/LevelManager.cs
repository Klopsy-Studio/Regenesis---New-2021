using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<SpriteFade> levelProps = new List<SpriteFade>();

    [SerializeField] List<Transform> parents;

    private void Start()
    {
        foreach(Transform t in parents)
        {
            foreach(Transform c in t)
            {
                if(c.TryGetComponent<SpriteFade>(out SpriteFade s))
                {
                    levelProps.Add(s);
                }
            }
        }
    }

    public void FadeProps()
    {
        foreach(SpriteFade s in levelProps)
        {
            s.SetFadeValue(true);
        }
    }

    public void ResetProps()
    {
        foreach (SpriteFade s in levelProps)
        {
            s.SetFadeValue(false);
        }
    }
}
