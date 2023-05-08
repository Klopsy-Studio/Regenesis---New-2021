using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPartyIcon : MonoBehaviour
{
    PlayerUnit user;
    [SerializeField] Slider hunterHealth;
    [SerializeField] Image healthBarImage;
    [SerializeField] Image hunterPortraitImage;

    Sprite hunterDefaultSprite;
    Sprite hunterDeadSprite;


    [Header("Health Bar Colors")]
    [SerializeField] Color goodHealthColor;
    [SerializeField] Color mediumHealthColor;
    [SerializeField] Color lowHealthColor;

    float currentTime;
    float speed = 1;

    public void UnitDead()
    {
        hunterPortraitImage.sprite = hunterDeadSprite;
    }

    public void UnitDefault()
    {
        hunterPortraitImage.sprite = hunterDefaultSprite;
    }
    public void AssignPartyIcon(PlayerUnit unit)
    {
        user = unit;
        unit.partyIcon = this;
        hunterHealth.value = user.health;
        UpdateHealthColor(goodHealthColor);

        hunterDefaultSprite = user.profile.unitPartyIcon;
        hunterDeadSprite = user.profile.unitPartyIconDead;

        hunterPortraitImage.sprite = hunterDefaultSprite;
    }

    public void UpdateHealthColor(Color newColor)
    {
        healthBarImage.color = newColor;
    }

    public void ManageHealth()
    {
        if(hunterHealth.value <= 100 && hunterHealth.value > 50)
        {
            UpdateHealthColor(goodHealthColor);
        }
        else if(hunterHealth.value <= 50 && hunterHealth.value > 25)
        {
            UpdateHealthColor(mediumHealthColor);
        }
        else
        {
            UpdateHealthColor(lowHealthColor);
        }
    }

    public void UpdateHealthBar()
    {
        StartCoroutine(SliderValueAnimation());
    }
    IEnumerator SliderValueAnimation()
    {
        if (hunterHealth.value >= user.health)
        {
            while (hunterHealth.value > user.health)
            {
                hunterHealth.value = Mathf.Lerp(hunterHealth.value, user.health, currentTime);
                currentTime += Time.deltaTime * speed;
                yield return null;
                if (hunterHealth.value <= 0)
                {
                    break;
                }
            }
        }
        else
        {
            while (hunterHealth.value < user.health)
            {
                hunterHealth.value = Mathf.Lerp(hunterHealth.value, user.health, currentTime);
                currentTime += Time.deltaTime * speed;
                yield return null;

                if (hunterHealth.value >= user.health)
                {
                    break;
                }
            }
        }

        hunterHealth.value = user.health;
        ManageHealth();

        currentTime = 0;
    }
}
