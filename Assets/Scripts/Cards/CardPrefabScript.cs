using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardPrefabScript : MonoBehaviour
{
    public CardSO Card;

    [Header("Card Elements")]
    [SerializeField] private TextMeshProUGUI cardNameTxt;
    [SerializeField] private TextMeshProUGUI cardRarityText;
    [SerializeField] private TextMeshProUGUI cardDescription;
    [SerializeField] private TextMeshProUGUI OwnerTag;
    [SerializeField] private Image cardBackgroundColor;

    private void Start()
    {
        OwnerTag.text = Card.cardOwner.displayname;

        Card.ColorUpdate();

        cardBackgroundColor.color = Card.cardColor;

        cardRarityText.text = Card.rarity.ToString();

        cardNameTxt.text = Card.cardName;

        cardDescription.text = Card.description;
    }
}
