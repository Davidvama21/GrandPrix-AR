using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // necesario para el texto del input field

public class LapSelector : MonoBehaviour
{
    public TrackController trackInfo; // para acceder al número de vueltas (y modificarlo si hace falta)
    public int maxLaps = 999; // tope de vueltas (arbitrario)

    private TMP_Text textInfo;

    // Start is called before the first frame update
    void Start()
    {
        textInfo = GetComponent<TMP_Text>();

        textInfo.text = trackInfo.totalLaps.ToString() + " lap(s)"; // para que esté inicializado con el valor del circuito
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Funciones para llamar desde fuera //
    public void increaseLaps() {

        if (trackInfo.totalLaps < maxLaps)
        {

            ++trackInfo.totalLaps;
            textInfo.text = trackInfo.totalLaps.ToString() + " lap(s)"; // actualizar info de laps mostrada
        }
    }

    public void decreaseLaps()
    {
        if (trackInfo.totalLaps > 1) {

            --trackInfo.totalLaps;
            textInfo.text = trackInfo.totalLaps.ToString() + " lap(s)"; // actualizar info de laps mostrada
        }
    }


}
