using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSprite : MonoBehaviour
{
    public SpriteRenderer weaponSprite;

    void Start()
    {
        
    }

    public void TakeOutWeapon(Transform weaponPosition)
    {
        weaponSprite.sortingOrder = 1;
        ChangeWeaponPosition(weaponPosition);
    }

    public void ChangeWeaponPosition(Transform weaponPosition)
    {
        weaponSprite.transform.position = weaponPosition.position;
        weaponSprite.transform.rotation = weaponPosition.rotation;
    }


    public void SaveWeapon(Transform weaponPosition)
    {
        weaponSprite.sortingOrder = -1;
        ChangeWeaponPosition(weaponPosition);
    }
}
