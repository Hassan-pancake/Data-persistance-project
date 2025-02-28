using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public TMP_InputField playername;

    public void nameField()
    {
        if (!string.IsNullOrEmpty(playername.text))
        {
            PlayerPrefs.SetString("PlayerName", playername.text); // Save name
            PlayerPrefs.Save();
        }
    }
    public void startGame()
    {
        nameField();
        SceneManager.LoadScene(1);

    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop Play Mode in Editor
#else
        Application.Quit(); // Quit the game in a built application
#endif
    }

}

