using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] AngryBar angryBar;

    private void Start()
    {
        angryBar.OnAngryBarFull += AngryBar_OnAngryBarFull;
        gameObject.SetActive(false);
    }

    private void AngryBar_OnAngryBarFull()
    {
        gameObject.SetActive(true);
    }

    public void ReloadLevel()
    {
        angryBar.OnAngryBarFull -= AngryBar_OnAngryBarFull;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
