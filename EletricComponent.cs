using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class ElectricComponent : MonoBehaviour
{
    public List<GameObject> InputComponent = new List<GameObject>();
    public List<GameObject> OutputComponent = new List<GameObject>();
    public float Current;
    public float Voltage;
    public float Resistor;
    public float neededCurrent;
    public GameObject Light;

    void Update(){
        InputComponent.RemoveAll(item => item == null);
        OutputComponent.RemoveAll(item => item == null);

        if (this.gameObject.name.Contains("LED")){
            if (Current >= neededCurrent){
                Light.GetComponent<Light2D>().intensity = 1;
                GameObject.Find("Gamemanager").GetComponent<SystemManager>().GameEnd = true;
            }
        }
    }
}
