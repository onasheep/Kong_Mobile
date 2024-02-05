using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    private GameObject GameOverPanel = default;
    private GameObject GameWinPanel = default;
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

        
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
        
        ResourceManager.Init();

    }

    private void Start()
    {
        GameOverPanel = GFunc.GetRootObj("Canvas").GetComponent<UIManager>().GameOverPanel;
        GameWinPanel = GFunc.GetRootObj("Canvas").GetComponent<UIManager>().GameWinPanel;
    }
    // Start is called before the first frame update

    public void GameOver()
    {
        GameOverPanel.SetActive(true);
    }

    public void GameWin()
    {
        GameWinPanel.SetActive(true);
    }

    public void SceneLoad()
    {
        SceneManager.LoadScene(0);
    }
}
