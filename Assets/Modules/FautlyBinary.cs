using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;
using JetBrains.Annotations;


public class FautlyBinary : MonoBehaviour {
    public Color[] _colors;
    public KMSelectable[] B;
    public KMSelectable R;
    public KMSelectable Send;
    public KMSelectable Bottom;
    public KMSelectable Display;
    public TextMesh Displayed;
    public TextMesh L;
    public TextMesh Ri;
    public TextMesh Button;
    public KMBombModule module;
    public string inputs;
    public string input;
    public bool once = false;
    public int label;
    public int inpCount;
    public int color;
    public int A1;
    public int A2;
    public int A3;
    public int A4;
    public int final;
    public string finalbin;
    public int color2;
    public int BottomTime;
    public int ScreenLab;
    public int BottomLab;
    public bool screened;
    public bool pressed;
    public bool solved = false;
    public bool bruh;
    public bool screening = false;
    public float time;
    public string[] force = { "", "solved" };
    public string[] log = {"Red", "Green", "Blue", "Magenta", "Yellow", "Cyan", "White" };
    public string[] botom = { "rutton", "buttoh", "button", "buton", "batton", "buffon", "betton", "butt()n", "bruhton", "bu11on" };
    public string[] screen = { "press #", "im not sure", "submit it", "left?", "press me", "tap 1 then 0", "tap 0 then 1", "##?", "not that!", "???" };
    public string[] _table1 = {"01", "01", "10", "01", "10", "10", "10", "10", "01", "10", "10", "01", "01", "10" };
    public string[] _Table2 = { "7", "4", "9", "4", "8", "7", "4", "8", "6", "9",
        "9", "3", "1", "4", "9", "8", "4", "4", "4", "1",
        "8", "3", "2", "3", "4", "2", "9", "3", "6", "6",
        "3", "3", "0", "6", "2", "3", "9", "3", "9", "0",
        "4", "6", "1", "0", "8", "8", "3", "5", "3", "6",
        "7", "8", "9", "5", "5", "1", "2", "3", "5", "7",
        "4", "0", "0", "6", "1", "4", "9", "3", "7", "4",
        "9", "4", "6", "8", "8", "5", "9", "9", "2", "2",
        "1", "3", "1", "9", "1", "4", "9", "8", "5", "5",
        "2", "1", "9", "2", "7", "4", "5", "3", "3", "1" };
    public string[] _table3 = { "104", "90", "72", "58", "83", "43", "43", "126", "52", "97",
        "128", "122", "125", "93", "109", "121", "49", "121", "64", "67",
        "48", "112", "117", "110", "58", "63", "79", "124", "65", "90",
        "73", "51", "112", "54", "75", "38", "35", "43", "48", "43",
        "89", "98", "47", "85", "109", "106", "95", "126", "79", "118",
        "54", "104", "108", "82", "88", "68", "99", "126", "32", "90",
        "79", "85", "106", "109", "37", "37", "63", "50", "65", "91" };
    private int _moduleID;
    private static int _moduleIDCounter = 1;
    // Use this for initialization
    void Awake ()
    {
        
        ScreenLab = Random.Range(0, 10);
        BottomLab = Random.Range(0, 10);
        label = Random.Range(0, 2);
        screen[0] = "press " + label;
        screen[7] = Random.Range(10, 100) + "?";
        Displayed.text = screen[ScreenLab];
        if (!once)
        {
            _moduleID = _moduleIDCounter++;
            once = true;
        }
        color = Random.Range(0, 7);
        color2 = Random.Range(0, 7);
        Displayed.color = _colors[color];       
        L.text = label.ToString();
        Ri.text = label.ToString();
        inputs = _table1[((label * 7) + color)];
        Button.text = botom[BottomLab];
        BottomTime = Int32.Parse(_Table2[(BottomLab * 10) + ScreenLab]);
        A1 = Int32.Parse(_table3[color * 10 + ScreenLab]);
        A2 = Int32.Parse(_table3[color2 * 10 + BottomLab]);
        A3 = Int32.Parse(_table3[color2 * 10 + ScreenLab]);
        A4 = Int32.Parse(_table3[color * 10 + BottomLab]);
        final = ((A1 + A2 + A3 + A4) % 256);
        finalbin = Convert.ToString(final, 2).PadLeft(8, '0');
        Debug.LogFormat("[Faulty Binary #{0}] Left button is " + inputs.Substring(0, 1) + ", right button is " + inputs.Substring(1, 1), _moduleID);
        Debug.LogFormat("[Faulty Binary #{0}] Displayed text is " + screen[ScreenLab], _moduleID);
        Debug.LogFormat("[Faulty Binary #{0}] Text of start button is " + botom[BottomLab], _moduleID);
        Debug.LogFormat("[Faulty Binary #{0}] Colors of displayed word was " + log[color] + " and " + log[color2], _moduleID);
        Debug.LogFormat("[Faulty Binary #{0}] Press start button when last digit of the timer is " + BottomTime, _moduleID);
    }
    void Start () {
        Display.OnInteract += delegate { StartCoroutine (DisplayPressed()); return false; };
        Bottom.OnInteract += delegate { StartCoroutine(BottomPressed()); return false; };
        B[0].OnInteract += delegate { Prep("L"); return false; };
        B[1].OnInteract += delegate { Prep("R"); return false; };
        R.OnInteract += delegate { StartCoroutine(Reseted());return false; };
        Send.OnInteract += delegate { StartCoroutine(Sended()); return false; };
    }
	
	// Update is called once per frame
	void Update () {
        time = (int)GetComponent<KMBombInfo>().GetTime() % 10;
    }
    void Prep(string c)
    {
        if (!bruh || inpCount >= 8 || solved)
        {
            return;
        }
        Displayed.text += label.ToString();
        inpCount += 1;
        GetComponent<KMSelectable>().AddInteractionPunch();
        GetComponent<KMAudio>().PlaySoundAtTransform("ButtonPressed", transform);
        switch (c)
        {
            case "L":
                input += inputs.Substring(0, 1);
                break;
            case "R":
                input += inputs.Substring(1, 1);
                break;
         }
    }
    IEnumerator Reseted()
    {
        if (!bruh || solved)
        {
            yield break;
        }
        GetComponent<KMAudio>().PlaySoundAtTransform("ButtonPressed", transform);
        GetComponent<KMSelectable>().AddInteractionPunch();
        Displayed.text = "";
        input = "";
        inpCount = 0;
        yield break;
    }
    IEnumerator DisplayPressed()
    {

        if (screened || solved)
        {
            yield break;
        }
        GetComponent<KMSelectable>().AddInteractionPunch();
        screened = true;
        screening = true;
        GetComponent<KMAudio>().PlaySoundAtTransform("screen", transform);
        for (int i = 0; i < 69; i++)
        {
            yield return new WaitForSeconds(0.01f);
            Displayed.color = _colors[Random.Range(0,7)];
        }
        Displayed.color = _colors[color2];
        screening = false;
    }
    IEnumerator BottomPressed()
    {
        if (!screened || bruh || solved || screening)
        {
            yield break;
        }
        GetComponent<KMSelectable>().AddInteractionPunch();
        Debug.LogFormat("[Faulty Binary #{0}] You pressed start button when last digit of the timer is " + time, _moduleID);
        if (time == BottomTime)
        {
            Debug.LogFormat("[Faulty Binary #{0}] That's correct, it's time to final input", _moduleID);
            Debug.LogFormat("[Faulty Binary #{0}] Values from table 3 are: " + A1 + ", " + A2 + ", " + A3 + ", " + A4, _moduleID);
            Debug.LogFormat("[Faulty Binary #{0}] Total answer is " + final + " which " + finalbin + " in binary", _moduleID);          
            GetComponent<KMAudio>().PlaySoundAtTransform("Bottom", transform);
            bruh = true;
            Displayed.text = "";
            Displayed.color = _colors[6];
        }
        else
        {
            Debug.LogFormat("[Faulty Binary #{0}] That's incorrect, try again", _moduleID);
            module.HandleStrike();
        }
    }
    IEnumerator Sended()
    {
        if (!bruh || inpCount != 8 || solved)
        {
            yield break;
        }
        GetComponent<KMSelectable>().AddInteractionPunch();
        GetComponent<KMAudio>().PlaySoundAtTransform("NextStage", transform);
        Displayed.text = input;
        yield return new WaitForSeconds(1.0f);
        Debug.LogFormat("[Faulty Binary #{0}] You submitted " + input, _moduleID);
        if (input == finalbin)
        {
            Debug.LogFormat("[Faulty Binary #{0}] That's correct, module solved!", _moduleID);
            solved = true;
            Displayed.color = _colors[1];
            GetComponent<KMAudio>().PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
            module.HandlePass();
        }
        else
        {
            Debug.LogFormat("[Faulty Binary #{0}] That's incorrect, resetting module...", _moduleID);
            Displayed.color = _colors[0];
            Displayed.text = "try again";
            module.HandleStrike();
            screened = false;
            bruh = false;
            inpCount = 0;
            input = "";
            yield return new WaitForSeconds(1.0f);
            Awake();
        }
    }
    private string TwitchHelpMessage = "!{0} press screen/screen [to press on screen] !{0} start on #[pressing start button at specific time] !{0} submit llllrrrr [pressing left/right input buttons] !{0} reset [to reset your input]";
    private IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.ToLowerInvariant().Trim();
        var split = command.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
        if ((split.Length == 2 && split[0].StartsWith("press") && split[1].StartsWith("screen")) || (split.Length == 1 && split[0].StartsWith("screen")))
        {
            yield return new WaitForSeconds(0.05f);
            Display.OnInteract();
            yield return false;
        }
        else if (split.Length == 3 && split[0].StartsWith("start") && split[1].StartsWith("on"))
        {
            if (!screened || solved || bruh || split[2].Length != 1 || split[2].Any(letter => !letter.EqualsAny ('0', '1', '2', '3', '4', '5', '6', '7', '8', '9')))
            {
                yield break;
            }
            pressed = false;
            int tptime = Int32.Parse(split[2]);
            while (!pressed)
            {
                if (time == tptime)
                {
                    Bottom.OnInteract();
                    pressed = true;
                }
                yield return new WaitForSeconds(.1f);
            }
        }
        else if (split.Length == 1 && split[0].StartsWith("reset"))
        {
            yield return new WaitForSeconds(1.0f);
            R.OnInteract();
            yield return null;
        }
        else if (split.Length == 2 && split[0].StartsWith("submit"))
        {
            string code = split[1]; 
            if (code.Any(letter => letter.EqualsAny('l', 'r')) && code.Length < 8)
            {
                yield return "sendtochat Sorry, there is not enough digits! If your answer lower than 10000000, add zeros before your number to make 8 digits.";
            }
            else if (code.Any(letter => letter.EqualsAny('l', 'r')) && code.Length == 8)
                foreach (char c in code)
                {
                    yield return new WaitForSeconds(.1f);
                    if (c == 'l')
                    {
                        B[0].OnInteract();
                    }
                    else if (c == 'r')
                    {
                        B[1].OnInteract();
                    }
                }
            Send.OnInteract();
            yield return null;
        }
    }
    private IEnumerator TwitchHandleForcedSolve()
    {
        Debug.LogFormat("[Faulty Binary #{0}] That module was autosolved. If you didn't use solve command please report about it.", _moduleID);
        Displayed.text = "solved";
        GetComponent<KMAudio>().PlaySoundAtTransform("ForceSolve", transform);
        module.HandlePass();
        screened = true;
        pressed = true;
        solved = true;
        for (int i = 0; i < 40; i++)
        {
            yield return new WaitForSeconds(0.05f);
            Displayed.text = force[i%2];
        }
    }
    }
