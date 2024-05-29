
using UnityEngine;
using UnityEngine.Rendering.Universal;



public class SystemManager : MonoBehaviour
{
    public GameObject BatteryPrefab;
    public GameObject ResistorPrefab;
    public GameObject LedPrefab;
    public GameObject GlobalLight;
    public GameObject timer;
    private bool FrozeLight;
    float duration = 60f; // Total duration in seconds (1 minute)
    float elapsedTime = 0f; // Time elapsed since starting
    int neededCurrent;
    public bool GameEnd = false;
    private void Start(){
        Generate();
    }

    private void Update()
    {
        // if (GameEnd){
        //     Generate();
        //     GameEnd = false;
        // }

        if (!FrozeLight)
        {
            elapsedTime += Time.deltaTime;
            float interpolationFactor = elapsedTime / duration;
            float newIntensity = 1f - interpolationFactor;
            
           
            RectTransform timerRectTransform = timer.GetComponent<RectTransform>();
            float initialHeight = 383.7f;
            float targetHeight = 0f;
            float newSize = Mathf.Lerp(initialHeight, targetHeight, elapsedTime / duration);

            //height 383.7 but reach 0 in exactly 1 mins
            timerRectTransform.sizeDelta = new Vector2(timerRectTransform.rect.width,newSize);

            // Set the global light intensity
            GlobalLight.GetComponent<Light2D>().intensity = newIntensity;
        }
    }

    public void HighLightcomponent(){
        GameObject[] existingComponents = GameObject.FindGameObjectsWithTag("Component");
        GlobalLight.GetComponent<Light2D>().intensity = 0.2f;
        foreach (GameObject component in existingComponents)
        {
            if (component.GetComponentInChildren<Light2D>() != null){
                component.GetComponentInChildren<Light2D>().intensity = 1;
            } else {
                component.GetComponent<Light2D>().intensity = 1;
            }
        }

        FrozeLight = true;
    }

    public void StopHighLight(){
        GameObject[] existingComponents = GameObject.FindGameObjectsWithTag("Component");
        GlobalLight.GetComponent<Light2D>().intensity = 0.2f;
        foreach (GameObject component in existingComponents)
        {
            component.GetComponentInChildren<Light2D>().intensity = 0;
        }
        
        FrozeLight = false;
    }

    void Generate()
    {
        // Destroy all existing components
        GameObject[] existingComponents = GameObject.FindGameObjectsWithTag("Component");
        foreach (GameObject component in existingComponents)
        {
            Destroy(component);
        }

        // Randomize parameters
        int voltage = Random.Range(20, 50);
        int resistorValue = Random.Range(20, 50);
        neededCurrent = voltage / resistorValue;

        // Instantiate components
        GameObject spawnpoint = GameObject.Find("Spawnpoint");
        if (spawnpoint == null){
            Debug.Log("No spawnpoint found");
            return;
        }

        GameObject batteryObj = Instantiate(BatteryPrefab, spawnpoint.transform.position, Quaternion.identity);
        batteryObj.transform.Rotate(0f, 0f, -90f, Space.Self);
        ElectricComponent batteryElectric = GetElectricComponent(batteryObj);
        batteryElectric.Voltage = voltage;

        GameObject resistorObj = Instantiate(ResistorPrefab, spawnpoint.transform.position + new Vector3(0, 50, 0), Quaternion.identity);
        ElectricComponent resistorElectric = GetElectricComponent(resistorObj);
        resistorElectric.Resistor = resistorValue;

        GameObject ledObj = Instantiate(LedPrefab, spawnpoint.transform.position + new Vector3(0, 100, 0), Quaternion.identity);
        ElectricComponent ledElectric = GetElectricComponent(ledObj);
        ledElectric.neededCurrent = neededCurrent;

        elapsedTime = 0;
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
}
