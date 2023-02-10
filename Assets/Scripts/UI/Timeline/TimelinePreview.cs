using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class TimelinePreview : MonoBehaviour
{
    List<Unit> previewTimelineList = new List<Unit>();
    List<Unit> sortedPreviewList;
    public void CalculateOrder(List<Unit> _unitsInGame)
    {
        previewTimelineList.Clear();
        //Reset previewTime just in case
        foreach (var unit in _unitsInGame)
        {
            unit.previewTime = 0;
        }

        //Set previewTime
        foreach (var unit in _unitsInGame)
        {
            unit.previewTime = UniformLinearMotion_Time(unit);
            previewTimelineList.Add(unit);
        }

        sortedPreviewList = previewTimelineList.OrderBy(o => o.previewTime).ToList();

        for (int i = 0; i < sortedPreviewList.Count; i++)
        {
            Debug.Log("orden es " + i + " " + sortedPreviewList[i].name);
        }
       
        
    }


    float UniformLinearMotion_Time(Unit unit)
    {
        float finalPos = 100;
        float initPos = unit.timelineFill;
        float velocity = unit.fTimelineVelocity;

        float time = (finalPos - initPos) / velocity;

        return time;
    }



}



// C# program to demonstrate the concept of 
// List<T>.Sort(IComparer <T>) method
//https://www.geeksforgeeks.org/how-to-sort-list-in-c-sharp-set-1/#m2


class GFG : IComparer<int>
{
    public int Compare(int x, int y)
    {
        if (x == 0 || y == 0)
        {
            return 0;
        }

        // CompareTo() method
        return x.CompareTo(y);

    }
}

public class geek
{

    // Main Method
    public static void Main()
    {

        // List initialize
        List<int> list1 = new List<int>
        {
              
            // list elements
            1,5,6,2,4,3

        };

        Console.WriteLine("Original List");

        foreach (int g in list1)
        {

            // Display Original List
            Console.WriteLine(g);

        }

        // "gg" is the object of class GFG
        GFG gg = new GFG();

        Console.WriteLine("\nSort with a comparer:");

        // use of List<T>.Sort(IComparer<T>) 
        // method. The comparer is "gg"
        list1.Sort(gg);

        foreach (int g in list1)
        {

            // Display sorted list
            Console.WriteLine(g);

        }
    }
}
