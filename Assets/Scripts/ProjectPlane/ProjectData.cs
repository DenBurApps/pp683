using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProjectData
{
    public string Name;
    public string Description;
    public string Goal;
    public string StartTime;
    public string EndTime;
    public string StartDate;
    public string EndDate;
    public GoalTypes Type;


    public ProjectData(string name, string description, string goal, string startTime, string endTime, string startDate, string endDate)
    {
        Name = name;
        Description = description;
        Goal = goal;
        StartTime = startTime;
        EndTime = endTime;
        StartDate = startDate;
        EndDate = endDate;
        Type = GoalTypes.None;
    }
}
