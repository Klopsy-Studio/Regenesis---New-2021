using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUnitUI : UnitUI
{
    bool updatingValue;
    [SerializeField] float speed;
    [Header("UI")]
    public Canvas unitUI;
    public GameObject actionPointsObject;
    [SerializeField] GameObject stunIndicator;
    [SerializeField] List<ActionPoint> actionPoints;

    [SerializeField] List<ActionPoint> originalActionPoints;
    [SerializeField] List<ActionPoint> usedActionPoints = new List<ActionPoint>();

    List<ActionPoint> previewActionPoints = new List<ActionPoint>();

    [SerializeField] Slider healthBar;
    [Header("Sprites")]
    [SerializeField] Sprite regularActionPointsSprite;
    [SerializeField] Sprite previewActionPointsSprite;
    [SerializeField] Sprite spentActionPointsSprite;
    [SerializeField] Animator bulletAnimations;
    [Header("Hammer")]
    [SerializeField] Slider unitFury;

    [Header("Gunblade")]
    [SerializeField] GameObject bulletsObject;
    [SerializeField] List<Image> bullets;
    [SerializeField] List<Image> originalBullets;
    [SerializeField] List<Image> usedBullets;

    [SerializeField] Color regularColor;
    [SerializeField] Color usedColor;
    [SerializeField] Color previewColor;

    List<Image> previewBullets = new List<Image>();

    int index;
    [SerializeField] float timer = 2f;
    [SerializeField] float originalTimer;
    bool beginTimer;
    [SerializeField] bool toggleShowUnitUI;

    private void Start()
    {
        originalTimer = timer;
        foreach(ActionPoint i in actionPoints)
        {
            originalActionPoints.Add(i);
        }

        foreach(Image i in bullets)
        {
            originalBullets.Add(i);
        }
    }

    public void HideBullets()
    {
        if (bulletsObject != null)
        {
            bulletsObject.SetActive(false);
        }
    }
    public void HideActionPoints()
    {
        actionPointsObject.SetActive(false);
    }
    
    public void ShowBullets()
    {
        previewBullets.Clear();
        bulletsObject.SetActive(true);

        if (bullets != null)
        {
            foreach (Image i in bullets)
            {
                i.color = regularColor;
            }
        }


        if (usedBullets != null)
        {
            foreach (Image i in usedBullets)
            {
                i.color = usedColor;
            }
        }
    }
    public void ShowActionPoints()
    {
        previewActionPoints.Clear();
        if (toggleShowUnitUI)
        {
            actionPointsObject.SetActive(true);
        }

        if (actionPoints != null)
        {
            foreach (ActionPoint i in actionPoints)
            {
                i.ResetPoint();
            }
        }
        

        if(usedActionPoints != null)
        {
            foreach (ActionPoint i in usedActionPoints)
            {
                i.SpendPoint();
            }
        }
    }

    public void ChangeFuryValue(int value)
    {
        StartCoroutine(SliderValueAnimation(unitFury, value));
    }

    public void EnableStun()
    {
        stunIndicator.SetActive(true);
    }

    public void DisableStun()
    {
        stunIndicator.SetActive(false);
    }

    public void PreviewActionCost(int actionCost)
    {
        index = actionPoints.Count - 1;
        for (int i = actionCost; i >0; i--)
        {
            actionPoints[index].PreviewPoint();
            previewActionPoints.Add(actionPoints[index]);
            index--;
        }
    }


    public void PreviewBulletCost(int bulletCost)
    {
        index = bullets.Count - 1;

        if(bulletCost > 0)
        {
            if(bulletCost == 6)
            {
                for (int i = bullets.Count; i > 0; i--)
                {
                    bullets[index].color = previewColor;
                    previewBullets.Add(bullets[index]);
                    index--;
                }
            }
            else
            {
                for (int i = bulletCost; i > 0; i--)
                {
                    bullets[index].color = previewColor;
                    previewBullets.Add(bullets[index]);
                    index--;
                }
            }
        }
    }

    public void GainBullets(int bulletGain)
    {
        while(bulletGain+bullets.Count> originalBullets.Count)
        {
            bulletGain--;
        }
        index = usedBullets.Count-1;

        for (int i = bulletGain; i >0; i--)
        {
            bullets.Add(usedBullets[index]);
            usedBullets.RemoveAt(index);
            index--;
        }

    }
    public void SpendActionPoints(int actionCost)
    {
        index = actionPoints.Count - 1;
        for (int i = actionCost; i >0; i--)
        {
            actionPoints.RemoveAt(index);
            index--;
        }

        foreach(ActionPoint i in previewActionPoints)
        {
            usedActionPoints.Add(i);
        }
    }


    public void SpendBullets(int bulletCost)
    {
        index = bullets.Count - 1;
        for (int i = bulletCost; i > 0; i--)
        {
            bullets.RemoveAt(index);
            index--;
        }

        foreach (Image i in previewBullets)
        {
            usedBullets.Add(i);
        }
    }

    public void ResetBullets()
    {
        bullets.Clear();
        usedBullets.Clear();
        previewBullets.Clear();

        foreach (Image i in originalBullets)
        {
            bullets.Add(i);
        }
    }
    public void ResetActionPoints()
    {
        actionPoints.Clear();
        usedActionPoints.Clear();
        previewActionPoints.Clear();

        foreach(ActionPoint i in originalActionPoints)
        {
            actionPoints.Add(i);
        }
    }

    public void EnableHealthBar()
    {
        healthBar.gameObject.SetActive(true);
    }
    public void DisableHealthBar()
    {
        healthBar.gameObject.SetActive(false);
    }

    float currentTime;

    public void HealthAnimation(int healthTarget)
    {
        EnableHealthBar();
        StartCoroutine(SliderValueAnimation(healthBar, healthTarget));
    }
    IEnumerator SliderValueAnimation(Slider s, int targetValue)
    {
        s.gameObject.SetActive(true);
        updatingValue = true;
        beginTimer = true;
        timer = originalTimer;
        if (s.value >= targetValue)
        {
            while (s.value > targetValue)
            {
                s.value = Mathf.Lerp(s.value, targetValue, currentTime);
                currentTime += Time.deltaTime * speed;
                yield return null;
                if (s.value <= 0)
                {
                    break;
                }
            }
        }
        else
        {
            while (s.value < targetValue)
            {
                s.value = Mathf.Lerp(s.value, targetValue, currentTime);
                currentTime += Time.deltaTime * speed;
                yield return null;

                if (s.value >= s.maxValue)
                {
                    break;
                }
            }
        }

        s.value = targetValue;
        currentTime = 0;
        updatingValue = false;
    }

    public void Update()
    {
        if (beginTimer)
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                beginTimer = false;
                timer = originalTimer;
                DisableHealthBar();
            }
        }
    }
}

[System.Serializable]
public class ActionPoint
{
    public GameObject parent;
    public SpriteRenderer regular;
    public SpriteRenderer preview;

    public void ResetPoint()
    {
        SetImageAlpha(preview, 0);
        SetImageAlpha(regular, 1);
    }

    public void SpendPoint()
    {
        SetImageAlpha(preview, 0);
        SetImageAlpha(regular, 0);
    }
    public void PreviewPoint()
    {
        SetImageAlpha(preview, 1);
        SetImageAlpha(regular, 0);
    }
    public void SetImageAlpha(SpriteRenderer image, int alpha)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }

    
}
