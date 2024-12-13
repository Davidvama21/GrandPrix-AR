using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // necesario para slider

public class InputController : MonoBehaviour
{
    public ButtonState forwardButton, backwardsButton; // para ver estado elementos
    public Slider horizontalTurning; // que indican input actual

    CarController car; // para enviar a control coche

    // Start is called before the first frame update
    void Start()
    {
        car = GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        car.setTurning (horizontalTurning.value);
        car.setMovement (-System.Convert.ToSingle (backwardsButton.pressed) + System.Convert.ToSingle (forwardButton.pressed) );
    }
}
