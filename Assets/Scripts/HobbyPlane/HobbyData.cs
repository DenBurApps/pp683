using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HobbyData
{
    public string Name;
    public string Description;
    public string Goal;
    public byte[] ImagePath;
    public GoalTypes Type;

    public HobbyData(string name, string description, string goal, byte[] imagePath)
    {
        Name = name;
        Description = description;
        Goal = goal;
        ImagePath = imagePath;
        Type = GoalTypes.None;
    }
}

public enum GoalTypes
{
    Yes,
    AlmostThere,
    No,
    None
}
