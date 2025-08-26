using TwitchIntegration;
using UnityEngine;
using TMPro;

public class TwitchCommandTest : TwitchMonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterText;

    private string textString  = " ";

    [TwitchCommand("CountUp", "c", "C")]
    public void displayTextToScreen(string chatText)
    {
        textString += "\n" + chatText;

        counterText.text = textString;  
    }
}
