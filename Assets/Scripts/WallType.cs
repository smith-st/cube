using System;
using UnityEngine;

public enum WallType
{
    Small,
    Big
}

[Serializable]
public struct WallAsset
{
    public WallType type;
    public GameObject gameObject;
}