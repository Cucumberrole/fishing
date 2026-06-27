using UnityEngine;

[CreateAssetMenu(fileName = "NewFish", menuName = "Fish")]
public class FishData : ScriptableObject
{
    public string fishName;
    public Sprite fishSprite;
    public int money;
    public FishSize size;
}

public enum FishSize
{
    Small,
    Medium,
    Large
}
