using UnityEngine;

public class PourManager : MonoBehaviour {
    [SerializeField] ParticleSystem RedPourParticles;
    [SerializeField] ParticleSystem GreenPourParticles;
    [SerializeField] ParticleSystem BluePourParticles;


    public GameObject ConnectedCauldron;
    CauldronManager cManager;
    Material connectedCauldronLiquid;

    Color oldColor = new Color(0.8f, 0.85f, 1f);
    [SerializeField] Vector3 newColorVector = new Vector3(0.8f, 0.85f, 1f);
    Color newColor = new Color(0.8f, 0.85f, 1f);
    Vector3 addedColor = Vector3.zero;

    bool lerpingColor = false;
    float enlapsedTime = 0f;
    [SerializeField] float colorLerpTime = 5f;

    Color potionColor = new Color(240, 255, 245);

    bool update = false;


    [Header("Testing")]
    [Range(0, 2)] public int c = 0;
    public bool u = false;

    private void Start() {
        ConnectCauldron(ConnectedCauldron);
        update = true;
    }

    public void ConnectCauldron(GameObject obj) {
        ConnectedCauldron = obj;
        connectedCauldronLiquid = obj.GetComponentInChildren<MeshRenderer>().material;
        cManager = obj.GetComponent<CauldronManager>();

        newColor = connectedCauldronLiquid.color;
        oldColor = newColor;
    }

    public void DisconnectCauldron() {
        newColor = oldColor = connectedCauldronLiquid.color;
        cManager.SetColor(new Vector3(newColor.r, newColor.g, newColor.b));
    }

    public void AddColor(int color) {
        if (color < 0 || color > 2) {
            Debug.LogWarning("Incorrect Color added " + color);
            return;
        }

        if (color == 0) {
            addedColor = new Vector3(1, 0, 0);
            RedPourParticles.Play();
            update = true;
        }
        if (color == 1) {
            addedColor = new Vector3(0, 1, 0);
            GreenPourParticles.Play();
            update = true;
        }
        if (color == 2) {
            addedColor = new Vector3(0, 0, 1);
            BluePourParticles.Play();
            update = true;
        }
    }


    private void Update() {
        if (u) {
            AddColor(c);
            u = false;
        }

        if (update) {
            Debug.Log("updated");
            oldColor = potionColor;
            newColorVector += (addedColor*2 + new Vector3(-0.5f, -0.5f, -0.5f)).normalized;
            newColorVector.Normalize();
            newColor = new Color(newColorVector.x, newColorVector.y, newColorVector.z);
            enlapsedTime = 0f;
            update = false;
            lerpingColor = true;
        }

        if (lerpingColor) {
            Debug.Log("Lerping");
            potionColor = Color.Lerp(oldColor, newColor, enlapsedTime/colorLerpTime);
            connectedCauldronLiquid.color = potionColor;

            enlapsedTime += Time.deltaTime;
            if (enlapsedTime >= colorLerpTime) {
                oldColor = newColor;
                lerpingColor = false;
            }
        }
    }
}
