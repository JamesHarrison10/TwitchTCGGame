using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using TwitchIntegration;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PackOpenScript : TwitchMonoBehaviour
{
    private int[] cardTable =
    {
        0,    // Nothing (0%)
        500,  // Common (50%)
        300,  // Uncommon (30%)
        150,  // Rare (15%)
        45,   // Epic (4.5%)
        5,    // Legendary (0.5%) - EXTREMELY RARE
    };

        //0, //Nothing (Nothing)
        //400, //common (%40)
        //300, // Uncommon (%30)
        //150, // Rare (%15)
        //100, //Epic (10%)
        //50, //Legendary

    [Header("Card Lists")]
    [SerializeField] private List<CardSO> commonCards;
    [SerializeField] private List<CardSO> uncommonCards;
    [SerializeField] private List<CardSO> rareCards;
    [SerializeField] private List<CardSO> epicCards;
    [SerializeField] private List<CardSO> legendaryCards;
    //public List<CardSO> sortedCards = new List<CardSO>();

    [Header("Card Stuff")]
    [SerializeField] private List<CardSO> topCards;
    [SerializeField] private CardSO nothingCard;
    public GameObject cardPrefab;

    [Header("Ui Locations")]
    public Transform ShowCardLocation;
    public Transform TopCardsUILocation;

    private GameObject lastPulledCard;

    private int total;

    public int totalPulls;

    //[TwitchCommand("OpenPack", "op", "OP")]
    public void OpenCard(TwitchUser user)
    {
        total = 0;

        totalPulls++;

        Destroy(lastPulledCard);

        foreach (var item in cardTable)
        {
            //summing up total weights
            total += item;
        }

        int randomNumber = Random.Range(0, total);
        Rarity selectedRarity = Rarity.Common;

        for (int i = 0; i < cardTable.Length; i++)
        {
            if (randomNumber <= cardTable[i])
            {
                selectedRarity = (Rarity)i;
                break;
            }
            else
            {
                randomNumber -= cardTable[i];
            }
        }

        CardSO cardChosen = GetRandomCardByRarity(selectedRarity);

        if (cardChosen != null)
        {
            cardChosen.cardOwner = user;

            TwitchManager.SendChatMessage($"Card Opening: {user.displayname} has opened... A {cardChosen.rarity.ToString()}!, Named: {cardChosen.cardName.ToString()}!!!");

            cardSpawn(cardChosen);
            UpdatePulls(user);
        }
        
    }

    private CardSO GetRandomCardByRarity(Rarity rarity)
    {
        List<CardSO> cardList = null;

        switch (rarity)
        {
            case Rarity.Common:
                cardList = commonCards;
                break;
            case Rarity.Uncommon:
                cardList = uncommonCards;
                break;
            case Rarity.Rare:
                cardList = rareCards;
                break;
            case Rarity.Epic:
                cardList = epicCards;
                break;
            case Rarity.Legendary:
                cardList = legendaryCards;
                break;
            default:
                return null;
        }

        if (cardList == null || cardList.Count == 0)
        {
            return nothingCard;
        }

        int randomIndex = Random.Range(0, cardList.Count);
        return cardList[randomIndex];
    }

    public void cardSpawn(CardSO selectedCard)
    {
        
        GameObject newCard = Instantiate(cardPrefab, ShowCardLocation);

        topCards.Add(selectedCard);

        newCard.GetComponent<CardPrefabScript>().Card = selectedCard;

        lastPulledCard = newCard;

        StartCoroutine(removeCard(selectedCard));
    }

    public void UpdatePulls(TwitchUser user)
    {
        List<CardSO> sortedCards = new List<CardSO>();

        //sort the list based off of Rarity
        for (int i = 0; i < topCards.Count; i++)
        {
            var highestCard = nothingCard;
            highestCard.rarity = Rarity.Nothing;

            foreach (CardSO card in topCards)
            {
                if (isMoreRare(card.rarity, highestCard.rarity) && countCards(card, topCards) > countCards(card, sortedCards))
                {
                    highestCard = card;
                }
            }
            sortedCards.Add(highestCard);
        }

        topCards = sortedCards;

        foreach (CardSO card in sortedCards)
        {
            print(card.name);
        }

        DisplayCards(sortedCards);
        //display the top 5 cards.
    }

    private void DisplayCards(List<CardSO> topCards)
    {
        int cardsToShow = 5;

        foreach (Transform child in TopCardsUILocation)
        {
            Destroy(child.gameObject);
        }

        //getting top x amount of cards
        List<CardSO> cardsOnScreen = topCards.Take(cardsToShow).ToList();
        
        foreach (CardSO card in cardsOnScreen)
        {
            GameObject newCardToShow = Instantiate(cardPrefab, TopCardsUILocation);
            newCardToShow.GetComponent<CardPrefabScript>().Card = card;
        }
    }

    public int countCards(CardSO c, List<CardSO> l)
    {
        int count = 0;
        foreach (CardSO e in l)
        {
            if (e == c) count++;
        }
        return count;
    }

    private IEnumerator removeCard(CardSO currentCard)
    {
        yield return new WaitForSeconds(60);

        Destroy(lastPulledCard);
    }

    private bool isMoreRare(Rarity A, Rarity B)
    {
        if(B == Rarity.Nothing) return true;

        switch (A)
        {
            case Rarity.Common:
                {
                    return false;
                }
            case Rarity.Uncommon:
                {
                    if(B == Rarity.Common)
                        { return true; }
                    else {  return false; }
                }
            case Rarity.Rare:
                {
                    if(B == Rarity.Common || B == Rarity.Uncommon)
                        { return true; }
                    else
                        { return false; }
                }
            case Rarity.Epic:
                {
                    if (B == Rarity.Common || B == Rarity.Uncommon || B == Rarity.Rare)
                        { return true; }
                    else { return false; }
                }
            case Rarity.Legendary:
                {
                    if(B != Rarity.Legendary)
                    {
                        return true;
                    }
                    else { return false; }
                }
             default:
                {
                    return false;
                }
        }
    }
}
