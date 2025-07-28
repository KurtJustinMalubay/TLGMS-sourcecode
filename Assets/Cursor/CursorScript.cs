using UnityEngine;

public class CursorScript : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Texture2D cursorReload;
    public playerScript playerScript;
    public playerAttack playerAttack;
    private Vector2 cursorHotspot;
    private Vector2 cursorHotspotReload;
    void Start()
    {
        if (!playerScript.sett.sett.isMobile)
        { 
            cursorHotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
            cursorHotspotReload = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        }
    }
    public void setReloadCursor()
    {
        Cursor.SetCursor(cursorReload, cursorHotspotReload, CursorMode.Auto);
    }
    public void setShootingCursor()
    {
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
    }
    public void setDefaultCursor()
    {
        Cursor.SetCursor(default, default, default);
    }
}
