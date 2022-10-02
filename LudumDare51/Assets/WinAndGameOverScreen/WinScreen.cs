
public class WinScreen : UnityEngine.MonoBehaviour
{
    [UnityEngine.SerializeField] Timer timer;

    private void Start()
    {
        timer.On5MinutesPassed += Timer_On5MinutesPassed1;
        gameObject.SetActive(false);
    }

    private void Timer_On5MinutesPassed1()
    {
        gameObject.SetActive(true);
    }

    public void ReloadLevel()
    {
        timer.On5MinutesPassed -= Timer_On5MinutesPassed1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
