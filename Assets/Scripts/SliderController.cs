using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // necesario para slider
using UnityEngine.EventSystems; // necesario para las interfaces de las que derivamos

public class SliderController : MonoBehaviour, IPointerUpHandler
{
    public float resetValue = 0; // el valor de reseteo del slider cuando dejamos de pulsarlo

    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        slider.value = resetValue;
    }
}
