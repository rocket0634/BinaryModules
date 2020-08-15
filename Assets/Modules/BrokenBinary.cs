using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BrokenBinary : MonoBehaviour
{



    public KMBombModule Module;
    public KMSelectable B0;
    public KMSelectable B1;
    public KMSelectable Reset;
    public KMSelectable Send;
    public KMSelectable NotSend;
    public int Order;
    public Color[] Colors;
    public string ordered = "01234";
    public string disordered = "00000";
    public int pressed;
    public TextMesh Slovo;
    public TextMesh Left;
    public TextMesh Right;
    public TextMesh Middle;
    public TextMesh Bottom;
    public TextMesh Up;
    public string CL;
    public string CM;
    public string CR;
    public string[] chars;
    public string word;
    public Color red;
    public Color green;
    public int stepcycle = -1;
    public string input;
    public bool c1 = false;
    public bool c2 = false;
    public bool c3 = false;
    public bool c4 = false;
    public bool c5 = false;
    public bool solvedd = false;
    public bool cheater = false;
    public int shift;
    public bool awaked = false;
    public char shifted;
    Random rnd = new Random();

    public string[] WordList = {
    "ABORT", "ABOUT", "ALERT", "ALPHA", "ALTER", "BLACK", "BASTE", "BEAST", "BEATS", "BLOCK", "BRAVO", "CLOCK", "CLOSE",
    "CHAIR", "CRASH", "DEALT", "DELTA", "DIGIT", "DISCO", "EIGHT", "EVENS", "GAMMA", "GENRE", "GHOST", "GIANT", "GLASS",
    "GOTHS", "GREEN", "GUESS", "HOTEL", "HYPER", "INDIA", "KAPPA", "LATER", "LEMON", "LUCKY", "MAGMA", "MARIO", "MELON",
    "MERIT", "MONTH", "MORSE", "NORTH", "OMEGA", "OSCAR", "PANIC", "PRESS", "PURSE", "ROMEO", "SEVEN", "SHOUT", "SIGMA",
    "SIMON", "SMASH", "SOUTH", "STICK", "SUPER", "TANGO", "THORN", "TICKS", "TIMER", "ULTRA", "VOICE", "WHILE", "WHITE",
    "WORLD", "WORRY", "WOULD"};


    private static int _moduleIDCounter = 1;
    private int _moduleID;

    void Awake()
    {

        Module.OnActivate += delegate ()
        {
            
            _moduleID = _moduleIDCounter++;
            awaked = true;
            word = WordList[Random.Range(0, 68)];
            for (int i = 0; i < 5; i++)
            {
                char c = word[i];
                chars[i] = Convert.ToString(c, 2).PadLeft(8, '0');

            }
            StartCoroutine(scramble());
        };

    }

    void Start()
    {
        if (!awaked)
        {
            return;
        }
        CL = chars[Int32.Parse(disordered[0].ToString())];
        CM = chars[Int32.Parse(disordered[1].ToString())];
        CR = chars[Int32.Parse(disordered[2].ToString())];
        if (!c4)
        {
            Up.text = chars[Int32.Parse(disordered[3].ToString())];
        }
        if (!c5)
        {
            Bottom.text = chars[Int32.Parse(disordered[4].ToString())];
        }
        if (pressed != 5)
        {
            StartCoroutine(cycle());
        }
        B0.OnInteract += delegate ()
        {
            if (!c1)
            {
                c1 = true; Left.text = ""; vc(0); return false;
            }
            else
            {
                return false;
            }
        };
        B1.OnInteract += delegate ()
        {
            if (!c2)
            {
                c2 = true; Middle.text = ""; vc(1); return false;
            }
            else
            {
                return false;
            }
        };
        Reset.OnInteract += delegate ()
        {
            if (!c3)
            {
                c3 = true; Right.text = ""; vc(2); return false;
            }
            else
            {
                return false;
            }
        };
        Send.OnInteract += delegate ()
        {
            if (!c4)
            {
                c4 = true; Up.text = ""; vc(3); return false;
            }
            else
            {
                return false;
            }
        };
        NotSend.OnInteract += delegate ()
        {
            if (!c5)
            {
                c5 = true; Bottom.text = ""; vc(4); return false;
            }
            else
            {
                return false;
            }
        };


    }
    void vc(int c)
    {
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        GetComponent<KMSelectable>().AddInteractionPunch();
        switch (c)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                Order = (Int32.Parse(disordered[2].ToString()) - shift) % 5;
                input += word[Int32.Parse(disordered[c].ToString())];
                pressed += 1;
                if (pressed == 5)
                {
                    StartCoroutine(check());
                }
                break;
        }
    }
    IEnumerator scramble()
    {
        Debug.LogFormat("[Broken Binary #{0}] Picked up word is " + word, _moduleID);
        yield return null;
        char[] array = ordered.ToCharArray();
        disordered = ordered;
        while (disordered == ordered)
        {
            System.Random rng = new System.Random();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            string scram = new string(array);
            disordered = scram;
        }
        Debug.LogFormat("[Broken Binary #{0}] Left button is spelling " + word[Int32.Parse(disordered[0].ToString())], _moduleID);
        Debug.LogFormat("[Broken Binary #{0}] Middle button is spelling " + word[Int32.Parse(disordered[1].ToString())], _moduleID);
        Debug.LogFormat("[Broken Binary #{0}] Right button is spelling " + word[Int32.Parse(disordered[2].ToString())], _moduleID);
        Debug.LogFormat("[Broken Binary #{0}] Top button is spelling " + word[Int32.Parse(disordered[3].ToString())], _moduleID);
        Debug.LogFormat("[Broken Binary #{0}] Bottom button is spelling " + word[Int32.Parse(disordered[4].ToString())], _moduleID);
        Start();
    }
    IEnumerator cycle()
    {
        stepcycle += 1;
        if (stepcycle == 9)
        {
            stepcycle = 0;
        }



        if (stepcycle == 8)
        {
            Left.text = "";
            Middle.text = "";
            Right.text = "";
        }
        else
        {
            if (!c1)
            {
                Left.text = char.ToString(CL[stepcycle]);
            }
            if (!c2)
            {
                Middle.text = char.ToString(CM[stepcycle]);
            }
            if (!c3)
            {
                Right.text = char.ToString(CR[stepcycle]);
            }
        }
        yield return new WaitForSeconds(1.0f);
        Start();
    }
    IEnumerator check()
    {
        Debug.LogFormat("[Broken Binary #{0}] You submitted " + input, _moduleID);
        GetComponent<KMAudio>().PlaySoundAtTransform("NextStage", transform);
        for (int i = 0; i < 5; i++)
        {
            Slovo.text += input[i];
            yield return new WaitForSeconds(0.5f);
        }
        if (WordList.Contains(input))
        {
            if (input == word)
            {
                Debug.LogFormat("[Broken Binary #{0}] Thats correct", _moduleID);
            }
            else
            {
                Debug.LogFormat("[Broken Binary #{0}] That's a anagram word but still correct", _moduleID);
            }
                StartCoroutine(solved());
            
        }
        else if (!WordList.Contains(input))
        {
            Debug.LogFormat("[Broken Binary #{0}] Thats incorrect", _moduleID);
            Slovo.color = red;

            Module.HandleStrike();
            yield return new WaitForSeconds(1f);
            for (int i = 3; i >= -5; i--)
            {
                yield return new WaitForSeconds(0.05f);
                if (i > 0)
                {
                    Slovo.text = input.Substring(0, i);
                }
                else if (i == 0)
                {
                    Slovo.text = "";
                }
                else
                {
                    Slovo.text = word.Substring(0, Math.Abs(i));
                }
            }
            yield return new WaitForSeconds(1f);
            Slovo.color = green;
            input = "";
            disordered = "01234";
            pressed = 0;
            c1 = false;
            c2 = false;
            c3 = false;
            c4 = false;
            c5 = false;
            stepcycle = -1;
            Slovo.text = "";
            word = WordList[Random.Range(0, 68)];

            for (int i = 0; i < 5; i++)
            {
                char c = word[i];
                chars[i] = Convert.ToString(c, 2).PadLeft(8, '0');

            }
            StartCoroutine(scramble());
        }
    }
    IEnumerator star()
    {
        yield return new WaitForSeconds(0.1f);
        Awake();
    }
    IEnumerator solved()
    {
        input = "";
        GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
        Module.HandlePass();
        if (!cheater)
        {
            Debug.LogFormat("[Broken Binary #{0}] Module solved!", _moduleID);
        }
        solvedd = true;
        yield return null;
    }
    private string TwitchHelpMessage = "To press small top buttons, use !{0} L(left), M(middle), R(right). To press bottom buttons use !{0} T(top), B(bottom)";
    public IEnumerator ProcessTwitchCommand(string command)
    {
        var tokens = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length == 0 || tokens.Length >= 6) yield break;
        var indices = new List<int>();
        foreach (var token in tokens)
        {
            yield return new WaitForSeconds(0.1f);
            switch (token.ToUpperInvariant())
            {
                case "R":  Reset.OnInteract(); break;
                case "L": B0.OnInteract(); break;
                case "M": B1.OnInteract(); break;
                case "B": NotSend.OnInteract(); break;
                case "T": Send.OnInteract(); break;
                default: yield break;
            }
        }
    }
    private IEnumerator TwitchHandleForcedSolve()
    {
        c1 = true;
        c2 = true;
        c3 = true;
        c4 = true;
        c5 = true;
        Left.text = "";
        Right.text = "";
        Middle.text = "";
        Up.text = "";
        Bottom.text = "";
        cheater = true;
        Debug.LogFormat("[Broken Binary #{0}] That module was autosolved. If you didn't use solve command, report about it.", _moduleID);
        StartCoroutine(solved());
        yield return null;
    }
}