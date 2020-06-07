using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Binary : MonoBehaviour {

    public KMBombModule Module;
    public KMAudio Audio;
    public KMSelectable B0;
    public KMSelectable B1;
    public KMSelectable Reset;
    public KMSelectable Send;
    public KMSelectable NotSend;
    public TextMesh Slovo;

    private string text;
    private List<string> words = new List<string>
    {
        "Finish", "Strike", "Solve", "Disarm", "NotSolve"
    };
    private bool active;
    private int te;
    private bool sol;

    // Static means this variable will be the same for every module
    // It will increase by one for every module activated
    private static int _moduleIDCounter = 1;
    private int _moduleID;

    void Awake () {
        // This is for logging
        _moduleID = _moduleIDCounter++;
        text = "";
        Slovo.text = "";
        te = UnityEngine.Random.Range(1, 6) - 1;
        B0.OnInteract += delegate ()
        {
            vc("0");
            return false;
        };
        B1.OnInteract += delegate ()
        {
            vc("1");
            return false;
        };
        Reset.OnInteract += delegate ()
        {
            vc("r");
            return false;
        };
        Send.OnInteract += delegate ()
        {
            Submit();
            return false;
        };
        NotSend.OnInteract += delegate ()
        {
            vc(null);
            return false;
        };
        Debug.LogFormat("[Binary #{0}] Selected word: {1}", _moduleID, words[te]);

        // This is defined by the game
        // This is when the lights turn on
        Module.OnActivate += delegate ()
        {
            Slovo.text = words[te];
            active = true;
        };
    }

    private bool vc(string c)
    {
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        // Don't do anything if the module has been solved
        if (!active || sol)
            return false;
        switch (c)
        {
            case "0":
            case "1":
                text += c;
                break;
            case "r":
                text = "";
                Debug.LogFormat("[Binary #{0}] The module has been reset.", _moduleID);
                break;
        }
        return true;
    }

    private void Submit()
    {
        if (!vc(null))
            return;
        Debug.LogFormat("[Binary #{0}] Sent: {1}", _moduleID, text);
        string match = CheckBinary(words[te]);
        if (text == match)
        {
            text = "";
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
            Module.HandlePass();
            Slovo.text = "";
            sol = true;
        }
        else
        {
            Module.HandleStrike();
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.Strike, transform);
            Debug.LogFormat("[Binary #{0}] Expected: {1}", _moduleID, match);
            text = "";
        }
    }

    private string CheckBinary(string word)
    {
        var binary = "";
        foreach (char c in word)
        {
            binary += Convert.ToString(c, 2).PadLeft(8, '0');
        }
        return binary;
    }

    // This is read by the Twitch Plays mod.
    // You can find more information here: https://github.com/samfun123/KtaneTwitchPlays/wiki/External-Mod-Module-Support
    private string TwitchHelpMessage = "!{0} submit 01 [submits the code 01]";
    
    private IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.ToLowerInvariant();
        var split = command.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
        if (split.Length >= 2 && split[0].StartsWith("submit"))
        {
            var code = split.Skip(1).Join("");
            if (code.Any(letter => !letter.EqualsAny('0', '1')))
                yield break;
            // Let TP know we're about to send an input
            // This needs to be done before every input
            yield return null;
            // Make sure the screen is empty before accepting a command
            yield return Reset.OnInteract();

            foreach (var letter in code)
            {
                yield return null;
                KMSelectable button = null;
                switch (letter)
                {
                    case '0':
                        button = B0;
                        break;
                    case '1':
                        button = B1;
                        break;
                }
                yield return button.OnInteract();
            }

            yield return null;
            yield return Send.OnInteract();
        }
    }

    private IEnumerator TwitchHandleForcedSolve()
    {
        var TPCoroutine = ProcessTwitchCommand("submit " + CheckBinary(words[te]));
        // This sends the command through the TP coroutine
        while (TPCoroutine.MoveNext())
            yield return TPCoroutine.Current;
    }
}
