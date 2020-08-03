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
    public Color[] _colors;
    public bool mgled;
    public bool mgling;
    string solved = "Solved";

    private string text;
    public List<string> wordList = new List<string>
    {
        "AH", "AT", "AM", "AS", "AN", "BE", "BY", "GO", "IF", "IN", "IS", "IT", "MU", "NU", "NO", "NU", "OF", "PI", "TO", "UP", "US", "WE", "XI",
        "ACE", "AIM", "AIR", "BED", "BOB", "BUT", "BUY", "CAN", "CAT", "CHI", "CUT", "DAY", "DIE", "DOG", "DOT", "EAT", "EYE", "FOR", "FLY", "GET",
        "GUT", "HAD", "HAT", "HOT", "ICE", "LIE", "LIT", "MAD", "MAP", "MAY", "NEW", "NOT", "NOW", "ONE", "PAY", "PHI", "PIE", "PSI", "RED", "RHO",
        "SAD", "SAY", "SEA", "SEE", "SET", "SIX", "SKY", "TAU", "THE", "TOO", "TWO", "WHY", "WIN", "YES", "ZOO", "ALFA", "BETA",  "BLUE", "CHAT",
        "CYAN", "DEMO", "DOOR", "EAST", "EASY", "EACH", "EDIT", "FAIL", "FALL", "FIRE", "FIVE", "FOUR", "GAME", "GOLF", "GRID", "HARD", "HATE", "HELP",
        "HOLD", "IOTA", "KILO", "LIMA", "LIME", "LIST", "LOCK", "LOST", "STOP", "TEST", "TIME", "TREE", "TYPE", "WEST", "WIRE", "WOOD", "XRAY", "YELL", "ZERO",
        "ZETA", "ZULU", "ABORT", "ABOUT", "ALPHA", "BLACK", "BRAVO", "CLOCK", "CLOSE", "COULD", "CRASH", "DELTA", "DIGIT", "EIGHT", "GAMMA", "GLASS",
    "GREEN", "GUESS", "HOTEL", "INDIA", "KAPPA", "LATER", "LEAST", "LEMON", "MONTH", "MORSE", "NORTH", "OMEGA",
    "OSCAR", "PANIC", "PRESS", "ROMEO", "SEVEN", "SIGMA", "SMASH", "SOUTH", "TANGO", "TIMER", "VOICE", "WHILE",
    "WHITE", "WORLD", "WORRY", "WOULD", "BINARY", "DEFUSE", "DISARM", "EXPERT", "FINISH", "FORGET", "LAMBDA", "MANUAL", "MODULE", "NUMBER", "ORANGE", "PERIOD",
    "PURPLE", "QUEBEC", "SHOULD", "SIERRA", "SOURCE", "STRIKE", "SUBMIT", "TWITCH", "VICTOR", "VIOLET", "WINDOW", "YELLOW", "YANKEE", "CHARLIE", "EPSILON", "EXPLODE",
    "FOXTROT", "JULIETT", "MEASURE", "MISSION", "OMICRON", "SUBJECT", "UNIFORM", "UPSILON", "WHISKEY", "DETONATE", "NOTSOLVE", "NOVEMBER"
    };
    public string[] _mLG = {"omg", "you", "pressed", "it", "ur", "so", "cool", "true", "expert", "our", "new", "hero", "wow", "poggers", "just solve it", "just solve it" };
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
        te = UnityEngine.Random.Range(0,202);
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
        Debug.LogFormat("[Binary #{0}] Selected word: {1}", _moduleID, wordList[te]);

        // This is defined by the game3
        // This is when the lights turn on
        Module.OnActivate += delegate ()
        {
            Slovo.text = wordList[te];
            active = true;
        };
    }
    void Update()
    { 
        if(!active || sol || mgling)
        {
            return;
        }
        if (text == "")
        {
            Slovo.text = wordList[te];
        }
        else
        {
            Slovo.text = "";
        }
    }
    private bool vc(string c)
    {
        GetComponent<KMSelectable>().AddInteractionPunch();
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        // Don't do anything if the module has been solved
        if (!active || sol || mgling)
            return false;
        switch (c)
        {
            case "0":
            case "1":
                text += c;       
                break;
            case "r":
                text = "";
                Slovo.text = wordList[te];
                Debug.LogFormat("[Binary #{0}] The module has been reset.", _moduleID);
                break;
        }
        return true;
    }

    private void Submit()
    {
        if (mgling)
        {
            return;
        }
        if (!vc(null))
            return;
        Debug.LogFormat("[Binary #{0}] Sent: {1}", _moduleID, text);
        string match = CheckBinary(wordList[te]);
        if (text == match)
        {
            sol = true;
            text = "";
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
            Module.HandlePass();      
            StartCoroutine(SolveAnim());
        }
        else
        {
            Module.HandleStrike();
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.Strike, transform);
            Debug.LogFormat("[Binary #{0}] Incorrect answer. Expected: {1}", _moduleID, match);
            Debug.LogFormat("[Binary #{0}] Resetting module", _moduleID);
            text = "";
            te = UnityEngine.Random.Range(0, 202);
            Debug.LogFormat("[Binary #{0}] Selected word: {1}", _moduleID, wordList[te]);
            Slovo.text = wordList[te];
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

    IEnumerator MLG()
    {
        GetComponent<KMAudio>().PlaySoundAtTransform("MLG", transform);
        mgling = true;
        for (int i = 0; i < 160; i++)
        {
            Slovo.color = _colors[i % 12];
            yield return new WaitForSeconds(0.05f);
            if (i % 10 == 0)
            {
                Slovo.text = _mLG[i/10];
            }
        }
        mgling = false;
        Slovo.text = wordList[te];
        Slovo.color = _colors[3];
    }
    IEnumerator SolveAnim()
    {
        for (int i = 0; 7 > i; i++)
        {
            yield return new WaitForSeconds(0.05f);
            Slovo.text = solved.Substring(0, i);
        }
    }
    // This is read by the Twitch Plays mod.
    // You can find more information here: https://github.com/samfun123/KtaneTwitchPlays/wiki/External-Mod-Module-Support
    private string TwitchHelpMessage = "!{0} submit 01 [submits the code 01], !{0} press useless button [pressing useless button]";

    private IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.ToLowerInvariant().Trim();
        var split = command.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
        if (split.Length == 3 && split[0].StartsWith("press") && split[1].StartsWith("useless") && split[2].StartsWith("button"))
        {
            yield return new WaitForSeconds(0.1f);
            NotSend.OnInteract();
            if (!mgled)
            {
                mgled = true;
                StartCoroutine(MLG());
            }
            else if (mgled && !mgling)
            {
                mgling = true;
                Slovo.text = "nope";
                yield return new WaitForSeconds(.5f);
                Slovo.text = wordList[te];
                mgling = false;
            }
        }
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
                yield return new WaitForSeconds(0.03f);
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

        Debug.LogFormat("[Binary #{0}] That module was autosolved. If you didn't use solve command, report about it.", _moduleID);
        var TPCoroutine = ProcessTwitchCommand("submit " + CheckBinary(wordList[te]));
            // This sends the command through the TP coroutine
            while (TPCoroutine.MoveNext())
                yield return TPCoroutine.Current;
        
    }
}
