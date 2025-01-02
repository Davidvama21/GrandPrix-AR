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
    public Slider horizontalTurning;                   // para el coche

    public TMP_Text lapCount; // interfaces para info
    public TMP_Text chronometer; // del coche

    
    public GameObject parentCarUI; // PADRE DE LOS ELEMENTOS UI ANTERIORES (para desactivar interfaz fácilmente)
    public GameObject parentResultsUI; // para interfaz resultados, cuando acaben las vueltas
    public GameObject parentMainMenuUI; // para ir y venir del menú de inicio

    public GameObject carPrefab;
    public Vector3 startingCarPosition; // posición (relativa al track) y
    public Vector3 startingCarRotation; // rotación del coche carPrefab al empezar


    private GameObject instantiatedCar; 


    // Start is called before the first frame update
    void Start()
    {
        //setupCar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Método para crear el coche //
    void setupCar() {

        instantiatedCar = Instantiate(carPrefab, transform.position + new Vector3 (transform.localScale.x * startingCarPosition.x, transform.localScale.y * startingCarPosition.y, transform.localScale.z * startingCarPosition.z),
                          Quaternion.Euler(startingCarRotation), transform); // crea el coche COMO HIJO DEL TRACK con posición relativa al track y su rotación


        // Ahora inicializamos los botones que darán su input, para poder controlarlo
        InputController inputInfo = instantiatedCar.transform.GetComponent<InputController>();
        inputInfo.horizontalTurning = horizontalTurning;
        inputInfo.forwardButton = forwardButton;
        inputInfo.backwardsButton = backwardsButton;
    }

    // FUNCIONES PARA CAMBIAR INTERFAZ //

    // Muestra interfaz de control coche e inicia la carrera //
    public void startCourse() {
        setupCar();

        parentMainMenuUI.SetActive(false);
        parentCarUI.SetActive(true); // activamos UI del coche
    }

    // Muestra la interfaz de resultados //
    public void showResults(float bestTime) {

        parentCarUI.SetActive(false); // desactivamos la UI del coche
        parentResultsUI.SetActive(true);

        // Ahora pondremos el mejor tiempo con formato de cronómetro en la interfaz de resultados
        TimeSpan timeInUnits = TimeSpan.FromSeconds(bestTime);
        parentResultsUI.transform.GetChild(1).GetComponent<TMP_Text>().text = string.Format("{0:00}:{1:00}:{2:000}", timeInUnits.Minutes, timeInUnits.Seconds, timeInUnits.Milliseconds);
    }

    // Reinicia la carrera, mostrando de nuevo interfaz de control // <- desde resultados
    public void retryCourse(){

        Destroy(instantiatedCar);
        setupCar(); // para crear nuevo coche con estado y lugar reseteado

        parentResultsUI.SetActive(false);
        parentCarUI.SetActive(true);
    }

    // Vuelve al menú principal, para permitir cambiar el número de vueltas // <- desde resultados
    public void changeLaps() {

        Destroy(instantiatedCar);

        parentResultsUI.SetActive(false);
        parentMainMenuUI.SetActive(true);
    }
}
