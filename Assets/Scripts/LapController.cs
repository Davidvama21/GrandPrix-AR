using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapController : MonoBehaviour
{
    private bool goingForward; // por lo general querrá decir que has salido por delante de la línea de meta
    private int laps;

    private float previousXPos; // posición x previa del coche (para checkeos de colisión)

    // Start is called before the first frame update
    void Start()
    {
        goingForward = false; // porque al principio estamos parados, DETRÁS de línea de meta
        laps = 0;

        previousXPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        previousXPos = transform.position.x; // actualizamos posición x para tener la previa en siguiente frame
    }

    // COMPROVACIONES DE COLISIÓN CON CHECKPOINT //
    // Aquí asumiremos que el checkpoint separa entre izquierda y derecha, y que derecha es avanzar por el circuito (ésto DEPENDERÍA del circuito en cuestión)

    void OnTriggerEnter (Collider other){

        if (previousXPos < other.transform.position.x){ // venías de la izquierda

            if (goingForward){
                ++laps;
                Debug.Log ("Laps: " + laps);
            }

        }

        goingForward = false;
    }

    void OnTriggerExit (Collider other){

        if (transform.position.x > other.transform.position.x){ // coche ha salido por la derecha (hacia adelante)
        
            goingForward = true;
        }

    }
}
