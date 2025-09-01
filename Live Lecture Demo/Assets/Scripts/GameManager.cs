using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int PlayerLayer = 6;   
    private static int MonsterLayer = 7;

    [SerializeField] private float gameTime = 60.0f;

    private float timer;
    public float TimeRemaining
    {
        get
        {
            return timer;
        }
    }


    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("No Game Manager Instance");
            }

            return instance;
        }
    }

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one game manager in scene");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        timer = gameTime;
    }


    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            Debug.Log("Game Over - Time has run out");
        }

        timer -= Time.deltaTime;
        
    }
}
