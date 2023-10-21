using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicaMenù : MonoBehaviour
{
    static bool creato;

    void Awake()
    {
        if (!creato)
        {
            DontDestroyOnLoad(gameObject);
            creato = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        
    }
}
