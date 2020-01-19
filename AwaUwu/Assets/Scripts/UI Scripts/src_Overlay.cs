using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class src_Overlay : MonoBehaviour
{
    public Image DmgOverlay;

    private float r;
    private float g;
    private float b;
    public float a;

    // Start is called before the first frame update
    void Start()
    {
        r = DmgOverlay.color.r;
        g = DmgOverlay.color.g;
        b = DmgOverlay.color.b;
        a = DmgOverlay.color.a;
    }

    public void adjustOverlayColor(float suma)
    {
        a += suma;
        Mathf.Clamp(a, 0, 1f);
        Color c = new Color(r, g, b, a);
        DmgOverlay.color = c;
    }
}
