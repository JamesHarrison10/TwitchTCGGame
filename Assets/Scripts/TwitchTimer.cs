using System.Collections;
using System.Collections.Generic;
using TwitchIntegration;
using UnityEngine;

public class TwitchTimer : TwitchMonoBehaviour
{
    public float timerLength;
    public float eventCountdown;

    [SerializeField] PackOpenScript OpenScript;

    public List<TwitchUser> registeredUsers;

    private bool canJoin;

    private bool isRunning;

    private void Start()
    {
        isRunning = false;

        StartCoroutine(timerCoroutine());
    }

    [TwitchCommand("ModOpening", "mb")]
    public void ModOpening(TwitchUser user)
    {
        if(user.mod == "1" || user.displayname == "MrMexx")
        {
            if(isRunning == false)
            {
                StopAllCoroutines();
                AnnounceEvent();
            }
        }
        
    }

    [TwitchCommand("Join", "join", "JOIN")]
    public void JoinPacKOpening(TwitchUser user)
    {
        if (canJoin && !registeredUsers.Contains(user))
        {
            registeredUsers.Add(user);
            print($"{user.displayname} has joined");
        }
        else if (registeredUsers.Contains(user))
        {
            TwitchManager.SendChatMessage($"{user.displayname}, you are already registerd");
        }
        else
        {
            TwitchManager.SendChatMessage("No Event is currently occuring");
        }
    }

    private IEnumerator timerCoroutine()
    {
        yield return new WaitForSeconds(timerLength);

        AnnounceEvent();
    }

    private void AnnounceEvent()
    {
        isRunning = true;

        TwitchManager.SendChatMessage($"A New Card Is Up For Grabs Type !Join To Register For The Pack Opening {eventCountdown.ToString()} seconds left to enter");

        canJoin = true;

        registeredUsers.Clear();

        StartCoroutine(countDown());
    }

    private IEnumerator countDown()
    {
        //TwitchManager.SendChatMessage($"Players have {eventCountdown.ToString()} seconds left to enter");
        yield return new WaitForSeconds(eventCountdown);

        drawWinner();
    }

    private void drawWinner()
    {
        if (registeredUsers.Count > 0)
        {
            int randomIndex = Random.Range(0, registeredUsers.Count);
            TwitchUser winningUser = registeredUsers[randomIndex];

            OpenScript.OpenCard(winningUser);

            isRunning = false;

            StartCoroutine(timerCoroutine());
        }
        else
        {
            TwitchManager.SendChatMessage("No One Enetered");

            isRunning = false;

            StartCoroutine(timerCoroutine());
        }
    }
}
