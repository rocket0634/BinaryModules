using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using JetBrains.Annotations;

public class CruelBinary : MonoBehaviour
{

    public KMBombModule Module;
    public KMSelectable Read;
    public KMSelectable Send;
    public KMSelectable[] Buttons;
    public KMSelectable Reset;
    public TextMesh Word;
    public int striked;
    public string[] _Oofed = { "", "oof", "to bad", "incorrect", "strike", "sample text", "nope", "game over", "try again", "nice try", "oh no" };
    public bool read;
    public string h;
    public string[] _WordList = {"ABORT", "ABOUT", "ALPHA", "BLACK", "BRAVO", "CLOCK", "CLOSE", "COULD", "CRASH", "DELTA", "DIGIT", "EIGHT", "GAMMA", "GLASS",
    "GREEN", "GUESS", "HOTEL", "INDIA", "KAPPA", "LATER", "LEAST", "LEMON", "MONTH", "MORSE", "NORTH", "OMEGA",
    "OSCAR", "PANIC", "PRESS", "ROMEO", "SEVEN", "SIGMA", "SMASH", "SOUTH", "TANGO", "TIMER", "VOICE", "WHILE",
    "WHITE", "WORLD", "WORRY", "WOULD", "BINARY", "DEFUSE", "DISARM", "EXPERT", "FINISH", "FORGET", "LAMBDA", "MANUAL", "MODULE", "NUMBER", "ORANGE", "PERIOD",
    "PURPLE", "QUEBEC", "SHOULD", "SIERRA", "SOURCE", "STRIKE", "SUBMIT", "TWITCH", "VICTOR", "VIOLET", "WINDOW", "YELLOW", "YANKEE" };
    public string d = "000000000000000000";
    public string e = "";
    public int chars;
    public int charss;
    public int[] hey;
    public int[] ff;
    public string[] hay;
    public float a = 8;
    public int jopa = -1;
    public int total;
    public int inpud = 0;
    public bool solved = false;
    public bool awaked = false;
    public string answer;
    public string input;
    public Color red;
    public Color green;
    public bool once = false;

    // Use this for initialization
    string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static int _moduleIDCounter = 1;
    private int _moduleID;

    string[] morseLetters = { "._", "_...", "_._.", "_..", ".", ".._.", "__.",
        "....", "..", ".___", "_._", "._..", "__", "_.", "___", ".__.",
        "__._", "._.", "...", "_", ".._", "..._", ".__", "_.._", "_.__",
        "__.."};
    void Awake()
    {
        if (!once)
        {
            _moduleID = _moduleIDCounter++;
            once = true;
        }
        h = _WordList[UnityEngine.Random.Range(0, 67)];
        Debug.LogFormat("[Cruel Binary #{0}] Displayed word is " + h, _moduleID);
        Read.OnInteract += delegate {
            if (!solved && !read)
            {
                Word.text = h; read = true; GetComponent<KMSelectable>().AddInteractionPunch(); GetComponent<KMAudio>().PlaySoundAtTransform("tick", transform); return false;
            }
            else
            {
                return false;
            }
        };
        d = "";

        foreach (char c in h)
        {
            d += (morseLetters[letters.IndexOf(c, 0)]);
        }

        foreach (char c in d)
        {
            if (c == '.')
            {
                e += ((c.ToString()).Replace('.', '0'));
            }
            else if (c == '_')
            {
                e += ((c.ToString()).Replace('_', '1'));
            }
        }

        foreach (char c in e)
        {
            chars += 1;
        }
        e = e.PadRight(((chars - (chars % 8)) + 8), '0');
        foreach (char c in e)
        {
            charss += 1;
        }

        for (int charr = 0; charr < (charss / 8); charr++)
        {
            hay[charr] = e.Substring(8 * charr, (8 * (charr + 1)) - charr * 8);

        }
        for (int charr = 0; charr < charss / 8; charr++)
        {
            a = 8;
            jopa = 8;
            foreach (char c in hay[charr])
            {
                a--;
                jopa--;
                ff[jopa] = Convert.ToInt32(Int32.Parse(c.ToString()) * Mathf.Pow(2, a));
                hey[charr] += ff[jopa];
            }

        }
        for (int charr = 0; charr < charss / 8; charr++)
        {
            total += hey[charr];

        }

        answer = Convert.ToString(total % 256, 2).PadLeft(8, '0');

        Debug.LogFormat("[Cruel Binary #{0}] Answer for current stage is " + answer, _moduleID);



    }
    void Start()
    {


        Send.OnInteract += delegate { StartCoroutine(check()); return false; };
        Buttons[0].OnInteract += delegate { vc("0"); return false; };
        Buttons[1].OnInteract += delegate { vc("1"); return false; };
        Reset.OnInteract += delegate { vc("R"); return false; };


    }
    void vc(string c)
    {

        switch (c)
        {

            case "0":
            case "1":
                if (inpud == 8)
                {
                    break;
                }
                if (!read || solved)
                {
                    break;
                }
                input += c;
                inpud += 1;
                GetComponent<KMSelectable>().AddInteractionPunch();
                GetComponent<KMAudio>().PlaySoundAtTransform("tick", transform);
                break;
            case "R":
                if (!read || solved || inpud == 0)
                {
                    break;
                }
                GetComponent<KMSelectable>().AddInteractionPunch();
                GetComponent<KMAudio>().PlaySoundAtTransform("tick", transform);
                input = "";
                inpud = 0;
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!solved && read)
        {
            if (inpud > 0)
            {
                Word.text = input;
            }
            else
            {
                Word.text = h;
            }
        }
    }
    IEnumerator check()
    {
        if (!read || solved || inpud == 0)
        {
            yield break;
        }
        GetComponent<KMSelectable>().AddInteractionPunch();
        Debug.LogFormat("[Cruel Binary #{0}] You submitted " + input, _moduleID);
        if (input == answer)
        {
            Debug.LogFormat("[Cruel Binary #{0}] That's correct, module solved.", _moduleID);
            Module.HandlePass();
            GetComponent<KMAudio>().PlaySoundAtTransform("NextStage", transform);
            solved = true;
            Word.text = "";
            yield break;
        }
        else
        {
            Debug.LogFormat("[Cruel Binary #{0}] That's incorrect, resetting module...", _moduleID);
            StartCoroutine(strike());
            yield break;
        }

    }
    IEnumerator strike()
    {
        Module.HandleStrike();
        striked = UnityEngine.Random.Range(1, 11);
        Word.color = red;
        input = "";
        h = "";
        answer = "";
        e = "";
        d = "";
        chars = 0;
        charss = 0;
        a = 8;
        jopa = -1;
        total = 0;
        read = false;
        for (int i = 0; i < 7; i++)
        {
            hay[i] = "";
            hey[i] = 0;
        }
        for (int i = 0; i < 9; i++)
        {
            ff[i] = 0;
        }
        for (int i = 0; i < 16; i++)
        {
            Word.text = _Oofed[striked * (i % 2)];
            yield return new WaitForSeconds(0.075f);
        }
        inpud = 0;
        Word.color = green;
        Word.text = "1 message";
        Awake();
    }
    private string TwitchHelpMessage = "!{0} submit 01001000 [submits the code 01001000]. !{0} read [to read message].";
    private IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.ToLowerInvariant();
        var split = command.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

        if (split[0].StartsWith("read"))
        {
            if (!read)
            {
                yield return new WaitForSeconds(0.05f);
                Read.OnInteract();
                yield return false;
            }
            else
            {
                yield break;
            }
        }
        else if (split[0].StartsWith("submit") && split[1].Length == 8 && read == true && split.Length == 2)
        {
            string code = split[1];
            if (code.Any(letters => !letters.EqualsAny('0', '1')))
            {
                yield break;
            }
            foreach (var c in code)
            {
                yield return new WaitForSeconds(0.1f);
                Buttons[Int32.Parse(c.ToString())].OnInteract();
            }
            yield return new WaitForSeconds(0.1f);
            Send.OnInteract();
            yield return false;
        }
    }
    private IEnumerator TwitchHandleForcedSolve()
    {
        if (!read)
        {
            Read.OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        Reset.OnInteract();
        foreach (char c in answer)
        {
            if (c == '0')
            {
                Buttons[0].OnInteract();
            }
            else
            {
                Buttons[1].OnInteract();
            }
            yield return new WaitForSeconds(0.1f);
        }
        Send.OnInteract();
        yield return null;
    }



}