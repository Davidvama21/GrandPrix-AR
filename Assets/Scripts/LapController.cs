using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // necesario para el texto
using System; // necesario para el TimeSpan

public class LapController : MonoBehaviour
{
    private bool goingForward; // por lo general querrá decir que has salido por delante de la línea de meta
    private int laps;
    private float currentLapTime;
    private float[] lapTimes; // para guardar los tiempos invertidos en cada vuelta
    private float currentBestTime; // el mejor tiempo hasta ahora

    private float previousXPos; // posición x previa del coche (para checkeos de colisión)

    private TrackController trackInfo; // para ver el tope de vueltas, y los objetos de la interfaz
    

    // Start is called before the first frame update
    void Start()
    {
        goingForward = false; // porque al principio estamos parados, DETRÁS de línea de meta
        laps = 0;
        currentLapTime = 0f;
        trackInfo = transform.parent.transform.GetComponent<TrackController>();
        lapTimes = new float [trackInfo.totalLaps]; // ponemos tantas posiciones como vueltas a considerar
        currentBestTime = float.MaxValue; // para tener valor "de pega"

        trackInfo.lapCount.text = "1/" + trackInfo.totalLaps; // inicializamos indicador de vueltas

        previousXPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        currentLapTime += Time.deltaTime;
        
        // Ahora ponemos el tiempo con formato de cronómetro en la interfaz
        TimeSpan timeInUnits = TimeSpan.FromSeconds (currentLapTime);
        trackInfo.chronometer.text = string.Format("{0:00}:{1:00}:{2:000}", timeInUnits.Minutes, timeInUnits.Seconds, timeInUnits.Milliseconds);

        previousXPos = transform.position.x; // actualizamos posición x para tener la previa en siguiente frame
    }

    // COMPROBACIONES DE COLISIÓN CON CHECKPOINT //
    // Aquí asumiremos que el checkpoint separa entre izquierda y derecha, y que derecha es avanzar por el circuito (ésto DEPENDERÍA del circuito en cuestión)

    void OnTriggerEnter (Collider other){

        if (previousXPos < other.transform.position.x){ // venías de la izquierda

            if (goingForward){
                if (laps < trackInfo.totalLaps){
                    lapTimes[laps] = currentLapTime; // guardamos tiempo actual como tiempo de la última vuelta pasada, cuya posición coincide con laps

                    ++laps;
                    currentBestTime = Mathf.Min(currentBestTime, currentLapTime);

                    Debug.Log ("Laps: " + laps);
                    Debug.Log ("Last lap took " + currentLapTime + " seconds");
                    trackInfo.lapCount.text = (laps+1).ToString() + '/' + trackInfo.totalLaps; // actualizamos indicador de vueltas

                    if (laps == trackInfo.totalLaps) {
                        Debug.Log ("Completed total laps");
                        trackInfo.showResults(currentBestTime); // saltamos a mostrar resultados
                    
                    }else
                        currentLapTime = 0f; // reseteamos tiempo para siguiente vuelta
                }
                
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
