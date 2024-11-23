using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject OptionPanel;
    public GameObject MainPanel;
    public GameObject MainBTN;
    public void PlayGame()
    {
        /* SceneManager.LoadSceneAsync(1);  เรียกตาม Index*/
        SceneManager.LoadScene("Level1"); //เรียกตามชื่อ scene
    }
    public void OpenOption()
    {
        MainBTN.SetActive(false);
        OptionPanel.SetActive(true);

    }
    public void CancelOption()
    {
        MainBTN.SetActive(true);
        OptionPanel.SetActive(false);
    }
    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

}
