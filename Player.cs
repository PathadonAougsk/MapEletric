using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

    public class Player : objectGame
    {
        private bool buildMode = false;
        public GameObject inputObject;
        private GameObject Firstcontact;
        public GameObject outputObject;
        private GameObject wire;
        public GameObject selectedObj;
        public bool tutorial = false;
        private LineRenderer wireComponent;
        private SystemManager SystemManager;
        public GameObject spot;
        
        private void Update()
        {
            HandleControls();
            Movement();

            if (buildMode && selectedObj == null){
                SelectedObj();
            }
            
            if (!buildMode)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit2D = Physics2D.Raycast(mousePos, Vector2.zero, 0f, 1 << 6);

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (hit2D.collider != null)
                    {
                        Destroy(hit2D.collider.gameObject);
                    }
                }
            }

            if (inputObject != null || outputObject != null)
            {
                ControlWire();
            }
        }

        private void HandleControls()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                buildMode = !buildMode;
                if (!buildMode)
                {
                    ResetSelection();
                }
            }
        }

        private void Movement()
        {
            Vector3 movePos = new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f).normalized;
            if (Camera.main.transform.position == new Vector3(0,0,-1)){
                if (movePos.y > 0){
                    movePos.y = 0;
                }
            }  

             if (Camera.main.transform.position == new Vector3(0,-9.25f,-1)){
                if (movePos.y < 0){
                    movePos.y = 0;
                }
            } // -9.25

            Camera.main.transform.position += movePos / 4;
        }

        private void SelectedObj()
        {   
            int layer = 6;
            int layerMask = 1 << layer;
            
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.Raycast(mousePos, Vector2.zero, 0f,layerMask);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (hit2D.collider != null)
                {
                    selectedObj = hit2D.collider.gameObject;
                }
                else
                {
                    selectedObj = Instantiate(spot, mousePos, Quaternion.identity);
                }

                if (inputObject != null){
                    ChooseNegative();
                    return;
                }

                if (outputObject != null){
                    ChoosePositive();
                }

                this.gameObject.GetComponent<SystemManager>().HighLightcomponent();
                Time.timeScale = 0.5f;
            }
        }

        private void ControlWire()
        {
            if (wire == null)
            {
                Vector2 startingPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                wire = initWire(startingPos, startingPos);
            }

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            if (Input.GetKeyDown(KeyCode.Q)){
                Destroy(wire);
                ResetSelection();
                selectedObj = null;
                inputObject = null;
                outputObject = null;
                this.gameObject.GetComponent<SystemManager>().StopHighLight();
                Time.timeScale = 1;
                return;
            }
            
            wire.initWire(startingPos, mousePos);

            if (inputObject != null && selectedObj != null){
                wire.initWire(startingPos, selectedObj.transform.position);
                ConnectComponents();
            }
        }
        
    private void ConnectComponents()
    {    
    Time.timeScale = 1;
    ElectricComponent inputObj = GetElectricComponent(inputObject);
    ElectricComponent outputObj = GetElectricComponent(outputObject);
    GameObject secondcontact;

    if (inputObject != Firstcontact){
            secondcontact = inputObject;
        } else {
            secondcontact = outputObject;
            wireComponent.startColor = Color.red;
            wireComponent.endColor = Color.red;
        }
    

    if (Firstcontact != null && Firstcontact.transform.parent != null)
        {
            Firstcontact = Firstcontact.transform.parent.gameObject;
            Debug.Log(Firstcontact);
        }


    if (inputObj != null && outputObj != null){
        if (tutorial){
            DistanceJoint2D joint = Firstcontact.AddComponent<DistanceJoint2D>();
            joint.autoConfigureDistance = false;

            joint.connectedBody =  GetRigidbody2DComponent(secondcontact);
            joint.connectedAnchor = Vector2.zero;
            joint.distance = 2;
        }

        inputObj.OutputComponent.Add(outputObject);
        outputObj.InputComponent.Add(inputObject);
        AddWireBehaviour();
        }

        wire = null;
        Firstcontact = null;
        ResetSelection();
        this.gameObject.GetComponent<SystemManager>().StopHighLight();
    }

        private ElectricComponent GetElectricComponent(GameObject obj)
        {
            ElectricComponent component = obj.GetComponent<ElectricComponent>();
            if (component == null && obj.transform.parent != null)
            {
                component = obj.transform.parent.GetComponent<ElectricComponent>();
            }
            return component;
        }

        private Rigidbody2D GetRigidbody2DComponent(GameObject obj){
            Rigidbody2D component = obj.GetComponent<Rigidbody2D>();
            if (component == null && obj.transform.parent != null)
            {
                component = obj.transform.parent.GetComponent<Rigidbody2D>();
            }
            return component;
        }
        private void ResetSelection()
        {
            inputObject = null;
            outputObject = null;
        }

        private void AddWireBehaviour()
        {
            var wireBehaviour = wire.AddComponent<WireBehaviour>();
            wireBehaviour.Firstobj = inputObject;
            wireBehaviour.SecondObj = outputObject;
        }
        public void ChoosePositive()
        {
            Debug.Log("Click");
            if (selectedObj == null){
                return;
            }

            inputObject = selectedObj;
            
            if (inputObject.transform.childCount > 0){
                Transform[] ts = inputObject.transform.GetComponentsInChildren<Transform>(true);
                foreach (Transform t in ts) if (t.gameObject.name == "Positive") inputObject = t.gameObject;
            }

            selectedObj = null;
        }

        public void ChooseNegative()
        {
            Debug.Log("Click");
            if (selectedObj == null){
                return;
            }
            
            outputObject = selectedObj;

            if (outputObject.transform.childCount > 0){
                Transform[] ts = outputObject.transform.GetComponentsInChildren<Transform>(true);
                foreach (Transform t in ts) if (t.gameObject.name == "Negative") outputObject = t.gameObject;
            }
            selectedObj = null;
            Debug.Log("Click");
        }
    }
