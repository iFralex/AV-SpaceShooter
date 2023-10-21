using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pinManager : MonoBehaviour
{
    public GameObject pinPan;
    public RectTransform mappaRT, minimappa;
    public Transform pin;
    public static bool staModificando;
    public Vector2 posizione;
    public Camera cam;
    bool premuto;

    void Start()
    {
        staModificando = false;
        AttivaModificaPin(false);
    }

    public void AttivaModificaPin(bool a)
    {
        pinPan.SetActive(a);
        //pin.gameObject.SetActive(a);
    }

    public void SetPin()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mappaRT, Input.mousePosition, null, out posizione);
        pin.position = posizione * 46.1614f / 207f;
        print(posizione.magnitude);
        pin.gameObject.SetActive(true);
        minimappa.gameObject.SetActive(true);
    }

    public void DeletePin()
    {
        menù.punti += 1;
        pin.gameObject.SetActive(false);
        minimappa.gameObject.SetActive(false);
    }

    public void Premuto(bool b)
    {
        if (b)
            premuto = b;
        else
            StartCoroutine(PremutoCor());
    }

    IEnumerator PremutoCor()
    {
        yield return null;
        premuto = false;
    }

    void Update()
    {
        if (pinPan.activeInHierarchy && Input.GetMouseButtonUp(0) && !premuto)
            SetPin();
        if (minimappa.gameObject.activeInHierarchy)
            minimappa.anchoredPosition = new Vector2(Mathf.Clamp((pin.position.x - partitaManager.localPlayer.transform.position.x) * 190 / 48f, -190, 190), Mathf.Clamp((pin.position.y - partitaManager.localPlayer.transform.position.y) * 190 / 48f, -190, 190));
    }
}