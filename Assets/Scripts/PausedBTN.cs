using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausedBTN : MonoBehaviour
{

    private Button playButton;
    private Button restartButton;
    private Button quitButton;
    private UIController ui;
    public static PausedBTN SharedInstance;
    

void Awake()
{
    SharedInstance = this;
    
}
    void Start()
    {
        ui = UIController.SharedInstance;

        Button[]  buttons = GetComponentsInChildren<Button>();

        foreach (Button button in buttons)
        {
            if (button.name == "Play")
            {
                playButton = button;

                if(ui.newGame){
                playButton.onClick.AddListener(OnRestartButtonClick);
                }else{
                playButton.onClick.AddListener(OnPlayButtonClick);
                }
            }
            else if (button.name == "Restart")
            {
                restartButton = button;
                restartButton.onClick.AddListener(OnRestartButtonClick);
            }
            else if (button.name == "Quit")
            {
                quitButton = button;
                quitButton.onClick.AddListener(OnQuitButtonClick);
            }
        }
    }

    private void OnPlayButtonClick()
    {
        ui.OnPauseGame();
    }

    private void OnRestartButtonClick()
    {
        Time.timeScale = 1f;
        ui.newGame = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    private void OnQuitButtonClick()
    {
        ui.newGame = false;
        Application.Quit();
        //ใน editor มันออกเกมจริงๆไม่ได้
        Time.timeScale = 0f;
        Debug.Log("ออกเกม!");
        ui.PauseGame.SetActive(false);
    }
}
