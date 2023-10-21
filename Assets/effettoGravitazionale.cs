using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effettoGravitazionale : MonoBehaviour
{
    public float maxDistance = 300;
    float mass;
    Rigidbody2D playerRb;

    void Start()
    {
        mass = transform.lossyScale.x / 10;
        playerRb = partitaManager.localPlayer.GetComponent<Rigidbody2D>();
        StartCoroutine(Gravita());
    }
    /*
    void FixedUpdate()
    {
        playerRb.AddForce(DetermineGravitationalForce(playerRb));
    }

    Vector2 DetermineGravitationalForce(Rigidbody2D otherBody)
    {
        Vector2 relativePosition = rigidbody2D.position - otherBody.position;

        float distance = Mathf.Clamp(relativePosition.magnitude, 0, maxDistance);

        //the force of gravity will reduce by the distance squared
        float gravityFactor = 1f - (Mathf.Sqrt(distance) / Mathf.Sqrt(maxDistance));

        //creates a vector that will force the otherbody toward this body, using the gravity factor times the mass of this body as the magnitude
        Vector2 gravitationalForce = relativePosition.normalized * (gravityFactor * rigidbody2D.mass);

        return gravitationalForce;
    }
    */
    IEnumerator Gravita()
    {
        while (true)
        {
            while (((Vector2)transform.position - playerRb.position).sqrMagnitude > Mathf.Pow(transform.lossyScale.x / 2, 2) + maxDistance * maxDistance)
                yield return new WaitForSeconds(.5f);
            if (((Vector2)transform.position - playerRb.position).sqrMagnitude < Mathf.Pow(transform.lossyScale.x / 2, 2))
            {
                yield return null;
                continue;
            }
            playerRb.AddForce((((Vector2)transform.position - playerRb.position).normalized * ((1f - Mathf.Sqrt(((Vector2)transform.position - playerRb.position).sqrMagnitude / (Mathf.Pow(transform.lossyScale.x / 2, 2) + maxDistance * maxDistance))) * mass)));
            playerRb.transform.localScale = Vector3.one * .3f * Mathf.Min(((Vector2)transform.position - playerRb.position).magnitude / maxDistance + .4f, 1);
            yield return null;
        }
    }
}