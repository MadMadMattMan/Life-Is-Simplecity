using UnityEngine;

public class PourManager : Interactable {
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

    Color potionColor;

    AudioSource pourAudio;
    AudioClip clip;

    bool update = false;


    [Header("Testing")]
    [Range(0, 2)] public int c = 0;
    public bool u = false;

    private void Start() {
        if (ConnectedCauldron != null) {
            ConnectCauldron(ConnectedCauldron);
            update = true;
        }
        pourAudio = GetComponent<AudioSource>();
        clip = pourAudio.clip;

        oldColor = new Color(0.8f, 0.85f, 1f);
        newColor = new Color(0.8f, 0.85f, 1f);
        potionColor = newColor;
    }

    public override void PlaceCauldron()
    {
        base.PlaceCauldron();
        ConnectCauldron(Cauldron);
    }
    public override void PickupCauldron()
    {
        base.PickupCauldron();
        DisconnectCauldron();
    }

    public void ConnectCauldron(GameObject obj) {
        ConnectedCauldron = obj;
        connectedCauldronLiquid = obj.GetComponentInChildren<MeshRenderer>().material;
        cManager = obj.GetComponent<CauldronManager>();

        newColor = connectedCauldronLiquid.color;
        oldColor = newColor;
        Tutorial.PlacedCauldronOnPourStation = true;
    }

    public void DisconnectCauldron() {
        if (ConnectedCauldron == null) {
            Debug.LogWarning("Cannot disconnect with no cauldron attached");
            return;
        }
        newColor = oldColor = connectedCauldronLiquid.color;
        ConnectedCauldron = null;
    }

    public void AddColor(int color) {
        if (ConnectedCauldron == null) {
            Debug.LogWarning("Cannot disconnect with no cauldron attached");
            return;
        }

        if (color < 0 || color > 2) {
            Debug.LogWarning("Incorrect Color added " + color);
            return;
        }
        if (!clip)
            clip = pourAudio.clip;

        pourAudio.PlayOneShot(clip);
        if (color == 0) {
            cManager.colorAdditions.x++;
            RedPourParticles.Play();
            update = true;
        }
        if (color == 1) {
            cManager.colorAdditions.y++;
            GreenPourParticles.Play();
            update = true;
        }
        if (color == 2) {
            cManager.colorAdditions.z++;
            BluePourParticles.Play();
            update = true;
        }
        Tutorial.UsedBase = true;
    }


    private void Update() {
        if (ConnectedCauldron == null)
            return;

        if (u) {
            AddColor(c);
            u = false;
        }

        if (update) {
            oldColor = potionColor;
            Vector3 col = new Vector3(0.5f, 0.2f, 0f);
            print(cManager.colorAdditions);
            if (cManager.GetColorSum() < 3)
                col = cManager.GetAdditions();
            newColor = new Color(col.x, col.y, col.z);

            enlapsedTime = 0f;
            update = false;
            lerpingColor = true;
        }

        if (lerpingColor) {
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
