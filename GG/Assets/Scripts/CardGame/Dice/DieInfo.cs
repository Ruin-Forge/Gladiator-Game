using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DieInfo
{
    #region Fields

    private readonly int[] validSideCounts = new int[] { 4, 6, 8, 12, 20 };

    private int[] sides;
    private int index = 0;

    #endregion Fields

    #region ctor

    public DieInfo(int sideCount)
    {
        if (sideCount < 2)
            throw new System.ArgumentException("Die must have more than one side");

        int totalSideCount = sideCount;
        //while (!validSideCounts.Contains(totalSideCount))
        //{
        //    if (totalSideCount < 21)
        //        totalSideCount *= 2;
        //    else
        //        throw new System.ArgumentException("SideCount not valid");
        //}

        this.sides = new int[totalSideCount];
        for (int i = 0; i < sides.Length; i++)
        {
            this.sides[i] = i % sideCount + 1;
        }
    }

    public DieInfo(params int[] sides)
    {
        if (sides == null)
            throw new System.ArgumentNullException();
        else if (sides.Length < 2)
            throw new System.ArgumentException("Die must have more than one side");

        this.sides = sides;

        //while (!validSideCounts.Contains(sides.Length))
        //{
        //    if (sides.Length < 21)
        //        this.sides = this.sides.Concat(sides).ToArray();
        //    else
        //        throw new System.ArgumentException("SideCount not valid");
        //}
    }

    #endregion ctor

    #region Properties

    public int[] Sides => sides;
    public int Value => sides[index];

    #endregion Properties

    public void Roll()
    {
        index = Random.Range(0, sides.Length);
    }
}