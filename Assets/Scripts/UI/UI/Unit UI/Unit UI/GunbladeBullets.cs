using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunbladeBullets : MonoBehaviour
{
    public GameObject bulletParent;

    [SerializeField] List<Animator> bullets;
    [SerializeField] List<Animator> originalBullets;
    [SerializeField] List<Animator> usedBullets;
    public MenuButton button;
    List<Animator> previewBullets = new List<Animator>();

    int index;


    private void Start()
    {
        foreach (Animator i in bullets)
        {
            originalBullets.Add(i);
        }
    }

    public void ShowBullets()
    {
        previewBullets.Clear();
        bulletParent.SetActive(true);

        if (bullets != null)
        {
            foreach (Animator i in bullets)
            {
                i.SetBool("preview", false);
                i.SetBool("used", false);
            }
        }


        if (usedBullets != null)
        {
            foreach (Animator i in usedBullets)
            {
                i.SetBool("used", true);
            }
        }
    }
    public void PreviewBulletCost(int bulletCost)
    {
        index = bullets.Count - 1;

        if (bulletCost > 0)
        {
            if (bulletCost == 6)
            {
                for (int i = bullets.Count; i > 0; i--)
                {
                    bullets[index].SetBool("preview", true);
                    previewBullets.Add(bullets[index]);
                    index--;
                }
            }
            else
            {
                for (int i = bulletCost; i > 0; i--)
                {
                    bullets[index].SetBool("preview", true);
                    previewBullets.Add(bullets[index]);
                    index--;
                }
            }
        }
    }

    public void GainBullets(int bulletGain)
    {
        while (bulletGain + bullets.Count > originalBullets.Count)
        {
            bulletGain--;
        }
        index = usedBullets.Count - 1;

        for (int i = bulletGain; i > 0; i--)
        {
            bullets.Add(usedBullets[index]);
            usedBullets.RemoveAt(index);
            index--;
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

        foreach (Animator i in previewBullets)
        {
            usedBullets.Add(i);
        }
    }

    public void ResetBullets()
    {
        bullets.Clear();
        usedBullets.Clear();
        previewBullets.Clear();

        foreach (Animator i in originalBullets)
        {
            bullets.Add(i);
        }
    }
  
}
