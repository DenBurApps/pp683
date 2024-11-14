using System;
using System.Collections.Generic;
using UnityEngine;

public class HobbyLogoHolder : MonoBehaviour
{
    [SerializeField] private List<HobbyLogo> _logos;

    public Sprite GetLogoSprite(GoalTypes type)
    {
        foreach (var logo in _logos)
        {
            if (logo.Type == type)
            {
                return logo.Sprite;
            }
        }

        return null;
    }
}

[Serializable]
public class HobbyLogo
{
    public GoalTypes Type;
    public Sprite Sprite;
}