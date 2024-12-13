using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // necesario para slider

public class TrackController : MonoBehaviour
{
    public int totalLaps = 3; // vueltas a dar por el circuito
    
    public GameObject carPrefab;
    public ButtonState forwardButton, backwardsButton; // interfaces de input
    public Slider horizontalTurning; // para el coche

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
}
