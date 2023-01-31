using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerScript : MonoBehaviour
{
    bool toggle;
    [SerializeField] KeyCode key;
    [SerializeField] GameObject battleUi;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            if (toggle)
            {
                battleUi.SetActive(true);
                toggle = false;
            }
            else
            {
                battleUi.SetActive(false);
                toggle = true;
            }
        }
    }
}
