#if UNITY_EDITOR
using UnityEditor;
#endif

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(1000)]
public class MenuUIHandler : MonoBehaviour
{
    public GameObject nicknameInput;

    public void StartGame()
    {
        var highscoreManager = GameObject.Find("HighscoreManager").GetComponent<HighscoreManager>().getInstance();
        var currNickname = nicknameInput.GetComponent<TMP_InputField>().text;
        if (currNickname.Length == 0)
        {
            currNickname = "Anonymous";
        }
        
        highscoreManager.setNickname(currNickname);
        SceneManager.LoadScene("Scenes/main");
    }
    
    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}
