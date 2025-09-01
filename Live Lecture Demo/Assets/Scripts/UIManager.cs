using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    private GameManager gameManager;

    [SerializeField] private TextMeshProUGUI timeLeftText;

    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("No UIManager in the scene!");
            }

            return instance;
        }
    }

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("A UIManager already exists in this scene!");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeftText.text = string.Format("Time Left: {0}", (int) gameManager.TimeRemaining);
        
    }
}
