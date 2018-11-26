using UnityEngine;

public class ChatZone : MonoBehaviour
{

    #region Attributes

    public GameObject chatButtonOff;
    public GameObject chatButtonOn;
    private GameObject[] particles;
    private HUDDisplay hpAndMp;


    #endregion

    #region Start & Update

    private void Start()
    {
        InitializeChatButtons();
        InitializeParticles();
    }

    #endregion

    #region Utils

    protected void InitializeParticles()
    {
        ParticleSystem[] _particles = gameObject.GetComponentsInChildren<ParticleSystem>();

        if (_particles.Length <= 0)
        {
            return;
        }

        particles = new GameObject[_particles.Length];

        for (int i = 0; i < particles.Length; i++)
        {
            particles[i] = _particles[i].gameObject;
        }

        ToggleParticles(false);
    }

    protected void ToggleParticles(bool activate)
    {
        if (particles != null && particles.Length > 0)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].SetActive(activate);
            }
        }
    }

    protected void InitializeChatButtons()
    {
        chatButtonOn = GameObject.Find("ToggleChatOn");
        chatButtonOff = GameObject.Find("ToggleChatOff");

        if (chatButtonOn != null)
        {
            chatButtonOn.SetActive(false);
        }

        if (chatButtonOff != null)
        {
            chatButtonOff.SetActive(false);
        }

    }

    protected bool CanRegenerateHPorMP()
    {
        if (!hpAndMp)
        {
            hpAndMp = FindObjectOfType<LevelManager>().hpAndMp;
        }

        return hpAndMp.hpCurrentPercentage < 1f || hpAndMp.mpCurrentPercentage < 1f;
    }

    protected bool GameObjectIsPlayer(GameObject other, bool isLocal)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if (isLocal)
        {
            return playerController && playerController.localPlayer;
        }

        else
        {
            return playerController;
        }
    }

    #endregion

    #region Events

    // Attack those who enter the alert zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameObjectIsPlayer(other.gameObject, true))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.availableChatZone = gameObject;
            if (CanRegenerateHPorMP())
            {
                ToggleParticles(true);
                HpMpManager hpManager = FindObjectOfType<HpMpManager>();
                int currentHP = hpManager.hpCurrentAmount;
                Debug.Log("currentHp is: " + currentHP);
                int currentMP = hpManager.mpCurrentAmount;
                Debug.Log("currentMp is: " + currentMP);
                SendMessageToServer("PlayerEnteredChatZone/" + currentHP.ToString() + " / " + currentMP.ToString());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (GameObjectIsPlayer(other.gameObject, true))
        {
            TurnChatZoneOff();
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.availableChatZone = null;
            TurnHpMpRegenerationOff();
        }
    }

    private void SendMessageToServer(string message)
    {
        if (Client.instance)
        {
            Client.instance.SendMessageToServer(message, true);
        }
    }

    public void TurnHpMpRegenerationOff()
    {
        HpMpManager hpManager = FindObjectOfType<HpMpManager>();
        int currentHP = hpManager.hpCurrentAmount;
        int currentMP = hpManager.mpCurrentAmount;
        Debug.Log("Im leaving Chatzone. CurrentHp is: " + currentHP.ToString() + "and currentMp is: + " + currentMP.ToString() + " I'll send the message");
        hpManager.SendUpdateExitToServer();
    }
    public void TurnChatZoneOff()
    {
        ToggleParticles(false);
        hpAndMp.StopParticles();
    }
    #endregion

}