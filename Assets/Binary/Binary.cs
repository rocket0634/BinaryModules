using System.Collections;
using System.Collections.Generic;
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
    public TextMesh text;
    public int step;
    public int te;
    public int sol;

    private static int _moduleIDCounter = 1;
    private int _moduleID;

    void Awake () {
        _moduleID = _moduleIDCounter++;
        text.text = "";
        step = 1;
        te = Random.Range(1, 6);
        B0.OnInteract += delegate ()
        {
            vc();
            return false;
        };
        B1.OnInteract += delegate ()
        {
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            text.text += "1";
            Debug.LogFormat("[Binary #{0}] {1}", _moduleID, text.text);
            return false;
        };
        Reset.OnInteract += delegate ()
        {
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            text.text = "";
            Debug.LogFormat("[Binary #{0}] The module has been reset", _moduleID);
            return false;
        };
        Send.OnInteract += delegate ()
        {
            if (sol == 0)
            {
                if (step == 1)
                {
                    if (te == 1)
                    {
                        if (text.text == "010001100110100101101110011010010111001101101000")
                        {
                            text.text = "";
                            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
                            Module.HandlePass();
                            Slovo.text = "";
                            sol = 1;
                        }
                        else
                        {
                            Module.HandleStrike();
                            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.Strike, transform);
                            text.text = "";
                        }
                    }
                    else if (te == 2)
                    {
                        if (text.text == "011000110110000101101110011000110110010101101100")
                        {
                            text.text = "";
                            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
                            Module.HandlePass();
                            Slovo.text = "";
                            sol = 1;
                        }
                        else
                        {
                            Module.HandleStrike();
                            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.Strike, transform);
                            text.text = "";
                        }
                    }
                    if (te == 3)
                    {
                        if (text.text == "0101001101101111011011000111011001100101")
                        {
                            text.text = "";
                            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
                            Module.HandlePass();
                            Slovo.text = "";
                            sol = 1;
                        }
                        else
                        {
                            Module.HandleStrike();
                            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.Strike, transform);
                            text.text = "";
                        }
                    }
                    if (te == 4)
                    {
                        if (text.text == "010001000110100101110011011000010111001001101101")
                        {
                            text.text = "";
                            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
                            Module.HandlePass();
                            Slovo.text = "";
                            sol = 1;
                        }
                        else
                        {
                            Module.HandleStrike();
                            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.Strike, transform);
                            text.text = "";
                        }
                    }
                    if (te == 5)
                    {
                        if (text.text == "010011100110111101110100010011100110111101110100")
                        {
                            text.text = "";
                            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
                            Module.HandlePass();
                            Slovo.text = "";
                            sol = 1;
                        }
                        else
                        {
                            Module.HandleStrike();
                            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.Strike, transform);
                            text.text = "";
                        }
                    }
                }
                else if (sol > 0)
                {
                    Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
                }
            }
            return false;
        };
        NotSend.OnInteract += delegate ()
        {
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            return false;
        };
        if (step == 1)
        {
            if (te == 1)
                Slovo.text = "Finish";
            if (te == 2)
                Slovo.text = "Strike";
            if (te == 3)
                Slovo.text = "Solve";
            if (te == 4)
                Slovo.text = "Disarm";
            if (te == 5)
                Slovo.text = "NotSolve";
        }
    }

    private void vc()
    {
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        text.text += "0";
        Debug.LogFormat("[Binary #{0}] {1}", _moduleID, text.text);
    }
}
