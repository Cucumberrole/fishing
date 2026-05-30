using UnityEngine;

[CreateAssetMenu(fileName = "NewFish", menuName = "Fish")]
public class FishData : ScriptableObject //ScriptableObjectêßçÏÅI
{
    public string fishName;

    public Sprite fishSprite;

    public int money;

    public FishSize size;

    public bool isRate;
}

public enum FishSize
{
    Small,
    Medium,
    Large
}