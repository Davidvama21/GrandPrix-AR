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
    public bool targetFound = false; // indica si el marcador ha sido reconocido o no (para decidir si el coche debe estar activo)   


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
                          Quaternion.Euler(startingCarRotation), transform); // crea el coche COMO HIJO DEL TRACK con posición relativa al track (considerando su escalado) y su rotación

        // Escalamos los valores de sus scripts y componentes para que se adapten al tamaño del circuito (usaremos escala en x del track para ésto)
        // 1. Parámetros CarController
        /*
        CarController movementInfo = instantiatedCar.transform.GetComponent<CarController>();
        movementInfo.brakeTorque *= transform.localScale.x;
        movementInfo.brakeScale *= transform.localScale.x;
        movementInfo.motorTorque *= transform.localScale.x;
        movementInfo.maxSpeed *= transform.localScale.x;
        movementInfo.turnAngle *= transform.localScale.x;
        movementInfo.turnAngleAtMaxSpeed *= transform.localScale.x;
        movementInfo.centreOfGravityOffset *= transform.localScale.x;

        // 2. Parámetros Rigidbody
        Rigidbody physicsInfo = instantiatedCar.transform.GetComponent<Rigidbody>();
        physicsInfo.mass *= transform.localScale.x;
        physicsInfo.drag *= transform.localScale.x;
        physicsInfo.angularDrag *= transform.localScale.x;

        // 3. Parámetros ruedas (WheelColliders)
        WheelCollider[] wheelsInfo = instantiatedCar.transform.GetComponentsInChildren<WheelCollider>();
        foreach (WheelCollider wheelInfo in wheelsInfo){

            wheelInfo.mass *= transform.localScale.x;
            wheelInfo.radius *= transform.localScale.x;
            wheelInfo.wheelDampingRate *= transform.localScale.x;
            wheelInfo.suspensionDistance *= transform.localScale.x;
            wheelInfo.forceAppPointDistance *= transform.localScale.x;
            wheelInfo.center *= transform.localScale.x;
            
            JointSpring wheelSSpring = wheelInfo.suspensionSpring; // no se puede asignar directo paráms., así que hay que hacer copia primero
            wheelSSpring.spring *= transform.localScale.x;
            wheelSSpring.damper *= transform.localScale.x;
            wheelSSpring.targetPosition *= transform.localScale.x;
            wheelInfo.suspensionSpring = wheelSSpring; // y asignación en bloque

            WheelFrictionCurve wheelFFriction = wheelInfo.forwardFriction; // otro igual
            wheelFFriction.extremumSlip *= transform.localScale.x;
            wheelFFriction.extremumValue *= transform.localScale.x;
            wheelFFriction.asymptoteSlip *= transform.localScale.x;
            wheelFFriction.asymptoteValue *= transform.localScale.x;
            wheelFFriction.stiffness *= transform.localScale.x;
            wheelInfo.forwardFriction = wheelFFriction;

            WheelFrictionCurve wheelSFriction = wheelInfo.sidewaysFriction; // otro igual
            wheelSFriction.extremumSlip *= transform.localScale.x;
            wheelSFriction.extremumValue *= transform.localScale.x;
            wheelSFriction.asymptoteSlip *= transform.localScale.x;
            wheelSFriction.asymptoteValue *= transform.localScale.x;
            wheelSFriction.stiffness *= transform.localScale.x;
            wheelInfo.sidewaysFriction = wheelSFriction;
        }
        */
        // Ahora inicializamos los botones que darán su input, para poder controlarlo
        InputController inputInfo = instantiatedCar.transform.GetComponent<InputController>();
        inputInfo.horizontalTurning = horizontalTurning;
        inputInfo.forwardButton = forwardButton;
        inputInfo.backwardsButton = backwardsButton;

        // Por último, decidimos si dejar activo el objeto, en función de si el marcador ya se encontró (si no, puede que no esté activo el suelo, así que es mejor no activarlo)
        instantiatedCar.SetActive (targetFound);
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

    // FUNCIÓN PARA HABILITAR EL COCHE O NO SEGÚN SI SE RECONOCE MARCADOR //
    public void enableCar (bool value){

        targetFound = value; // porque habilitaremos coche sólo cuando el marcador se reconozca (para evitar que el coche aparezca sin el suelo)

        if (instantiatedCar != null) // si existe,
            instantiatedCar.SetActive (value);
    }
}
