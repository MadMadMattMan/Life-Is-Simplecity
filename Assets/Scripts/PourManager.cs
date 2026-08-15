using UnityEngine;

public class PourManager : MonoBehaviour
{
    public GameObject connectedFlask;
    public Material flaskMaterial;
    [SerializeField] GameObject PourObject;


    private void Start() {
        if (connectedFlask != null)
            connectFlask(connectedFlask);
    }

    private void connectFlask(GameObject flask) {
        if (flask == null) {
            Debug.LogError("Failed to get flask object");
            return;
        }
        connectedFlask = flask;
        flaskMaterial = flask.GetComponent<MeshRenderer>().material;
        if (flaskMaterial == null) {
            Debug.LogError("Failed to get Material from flask: " + connectedFlask);
            return;
        }

        flaskMaterial.SetVector(Shader.PropertyToID("Object Height"), 
            new Vector2(-connectedFlask.transform.localScale.y / 2, 
                        connectedFlask.transform.localScale.y / 2));
        flaskMaterial.SetFloat("Fill Volume", 0f);
    }
}
