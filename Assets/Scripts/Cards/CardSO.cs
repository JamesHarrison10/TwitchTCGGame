using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TwitchIntegration;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "TraidingCard")]
public class CardSO : ScriptableObject
{
    public TwitchUser cardOwner;
    public string cardName;
    public string description;
    public Rarity rarity;
    public Sprite icon;
    [Range(0f, 100f)]public int score;

    public Color cardColor;

    public void ColorUpdate()
    {
        Debug.Log(rarity.ToString());
        switch (rarity)
        {
            case Rarity.Nothing:
                {
                    cardColor = Color.white;
                    break;
                }
            case Rarity.Common:
                {
                    cardColor = Color.gray;
                    break;
                }
            case Rarity.Uncommon:
                {
                    cardColor = Color.green;
                    break;
                }
            case Rarity.Rare:
                {
                    cardColor = Color.blue;
                    break;
                }
            case Rarity.Epic:
                {
                    cardColor = Color.magenta;
                    break;
                }
            case Rarity.Legendary:
                {
                    cardColor = Color.yellow;
                    break;
                }
        }
    }
}

public enum Rarity
{
    Nothing,
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}
