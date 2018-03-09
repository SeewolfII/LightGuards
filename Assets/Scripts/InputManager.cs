using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public static InputManager Get { get { return Instance; } }

    public static int PlayerCount;

    // keyboard 1 buttons
    public KeyCode k1Jump       = KeyCode.W;
    private KeyCode k1Left      = KeyCode.A;
    private KeyCode k1Right     = KeyCode.D;
    private KeyCode k1Swap      = KeyCode.S;
    private KeyCode k1Cancel    = KeyCode.Escape;
    
    // keyboard 2 buttons
    public KeyCode k2Jump       = KeyCode.UpArrow;
    private KeyCode k2Left      = KeyCode.LeftArrow;
    private KeyCode k2Right     = KeyCode.RightArrow;
    private KeyCode k2Swap      = KeyCode.DownArrow;
    private KeyCode k2Cancel    = KeyCode.P;
    
    // controller buttons
    public KeyCode cJump        = KeyCode.Joystick1Button0;
    private KeyCode cLeft       = KeyCode.Joystick1Button2;
    private KeyCode cRight      = KeyCode.Joystick1Button1;
    private KeyCode cSwap       = KeyCode.Joystick1Button3;
    private KeyCode cCancel     = KeyCode.Joystick1Button7;

    public Dictionary<string, KeyCode> PlayerSet = new Dictionary<string, KeyCode>();

    private Dictionary<string, KeyCode> KeyboardSet1 = new Dictionary<string, KeyCode>();
    private Dictionary<string, KeyCode> KeyboardSet2 = new Dictionary<string, KeyCode>();
    private Dictionary<string, KeyCode> ControllerSet = new Dictionary<string, KeyCode>();

    private bool hasCInput, hasK1Input, hasK2Input;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        KeyboardSet1.Add("Jump",    k1Jump);
        KeyboardSet1.Add("Left",    k1Left);
        KeyboardSet1.Add("Right",   k1Right);
        KeyboardSet1.Add("Swap",    k1Swap);
        KeyboardSet1.Add("Cancel",  k1Cancel);

        KeyboardSet2.Add("Jump",    k2Jump);
        KeyboardSet2.Add("Left",    k2Left);
        KeyboardSet2.Add("Right",   k2Right);
        KeyboardSet2.Add("Swap",    k2Swap);
        KeyboardSet2.Add("Cancel",  k2Cancel);

        ControllerSet.Add("Jump",   cJump);
        ControllerSet.Add("Left",   cLeft);
        ControllerSet.Add("Right",  cRight);
        ControllerSet.Add("Swap",   cSwap);
        ControllerSet.Add("Cancel", cCancel);
    }

    public void AddToPlayerSet(KeyCode _key)
    {
        if (PlayerCount >= 2)
            return;

        if (_key == k1Jump && !hasK1Input)
        {
            hasK1Input = true;
            PlayerCount++;

            foreach (KeyValuePair<string, KeyCode> pair in KeyboardSet1)
                PlayerSet.Add(pair.Key + PlayerCount, KeyboardSet1[pair.Key]);
        }
        else if (_key == k2Jump && !hasK2Input)
        {
            hasK2Input = true;
            PlayerCount++;

            foreach (KeyValuePair<string, KeyCode> pair in KeyboardSet2)
                PlayerSet.Add(pair.Key + PlayerCount, KeyboardSet2[pair.Key]);
        }
        else if (_key == cJump && !hasCInput)
        {
            hasCInput = true;
            PlayerCount++;

            foreach (KeyValuePair<string, KeyCode> pair in ControllerSet)
                PlayerSet.Add(pair.Key + PlayerCount, ControllerSet[pair.Key]);
        }
    }
}