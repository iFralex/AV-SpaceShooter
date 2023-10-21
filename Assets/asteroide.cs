using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroide : MonoBehaviour
{
    public Sprite[] asteroidi;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = asteroidi[Random.Range(0, 2)];
    }
}
