using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapController : MonoBehaviour
{
    private bool goingForward;
    private int laps;

    // Start is called before the first frame update
    void Start()
    {
        goingForward = false; // porque al principio estamos parados, DETRÁS de línea de meta
        laps = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerExit (Collider other){
        // Aquí asumiremos que el checkpoint separa entre izquierda y derecha, y que derecha es avanzar por el circuito (ésto DEPENDERÍA del circuito en cuestión)

        if (transform.position.x < other.transform.position.x){ // coche ha salido por la izquierda (hacia atrás)

            goingForward = false;

        }else { // tiene que haber salido por la derecha (adelante)

            if (goingForward){ // ésto querría decir que ya venía de ir hacia adelante, por lo que ha tenido que dar la vuelta al circuito
                ++laps;
                Debug.Log ("Laps: " + laps);
            }
            
            else
                goingForward = true;
        }

    }
}
