using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal class variabili : MonoBehaviour
{
    public static RectTransform areaMov;
    public static RectTransform pomelloMov;
    //public static RectTransform barraCarburante;
    public static RectTransform barraVita;
    public static Button sparaBt;
    public static LineRenderer linea;
    public static Button warpBt;
    public static Button upgradeBt;
    public static List<effettoGravitazionale> pianeti;
    public static List<GameObject> players = new List<GameObject>();
    public static Text puntiT;

    public RectTransform _areaMov;
    public RectTransform _pomelloMov;
    //public RectTransform _barraCarburante;
    public RectTransform _barraVita;
    public Button _sparaBt;
    public LineRenderer _linea;
    public Button _warpBt;
    public Button _upgradeBt;
    public List<effettoGravitazionale> _pianeti;
    public Text _puntiT;

    void Awake()
    {
        areaMov = _areaMov;
        pomelloMov = _pomelloMov;
        //barraCarburante = _barraCarburante;
        barraVita = _barraVita;
        sparaBt = _sparaBt;
        linea = _linea;
        warpBt = _warpBt;
        upgradeBt = _upgradeBt;
        pianeti = _pianeti;
        puntiT = _puntiT;
}
}