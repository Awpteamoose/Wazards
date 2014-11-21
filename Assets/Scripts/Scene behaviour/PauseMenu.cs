using UnityEngine;
using System.Collections;

//This is messy, but if someone knows a better way - inform me, please
//It also probably is a lot more coupled with the rest of the code than it should be
public class PauseMenu : MonoBehaviour
{
    public GUISkin skin;

    public Vector2 pauseMenuSize;
    public Vector2 controlsMenuSize;
    public Vector2 mappingsMenuSize;
    public Vector2 remapMenuSize;

    private Rect pauseMenuRect;
    private Rect controlsMenuRect;
    private Rect mappingsMenuRect;
    private Rect remapMenuRect;

    public static bool paused = false;

    private string o_width;
    private string o_height;
    private bool fullscreen;
    
    private int inputIterator;
    private string player;
    private PlayerControl playerControl;
    private float reticleSpeed;
    private bool aimMode;

    private bool drawControls;
    private bool drawMappings;
    private bool drawRemap;

    Rect GetMenuRect(Vector2 center, Vector2 size)
    {
        return new Rect(center.x - (size.x / 2f), center.y - (size.y / 2f), size.x, size.y);
    }
    void getSizePosition()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        pauseMenuRect = GetMenuRect(screenCenter, pauseMenuSize);
        controlsMenuRect = GetMenuRect(screenCenter, controlsMenuSize);
        mappingsMenuRect = GetMenuRect(screenCenter, mappingsMenuSize);
        remapMenuRect = GetMenuRect(screenCenter, remapMenuSize);
    }

    void SetPlayer(string playername)
    {
        player = playername;
        if (GameObject.Find(player) != null)
            playerControl = GameObject.Find(player).GetComponentInChildren<PlayerControl>();
        reticleSpeed = PlayerPrefs.GetFloat(player + " Reticle Speed", 3f);
        aimMode = (PlayerPrefs.GetInt(player + " Aim Mode", 0) == 0)?false:true;
    }

    void SaveMappings()
    {
        PlayerPrefs.SetFloat(player + " Reticle Speed", reticleSpeed);
        PlayerPrefs.SetInt(player + " Aim Mode", aimMode ? 1 : 0);
        if (playerControl != null)
        {
            playerControl.castComponent.reticle_speed = reticleSpeed;
            playerControl.castComponent.altAimMode = aimMode;
        }
    }

    void Start ()
    {
        Screen.showCursor = false;
        AudioListener.volume = PlayerPrefs.GetFloat("Volume", 1f);
        o_width = Screen.width.ToString();
        o_height = Screen.height.ToString();
        fullscreen = Screen.fullScreen;
    }

    void OpenMenu()
    {
        Time.timeScale = 0;
        Screen.showCursor = true;
        paused = true;

        getSizePosition();
        inputIterator = 0;
        drawControls = false;
        drawMappings = false;
        drawRemap = false;
    }

    void CloseMenu()
    {
        PlayerPrefs.SetFloat("Volume", AudioListener.volume);
        if (drawMappings)
            SaveMappings();
        PlayerPrefs.Save();
        Time.timeScale = 1;
        Screen.showCursor = false;
        paused = false;
    }

    #region Windows
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
            } GUILayout.EndHorizontal();
            
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
                    AudioListener.volume = GUILayout.HorizontalSlider(AudioListener.volume, 0, 2f);
                    GUILayout.FlexibleSpace();
                } GUILayout.EndVertical();
            } GUILayout.EndHorizontal();

            GUILayout.Label("", "Divider");

            if (GUILayout.Button("Controls"))
            {
                drawControls = true;
            }

            if (GUILayout.Button("Resume"))
            {
                CloseMenu();
            }

            if (GUILayout.Button("Exit"))
            {
                CloseMenu();
                Application.Quit();
            }
        } GUILayout.EndVertical();
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
                    SetPlayer("Player 1");
                    drawMappings = true;
                }
                if (GUILayout.Button("Player 2"))
                {
                    SetPlayer("Player 2");
                    drawMappings = true;
                }
            } GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(1f));
            {
                if (GUILayout.Button("Player 3"))
                {
                    SetPlayer("Player 3");
                    drawMappings = true;
                }
                if (GUILayout.Button("Player 4"))
                {
                    SetPlayer("Player 4");
                    drawMappings = true;
                }
            } GUILayout.EndHorizontal();

            GUILayout.Label("", "Divider");

            if (GUILayout.Button("Return"))
            {
                drawControls = false;
            }
        } GUILayout.EndVertical();
    }

    void mappingsWindow (int windowID)
    {
        GUILayout.BeginVertical();
        {
            GUILayout.Space(50f);

            GUILayout.BeginHorizontal(GUILayout.Height(1f));
            {
                GUILayout.BeginVertical(GUILayout.Width(mappingsMenuSize.x / 2 - 50f));
                {
                    for (int i = 0; i < PlayerInputComponent.inputNames.Length / 2; i++)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(PlayerInputComponent.inputNames[i], "ShortLabel");
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(PlayerInputComponent.mappings[player + " " + PlayerInputComponent.inputNames[i]].ToString(), "LegendaryText");
                            GUILayout.Space(25f);
                        } GUILayout.EndHorizontal();
                    }
                } GUILayout.EndVertical();

                GUILayout.BeginVertical(GUILayout.Width(mappingsMenuSize.x / 2 - 50f));
                {
                    for (int i = PlayerInputComponent.inputNames.Length / 2; i < PlayerInputComponent.inputNames.Length; i++)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            GUILayout.Label(PlayerInputComponent.inputNames[i], "ShortLabel");
                            GUILayout.FlexibleSpace();
                            GUILayout.Label(PlayerInputComponent.mappings[player + " " + PlayerInputComponent.inputNames[i]].ToString(), "LegendaryText");
                            GUILayout.Space(25f);
                        } GUILayout.EndHorizontal();
                    }
                } GUILayout.EndVertical();
            } GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(GUILayout.Height(1f));
            {
                GUILayout.Label("Reticle speed", "ShortLabel");
                GUILayout.BeginVertical();
                {
                    GUILayout.FlexibleSpace();
                    reticleSpeed = GUILayout.HorizontalSlider(reticleSpeed, 1f, 10f);
                    GUILayout.FlexibleSpace();
                } GUILayout.EndVertical();
                float.TryParse(GUILayout.TextField(reticleSpeed.ToString(), GUILayout.Width(60f)), out reticleSpeed);
                aimMode = GUILayout.Toggle(aimMode, "Alternative aim mode");
            } GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Redefine"))
                {
                    drawRemap = true;
                }

                if (GUILayout.Button("Return"))
                {
                    SaveMappings();
                    drawMappings = false;
                }
            } GUILayout.EndHorizontal();
        } GUILayout.EndVertical();
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
            } GUILayout.EndHorizontal();
        } GUILayout.EndVertical();
    }
    #endregion

    void Update()
    {
        if (!paused && Input.GetKeyDown(KeyCode.Escape))
        {
            OpenMenu();
        }
        else if (paused && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu();
        }
    }

    void OnGUI()
    {
        getSizePosition();
        if (paused)
        {
            GUI.skin = skin;
            pauseMenuRect = GUI.Window(0, pauseMenuRect, pauseWindow, "");
            GUI.BringWindowToFront(0);

            if (drawControls)
            {
                controlsMenuRect = GUI.Window(1, controlsMenuRect, controlsWindow, "");
                GUI.BringWindowToFront(1);
            }

            if (drawMappings)
            {
                mappingsMenuRect = GUI.Window(2, mappingsMenuRect, mappingsWindow, "");
                GUI.BringWindowToFront(2);
            }

            if (drawRemap)
            {
                remapMenuRect = GUI.Window(3, remapMenuRect, remapWindow, "");
                GUI.BringWindowToFront(3);
                if (Event.current.isKey && Event.current.type == EventType.KeyUp && Event.current.keyCode != KeyCode.Escape)
                {
                    PlayerPrefs.SetInt(player + " " + PlayerInputComponent.inputNames[inputIterator], (int)Event.current.keyCode);
                    PlayerInputComponent.mappings[player + " " + PlayerInputComponent.inputNames[inputIterator++]] = Event.current.keyCode;
                    if (inputIterator >= PlayerInputComponent.inputNames.Length)
                    {
                        inputIterator = 0;
                        drawRemap = false;
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
