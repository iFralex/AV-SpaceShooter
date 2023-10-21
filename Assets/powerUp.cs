using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUp : MonoBehaviour
{
    public enum PowerUps { velocità, salute, rifornimento};
    public PowerUps tipo;
    public corsaManager player;

    void Start()
    {
        tipo = (PowerUps)Random.Range(0, 3);
        GetComponent<SpriteRenderer>().sprite = player.powerUpsImg[(int)tipo];
        if (tipo == PowerUps.velocità)
            gameObject.AddComponent<CircleCollider2D>();
        else if (tipo == PowerUps.rifornimento)
            gameObject.AddComponent<PolygonCollider2D>();
        else if (tipo == PowerUps.salute)
            gameObject.AddComponent<BoxCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        switch (tipo)
        {
            case PowerUps.velocità:
                player.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * 2, ForceMode2D.Impulse);
                break;
            case PowerUps.salute:
                player.vita = 10;
                break;
            case PowerUps.rifornimento:
                player.carburante = 10;
                break;
        }
        col.gameObject.GetComponent<proiettile>().Dist();
    }
}