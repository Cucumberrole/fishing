using UnityEngine;

[CreateAssetMenu(fileName = "NewFish", menuName = "Fish")]
public class FishData : ScriptableObject //ScriptableObject§ìI
{
    public string fishName;

    public Sprite fishSprite;

    public int Money;

    public float speed;

    public float isRate;
}