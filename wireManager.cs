using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class wireManager : MonoBehaviour
{   
    private bool Finished = false; 
    private Dictionary<int, List<GameObject>> nodeDictionary = new Dictionary<int, List<GameObject>>();
    private List<GameObject> visitedNodes = new List<GameObject>();
    public float totalResistor;
    public static event Action allconnect;
    public void Update()
    {
        if (GameObject.FindWithTag("Battery") == null){
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("hello");
            ArrangeNode();
            TotalResistorCalculate();
            AssignCurrent();
            Finished = true;
        }
    }

    private void ArrangeNode()
    {
        nodeDictionary.Clear();
        visitedNodes.Clear();

        GameObject root = GameObject.FindWithTag("Battery");
        if (root == null)
        {
            return;
        }

        List<GameObject> nodeList = new List<GameObject> { root };
        nodeDictionary.Add(0, nodeList);

        int placement = 0;
        while (true)
        {
            if (!nodeDictionary.ContainsKey(placement))
            {
                break;
            }

            nodeList = nodeDictionary[placement];
            List<GameObject> nextNodeList = new List<GameObject>();

            foreach (GameObject nodeObject in nodeList)
            {
                GameObject checkList = nodeObject.transform.parent != null ? nodeObject.transform.parent.gameObject : nodeObject;
                ElectricComponent electricComponent = checkList.GetComponent<ElectricComponent>();
                if (electricComponent != null)
                {
                    foreach (GameObject outputObject in electricComponent.OutputComponent)
                    {
                        checkList = outputObject.transform.parent != null ? outputObject.transform.parent.gameObject : outputObject;

                        if (!visitedNodes.Contains(checkList))
                        {
                            nextNodeList.Add(checkList);
                            visitedNodes.Add(checkList);
                        }
                    }
                }
            }

            if (nextNodeList.Count > 0)
            {
                placement++;
                nodeDictionary.Add(placement, nextNodeList);
            }
            else
            {
                break;
            }
        }

        allconnect?.Invoke();
    }
    private void AssignCurrent(){
        Debug.Log("assign");
        foreach (List<GameObject> OutputComponet in nodeDictionary.Values){
            if (!OutputComponet.Contains(GameObject.FindWithTag("Battery"))){
                foreach(GameObject x in OutputComponet){
                    Debug.Log(GameObject.FindWithTag("Battery").GetComponent<ElectricComponent>().Voltage);
                    Debug.Log(totalResistor);
                    
                    x.GetComponent<ElectricComponent>().Current += GameObject.FindWithTag("Battery").GetComponent<ElectricComponent>().Voltage / totalResistor;
                }
            }
        }
    }

    private void TotalResistorCalculate()
    {
        List<int> largestListKeys = nodeDictionary.Where(kv => kv.Value.Count > 1)
                                          .Select(kv => kv.Key)
                                          .ToList();


        
        List<GameObject> parallelObj = new List<GameObject>();
        foreach (int key in largestListKeys)
        {
            parallelObj.AddRange(nodeDictionary[key]);
        }

        float totalReciprocalResistor = 0;
        foreach (GameObject parallelComponent in parallelObj)
        {
            if (parallelComponent.TryGetComponent<ElectricComponent>(out ElectricComponent electricComponent))
            {
                totalReciprocalResistor += 1 / electricComponent.Resistor;
            }
        }

        totalResistor = 0;
        if (totalReciprocalResistor != 0){
            totalResistor = 1 / totalReciprocalResistor;
        }

        List<int> NodeUnParallel = nodeDictionary.Keys.Except(largestListKeys).ToList();
        foreach(int placement in NodeUnParallel){
            foreach(GameObject component in nodeDictionary[placement]){
                if (component.TryGetComponent<ElectricComponent>(out ElectricComponent electricComponent))
                {
                totalResistor += electricComponent.Resistor;
                }
            }
        }

        Debug.Log(totalResistor);
    }

    private ElectricComponent TryGetComponentParent(GameObject gameObject)
    {
        ElectricComponent component = gameObject.GetComponentInParent<ElectricComponent>();
        return component != null ? component : gameObject.GetComponent<ElectricComponent>();
    }
}
