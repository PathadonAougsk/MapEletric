using Unity.VisualScripting;
using UnityEngine;

public class objectGame : MonoBehaviour{
    public class Eletricwire
    {
        public Vector2 startingPos {get;, set;}
        public Vector2 endingPos {get;, set;}
        private LineRenderer LineRenderer;
        private ElectricComponent ElectricComponent;

        public initWire(Vector2 startingPos, Vector2 endingPos){
            if (!this.gameObject.contain(ElectricComponent)){
                this.gameObject.Add(ElectricComponent)
                this.ElectricComponent = this.gameObject.GetComponent<ElectricComponent>();
            }

            this.startingPos = startingPos;
            this.endingPos = endingPos;
            this.inputObj = ElectricComponent.inputObject;
            this.outputObj = ElectricComponent.outputObject;
        }
        
        public void wireupdate(){
            Lineset = this.gameObject.GetComponent<LineRenderer>();
    
            Lineset.SetPosition(0, startingPos);
            Lineset.SetPosition(1, endingPos);

            Lineset.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            Lineset.startWidth = 0.2f;
            Lineset.endWiwth = 0.2f;

            ElectricComponent.inputObject = this.inputObj;
            ElectricComponent.outputObj = this.outputObj;
        }
    }

    public class battery
    {
        public GameObject positive  {get;, set;}
        public GameObject negative  {get;, set;}
        public GameObject parent  {get;, set;}

        public initbattery(GameObject batteryObj){
            this.parent = this.gameObject.transform.parent.GetComponent<ElectricComponent>();
            this.Positive = batteryObj.transform.Find("positive").gameObject;
            this.Negative = batteryObj.transform.Find("negative").gameObject;
        }
    }

    public class resistor
    {
        public GameObject positive  {get;, set;}
        public GameObject negative  {get;, set;}
        public GameObject parent  {get;, set;}

        public initresistor(GameObject resistorObj){
            this.parent = this.gameObject.transform.parent.GetComponent<ElectricComponent>();
            this.Positive = batteryObj.transform.Find("positive").gameObject;
            this.Negative = batteryObj.transform.Find("negative").gameObject;
        }
    }

    public class Led
    {
        public GameObject positive  {get;, set;}
        public GameObject negative  {get;, set;}
        public GameObject parent  {get;, set;}

        public initLed(GameObject resistorObj){
            this.parent = this.gameObject.transform.parent.GetComponent<ElectricComponent>();
            this.Positive = batteryObj.transform.Find("positive").gameObject;
            this.Negative = batteryObj.transform.Find("negative").gameObject;
        }
    }

}