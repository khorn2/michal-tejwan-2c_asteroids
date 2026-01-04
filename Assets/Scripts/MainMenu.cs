using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayGame();
        }
    }
        public void OpenCredits()
    {
        Debug.Log("CLICK CREDITS");
        SceneManager.LoadScene(2); 
    }
}
