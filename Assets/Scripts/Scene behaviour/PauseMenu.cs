using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public GUISkin skin;

    bool ToggleBTN = false;

    private bool paused = false;
    private const float pauseMenuWidth = 350f;
    private const float pauseMenuHeight = 510f;
    private Rect pauseMenuRect = new Rect(Screen.width / 2f - pauseMenuWidth / 2f, Screen.height / 2f - pauseMenuHeight / 2f, pauseMenuWidth, pauseMenuHeight);

    void Start ()
    {
        //GUI.skin = skin;
    }

    void testWindow (int windowID)
    {
        AddSpikes(pauseMenuRect.width);
        FancyTop(pauseMenuRect.width);
        WaxSeal(pauseMenuRect.width, pauseMenuRect.height);

        GUILayout.BeginVertical();
        /*GUILayout.Space(8);
        GUILayout.Label("", "Divider");//-------------------------------- custom
        GUILayout.Label("Standard Label");
        GUILayout.Label("Short Label", "ShortLabel");//-------------------------------- custom
        GUILayout.Label("", "Divider");//-------------------------------- custom
        if (GUILayout.Button("Standard Button"))
            Debug.Log("Clickyclick");
        GUILayout.Button("Short Button", "ShortButton");//-------------------------------- custom
        GUILayout.Label("", "Divider");//-------------------------------- custom
        ToggleBTN = GUILayout.Toggle(ToggleBTN, "This is a Toggle Button");
        GUILayout.Label("", "Divider");//-------------------------------- custom
        GUILayout.Box("This is a textbox\n this can be expanded by using \\n");
        GUILayout.TextField("This is a textfield\n You cant see this text!!");
        GUILayout.TextArea("This is a textArea\n this can be expanded by using \\n");*/
        GUILayout.Label("", "Divider");
        if (GUILayout.Button("Exit"))
        {
            Application.Quit();
        }
        GUILayout.EndVertical();
    }

    void AddSpikes(float winX)
    {
	    int spikeCount = Mathf.FloorToInt(winX - 152f)/22;
	    GUILayout.BeginHorizontal();
	    GUILayout.Label ("", "SpikeLeft");//-------------------------------- custom
	    for (int i = 0; i < spikeCount; i++)
        {
			GUILayout.Label ("", "SpikeMid");//-------------------------------- custom
        }
	    GUILayout.Label ("", "SpikeRight");//-------------------------------- custom
	    GUILayout.EndHorizontal();
    }

    void FancyTop(float topX)
    {
	    float leafOffset = (topX/2f)-64f;
	    float frameOffset = (topX/2f)-27f;
	    float skullOffset = (topX/2f)-20f;
	    GUI.Label(new Rect(leafOffset, 18f, 0, 0), "", "GoldLeaf");//-------------------------------- custom	
	    GUI.Label(new Rect(frameOffset, 3f, 0, 0), "", "IconFrame");//-------------------------------- custom	
	    GUI.Label(new Rect(skullOffset, 12f, 0, 0), "", "Skull");//-------------------------------- custom	
    }

    void WaxSeal(float x, float y)
    {
	    float WSwaxOffsetX = x - 120f;
	    float WSwaxOffsetY = y - 115f;
	    float WSribbonOffsetX = x - 114f;
	    float WSribbonOffsetY = y - 83f;
	
	    GUI.Label(new Rect(WSribbonOffsetX, WSribbonOffsetY, 0, 0), "", "RibbonBlue");//-------------------------------- custom	
	    GUI.Label(new Rect(WSwaxOffsetX, WSwaxOffsetY, 0, 0), "", "WaxSeal");//-------------------------------- custom	
    }

    void DeathBadge(float x, float y)
    {
	    float RibbonOffsetX = x;
	    float FrameOffsetX = x+3f;
	    float SkullOffsetX = x+10f;
        float RibbonOffsetY = y + 22f;
	    float FrameOffsetY = y;
	    float SkullOffsetY = y+9f;
	
	    GUI.Label(new Rect(RibbonOffsetX, RibbonOffsetY, 0, 0), "", "RibbonRed");//-------------------------------- custom	
	    GUI.Label(new Rect(FrameOffsetX, FrameOffsetY, 0, 0), "", "IconFrame");//-------------------------------- custom	
	    GUI.Label(new Rect(SkullOffsetX, SkullOffsetY, 0, 0), "", "Skull");//-------------------------------- custom	
    }

    void Update()
    {
        if (!paused && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            paused = true;
        }
        else if (paused && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1;
            paused = false;
        }
    }

    void OnGUI()
    {
        if (paused)
        {
            GUI.skin = skin;

            pauseMenuRect = GUI.Window(0, pauseMenuRect, testWindow, "");
            GUI.BeginGroup(new Rect(0, 0, 100, 100));
            GUI.EndGroup();
        }
    }
}
