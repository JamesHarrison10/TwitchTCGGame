using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UiManager : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu;

    [SerializeField] TextMeshProUGUI _timerInputText;
    [SerializeField] TextMeshProUGUI _eventInputText;

    public TwitchTimer twitchTimer;

    public void ConnectGame()
    {
        updateTimer();
    }

    public void updateTimer()
    {
        if(_timerInputText == null || _eventInputText == null)
        {
            return;
        }

        string timerText = _timerInputText.text;
        int timerInput;

        if(int.TryParse(timerText, out timerInput))
        {
            twitchTimer.timerLength = timerInput;
        }
        else
        {
            Debug.LogError("Parsed Interger Failed");
        }

        string eventText = _eventInputText.text;
        int eventInput;

        if(int.TryParse(eventText, out eventInput))
        {
            twitchTimer.eventCountdown = eventInput;
        }
        else
        {
            Debug.LogError("Parsed Interger Failed");
        }
    }


    private void Update()
    {
        if (Application.isFocused)
        {
            settingsMenu.SetActive(true);
        }
        else
        {
            settingsMenu.SetActive(false);
        }
    }
}
