using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject m_ingameMenu;

    private void Update()
    {
        if (m_ingameMenu && Input.GetButtonDown("Cancel"))
        {
            TogglePanel(m_ingameMenu);
        }
    }

    public void TogglePanel(GameObject _panel)
    {
        _panel.SetActive(!_panel.activeSelf);
    }

    public void ChangeScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }
}