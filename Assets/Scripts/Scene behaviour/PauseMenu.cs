using UnityEngine;
using System.Collections;

//This is messy as shit, but if someone knows a better way - inform me, please
public class PauseMenu : MonoBehaviour
{
    public GUISkin skin;

    public Vector2 pauseMenuSize;
    public Vector2 controlsMenuSize;
    public Vector2 remapMenuSize;

    private Vector2 screenCenter;
    private Rect pauseMenuRect;
    private Rect controlsMenuRect;
    private Rect remapMenuRect;

    public static bool paused = false;
    enum Window
    {
        Main,
        Controls,
        Remap
    }
    private Window window;

    private string o_width;
    private string o_height;
    private bool fullscreen;

    private int inputIterator = 0;
    private string playerToRemap;
    private bool remapping = false;

    void Start ()
    {
        Screen.showCursor = false;
        AudioListener.volume = PlayerPrefs.GetFloat("Volume", 1f);
        screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        pauseMenuRect = new Rect(screenCenter.x - (pauseMenuSize.x / 2f), screenCenter.y - (pauseMenuSize.y / 2f), pauseMenuSize.x, pauseMenuSize.y);
        controlsMenuRect = new Rect(screenCenter.x - (controlsMenuSize.x / 2f), screenCenter.y - (controlsMenuSize.y / 2f), controlsMenuSize.x, controlsMenuSize.y);
        remapMenuRect = new Rect(screenCenter.x - (remapMenuSize.x / 2f), screenCenter.y - (remapMenuSize.y / 2f), remapMenuSize.x, remapMenuSize.y);
        o_width = Screen.width.ToString();
        o_height = Screen.height.ToString();
        fullscreen = Screen.fullScreen;
    }

    void pauseWindow (int windowID)
    {
        AddSpikes(pauseMenuRect.width);
        FancyTop(pauseMenuRect.width);
        WaxSeal(pauseMenuRect.width, pauseMenuRect.height);

        GUILayout.BeginVertical();
        {
            GUILayout.Space(8);
            GUILayout.Label("", "Divider");
            GUILayout.Label("Resolution");
            GUILayout.BeginHorizontal(GUILayout.Height(1f));
            {
                GUILayout.FlexibleSpace();
                o_width = GUILayout.TextField(o_width, GUILayout.Width(pauseMenuRect.width/5f));
                o_height = GUILayout.TextField(o_height, GUILayout.Width(pauseMenuRect.width/5f));
                fullscreen = GUILayout.Toggle(fullscreen, "Fullscreen?");
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
            
            if (GUILayout.Button("Apply"))
            {
                int newX;
                int newY;
                if (int.TryParse(o_width, out newX) && int.TryParse(o_height, out newY))
                    Screen.SetResolution(newX, newY, fullscreen);
            }

            GUILayout.Label("", "Divider");

            GUILayout.BeginHorizontal(GUILayout.Height(1f));
            {
                GUILayout.Label("Volume", "ShortLabel");
                GUILayout.BeginVertical();
                {
                    GUILayout.FlexibleSpace();
                    GUI.SetNextControlName("Volume");
                    AudioListener.volume = GUILayout.HorizontalSlider(AudioListener.volume, 0, 2f);
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            GUILayout.Label("", "Divider");

            if (GUILayout.Button("Controls"))
            {
                window = Window.Controls;
            }

            if (GUILayout.Button("Resume"))
            {
                PlayerPrefs.SetFloat("Volume", AudioListener.volume);
                Time.timeScale = 1;
                Screen.showCursor = false;
                paused = false;
            }

            if (GUILayout.Button("Exit"))
            {
                PlayerPrefs.SetFloat("Volume", AudioListener.volume);
                Application.Quit();
            }
        }
        GUILayout.EndVertical();
    }

    void controlsWindow (int windowID)
    {
        GUILayout.BeginVertical();
        {
            GUILayout.Space(40f);
            GUILayout.BeginHorizontal(GUILayout.Height(1f));
            {
                if (GUILayout.Button("Player 1"))
                {
                    playerToRemap = "Player 1";
                    remapping = true;
                }
                if (GUILayout.Button("Player 2"))
                {
                    playerToRemap = "Player 2";
                    remapping = true;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(1f));
            {
                if (GUILayout.Button("Player 3"))
                {
                    playerToRemap = "Player 3";
                    remapping = true;
                }
                if (GUILayout.Button("Player 4"))
                {
                    playerToRemap = "Player 4";
                    remapping = true;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Label("", "Divider");

            if (GUILayout.Button("Return"))
            {
                window = Window.Main;
            }
        }
        GUILayout.EndVertical();
    }

    void remapWindow (int windowID)
    {
        GUILayout.BeginVertical();
        {
            GUILayout.Space(50f);
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("Press the key corresponding to", "PlainText");
                GUILayout.Label(PlayerInputComponent.inputNames[inputIterator], "LegendaryText");
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    void Update()
    {
        if (!paused && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            Screen.showCursor = true;
            paused = true;
            window = Window.Main;
        }
        else if (paused && Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerPrefs.SetFloat("Volume", AudioListener.volume);
            Time.timeScale = 1;
            Screen.showCursor = false;
            paused = false;
        }
    }

    void OnGUI()
    {
        if (paused)
        {
            GUI.skin = skin;
            pauseMenuRect = GUI.Window((int)Window.Main, pauseMenuRect, pauseWindow, "");
            if (window == Window.Controls)
            {
                controlsMenuRect = GUI.Window((int)Window.Controls, controlsMenuRect, controlsWindow, "");
                GUI.BringWindowToFront((int)Window.Controls);
                if (remapping)
                {
                    remapMenuRect = GUI.Window((int)Window.Controls + 1, remapMenuRect, remapWindow, "");
                    GUI.BringWindowToFront((int)Window.Controls+1);
                    if (Event.current.isKey && Event.current.type == EventType.KeyUp)
                    {

                        PlayerPrefs.SetInt(playerToRemap + " " + PlayerInputComponent.inputNames[inputIterator], (int)Event.current.keyCode);
                        PlayerInputComponent.mappings[playerToRemap + " " + PlayerInputComponent.inputNames[inputIterator++]] = Event.current.keyCode;
                        if (inputIterator >= PlayerInputComponent.inputNames.Length)
                        {
                            inputIterator = 0;
                            remapping = false;
                        }
                    }
                }
            }
        }
    }

    #region Decorations
    void AddSpikes(float winX)
    {
        int spikeCount = Mathf.FloorToInt(winX - 152f) / 22;
        GUILayout.BeginHorizontal();
        GUILayout.Label("", "SpikeLeft");//-------------------------------- custom
        for (int i = 0; i < spikeCount; i++)
        {
            GUILayout.Label("", "SpikeMid");//-------------------------------- custom
        }
        GUILayout.Label("", "SpikeRight");//-------------------------------- custom
        GUILayout.EndHorizontal();
    }

    void FancyTop(float topX)
    {
        float leafOffset = (topX / 2f) - 64f;
        float frameOffset = (topX / 2f) - 27f;
        float skullOffset = (topX / 2f) - 20f;
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
        float FrameOffsetX = x + 3f;
        float SkullOffsetX = x + 10f;
        float RibbonOffsetY = y + 22f;
        float FrameOffsetY = y;
        float SkullOffsetY = y + 9f;

        GUI.Label(new Rect(RibbonOffsetX, RibbonOffsetY, 0, 0), "", "RibbonRed");//-------------------------------- custom	
        GUI.Label(new Rect(FrameOffsetX, FrameOffsetY, 0, 0), "", "IconFrame");//-------------------------------- custom	
        GUI.Label(new Rect(SkullOffsetX, SkullOffsetY, 0, 0), "", "Skull");//-------------------------------- custom	
    }
    #endregion
}
