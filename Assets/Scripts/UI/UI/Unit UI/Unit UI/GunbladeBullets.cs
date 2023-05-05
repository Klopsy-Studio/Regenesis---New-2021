using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunbladeBullets : MonoBehaviour
{
    public GameObject bulletParent;

    [SerializeField] List<Animator> originalBullets;
    [SerializeField] List<Animator> currentBullets;
    [SerializeField] List<Animator> usedBullets;
    public MenuButton button;
    List<Animator> previewBullets = new List<Animator>();

    int index;


    private void Start()
    {
        foreach (Animator i in currentBullets)
        {
            originalBullets.Add(i);
        }
    }

    public void ShowBullets(int currentBulletNumber)
    {
        previewBullets.Clear();
        usedBullets.Clear();
        currentBullets.Clear();
        bulletParent.SetActive(true);


        for (int i = 0; i < 5; i++)
        {
            if(i >= currentBulletNumber)
            {
                originalBullets[i].SetBool("used", true);
                usedBullets.Add(originalBullets[i]);
                originalBullets[i].SetBool("preview", false);
                originalBullets[i].SetBool("gain", false);

            }
            else
            {
                originalBullets[i].SetBool("used", false);
                originalBullets[i].SetBool("preview", false);
                originalBullets[i].SetBool("gain", false);

                currentBullets.Add(originalBullets[i]);
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
        index = currentBullets.Count - 1;

        if (bulletCost > 0)
        {
            if (bulletCost == 6)
            {
                foreach(Animator anim in currentBullets)
                {
                    anim.SetBool("preview", true);
                    previewBullets.Add(anim);
                }
            }
            else
            {
                for (int i = bulletCost; i > 0; i--)
                {
                    currentBullets[index].SetBool("preview", true);
                    previewBullets.Add(currentBullets[index]);
                    index--;
                }
            }
        }
    }


    public void PreviewBulletGain(int bulletGain)
    {
        if (bulletGain > 0 && currentBullets.Count<5)
        {
            for (int i = 0; i < bulletGain; i++)
            {
                if (i >= usedBullets.Count)
                    break;

                usedBullets[i].SetBool("gain", true);
                previewBullets.Add(usedBullets[i]);
            }
        }
    }

    public void GainBullets(int bulletGain)
    {
        while (bulletGain + currentBullets.Count > originalBullets.Count)
        {
            bulletGain--;
        }
        index = usedBullets.Count - 1;

        for (int i = bulletGain; i > 0; i--)
        {
            currentBullets.Add(usedBullets[index]);
            usedBullets.RemoveAt(index);
            index--;
        }

    }


    public void SpendBullets(int bulletCost)
    {
        index = currentBullets.Count - 1;
        for (int i = bulletCost; i > 0; i--)
        {
            currentBullets.RemoveAt(index);
            index--;
        }

        foreach (Animator i in previewBullets)
        {
            usedBullets.Add(i);
        }
    }

    public void ResetBullets()
    {
        currentBullets.Clear();
        usedBullets.Clear();
        previewBullets.Clear();
    }
  
}
