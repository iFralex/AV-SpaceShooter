using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfondoStellatoParallasse : sfondoStellato
{
    public float velocita;
    Rigidbody2D player;
    Vector2 cambio;

    public override void Start()
    {
        base.Start();
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            player = partitaManager.localPlayer.GetComponent<Rigidbody2D>();
            return;
        }

        if (player.velocity.x > 0)
        {
            if (transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition.x < 0)
            {
                transform.GetChild(3).SetSiblingIndex(2);
                transform.GetChild(1).SetSiblingIndex(0);
            }
        }
        else if (player.velocity.x < 0)
        {
            if (transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition.x > 0)
            {
                transform.GetChild(3).SetSiblingIndex(2);
                transform.GetChild(1).SetSiblingIndex(0);
            }
        }

        if (player.velocity.y > 0)
        {
            if (transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition.y < 0)
            {
                transform.GetChild(2).SetSiblingIndex(0);
                transform.GetChild(3).SetSiblingIndex(1);
            }
        }
        else if (player.velocity.y < 0)
        {
            if (transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition.y > 0)
            {
                transform.GetChild(2).SetSiblingIndex(0);
                transform.GetChild(3).SetSiblingIndex(1);
            }
        }

        if (player.velocity.x > 0)
        {
            transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.x - player.velocity.x * velocita * Time.fixedDeltaTime, transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.y - player.velocity.y * velocita * Time.fixedDeltaTime);
            transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.x + 2208, transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.y - player.velocity.y * velocita * Time.fixedDeltaTime);
        }
        else if (player.velocity.x < 0)
        {
            transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.x - player.velocity.x * velocita * Time.fixedDeltaTime, transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.y);
            transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.x - 2208, transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.y);
        }

        if (player.velocity.y > 0)
        {
            transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.x, transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition.y + 1284);
            transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition.x, transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition.y + 1284);
        }
        else if (player.velocity.y < 0)
        {
            transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.x, transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.y - 1284);
            transform.GetChild(3).GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition.x, transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.y - 1284);
        }
    }
}