using TMPro;
using UnityEngine;

public class MainCursor : MonoBehaviour
{
    public TextMeshProUGUI text;
    public SceneMgr sceneMgr;
    void Start()
    {
        Cursor.SetCursor(default, default, default);
        text.text += sceneMgr.sets.maxScore.ToString();
    }
}
