using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UiManager : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu;

    [SerializeField] TMP_InputField _timerInputText;
    [SerializeField] TMP_InputField _eventInputText;

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

        string timerText = _timerInputText.text.Trim();
        string eventText = _eventInputText.text.Trim();

        if(float.TryParse(timerText, out float parsedTimer))
        {
            twitchTimer.timerLength = parsedTimer;
        }
        else
        {
            Debug.Log("Invalid Input for timer");
        }

        if (float.TryParse(eventText, out float parsedEvent))
        {
            twitchTimer.eventCountdown = parsedEvent;
        }
        else
        {
            Debug.Log("Invalid Input for timer");
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
