using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    public Text text;

    private bool victory;
    private CanvasGroup cg;

    void Awake()
    {
        victory = false;
        cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
    }

    void Update()
    {
        if (victory)
        {
            cg.alpha += Time.deltaTime;
            if (cg.alpha > 1f)
                cg.alpha = 1f;
        }
    }

	void OnGUI()
    {
        if (PlayerControl.activePlayers.Count <= 1 && !victory)
        {
            text.text = PlayerControl.activePlayers[0].player + " wins!";
            victory = true;
            StartCoroutine(RestartDelayed(5f));
        }
    }

    IEnumerator RestartDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayerControl.activePlayers.Clear();
        Application.LoadLevel(Application.loadedLevel);
    }
}
