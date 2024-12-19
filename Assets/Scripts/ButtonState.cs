using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // necesario para las interfaces de las que derivamos

public class ButtonState : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] public bool pressed;

    // Start is called before the first frame update
    void Start()
    {
        pressed = false;
    }

    void OnDisable() // Para evitar fallos
    {
        pressed = false;
    }

    public void OnPointerDown(PointerEventData pointerEventData){
        pressed = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData){
        pressed = false;
    }
}
