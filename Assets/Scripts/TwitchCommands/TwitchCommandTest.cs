using System.Threading.Tasks;
using TwitchIntegration;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class TwitchCommandTest : TwitchMonoBehaviour
{
    public PackOpenScript POS;

    [TwitchCommand("Cards", "cards", "CARDS")]
    public void showCurrentCardsInChat()
    {
        string currentCards = " ";

        if (POS.sortedCards.Count > 0)
        {
            foreach (var card in POS.sortedCards)
            {
                currentCards += $" {card.name} Owned By: {card.cardOwner} \n";
            }

            TwitchManager.SendChatMessage($"These are the top current cards: \n" +
            $"{currentCards}");
            //display top cards in chat
        }
        else
        {
            TwitchManager.SendChatMessage("There are no cards drawn");
        }
    }
}
