using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfondoStellato : MonoBehaviour
{
    Color[] colors = new Color[] { Color.red, Color.red, Color.red, Color.blue, Color.white, Color.white, Color.white, Color.white, Color.white, Color.white, Color.white, Color.white, Color.white, Color.white, Color.white, Color.white, Color.white, Color.white, Color.white, Color.white };

    public virtual void Start()
    {
        for (int o = 0; o < transform.childCount; o++)
        {
            for (int i = 0; i < transform.GetChild(o).childCount; i++)
            {
                float n = Random.Range(0, .8f);
                float s = Random.Range(.5f, 1.5f);
                transform.GetChild(o).GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector3(Random.Range(-1104, 1104), Random.Range(-621, 621), 0);
                transform.GetChild(o).GetChild(i).GetComponent<RectTransform>().localScale = new Vector3(s, s, 0);
                Color color = colors[i % 20];
                if (color == Color.blue)
                {
                    color.r = n;
                    color.g = n;
                }
                else if (color == Color.red)
                {
                    color.b = n;
                    color.g = n;
                }
                transform.GetChild(o).GetChild(i).GetComponent<UnityEngine.UI.Image>().color = color;
            }
        }
    }
}