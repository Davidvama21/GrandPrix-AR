using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // necesario para slider
using TMPro; // necesario para el texto
using System; // necesario para el TimeSpan

public class TrackController : MonoBehaviour
{
    public int totalLaps = 3; // vueltas a dar por el circuito
    
    public ButtonState forwardButton, backwardsButton; // interfaces de input
    public Slider horizontalTurning; // para el coche

    public TMP_Text lapCount; // interfaces para info
    public TMP_Text chronometer; // del coche

    // PADRE DE LOS ELEMENTOS UI ANTERIORES (para desactivar interfaz fácilmente) //
    public GameObject parentCarUI;

    public GameObject parentResultsUI; // para resultados, cuando acaben las vueltas

    public GameObject carPrefab;
    public Vector3 startingCarPosition; // posición (relativa al track) y
    public Vector3 startingCarRotation; // rotación del coche carPrefab al empezar


    // Start is called before the first frame update
    void Start()
    {
       GameObject car = Instantiate (carPrefab, transform.position + startingCarPosition, Quaternion.Euler(startingCarRotation), transform); // crea el coche COMO HIJO DEL TRACK con posición relativa al track y su rotación

        // Ahora inicializamos los botones que darán su input, para poder controlarlo
        InputController inputInfo = car.transform.GetComponent<InputController>();
        inputInfo.horizontalTurning = horizontalTurning;
        inputInfo.forwardButton = forwardButton;
        inputInfo.backwardsButton = backwardsButton;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Muestra la interfaz de resultados //
    public void showResults(float bestTime) {

        parentCarUI.SetActive(false); // desactivamos la UI del coche
        parentResultsUI.SetActive(true);

        // Ahora pondremos el mejor tiempo con formato de cronómetro en la interfaz de resultados
        TimeSpan timeInUnits = TimeSpan.FromSeconds(bestTime);
        parentResultsUI.transform.GetChild(1).GetComponent<TMP_Text>().text = string.Format("{0:00}:{1:00}:{2:000}", timeInUnits.Minutes, timeInUnits.Seconds, timeInUnits.Milliseconds);
    }
}
