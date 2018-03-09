using UnityEngine;

public class PlayerPicker : MonoBehaviour
{
    public GameObject m_inputPanel;
    public GameObject m_textInput1;
    public GameObject m_textInput2;

    private InputManager m_input;

    private void Start()
    {
        m_input = InputManager.Get;
    }

    private void Update()
    {
        if (Input.GetKeyDown(m_input.k1Jump))
        {
            m_input.AddToPlayerSet(m_input.k1Jump);
            HideUI();
        }
        else if (Input.GetKeyDown(m_input.k2Jump))
        {
            m_input.AddToPlayerSet(m_input.k2Jump);
            HideUI();
        }
        else if (Input.GetKeyDown(m_input.cJump))
        {
            m_input.AddToPlayerSet(m_input.cJump);
            HideUI();
        }
    }

    private void HideUI()
    {
        if (InputManager.PlayerCount == 1)
        {
            m_textInput1.SetActive(false);
        }
        else if (InputManager.PlayerCount >= 2)
        {
            m_textInput1.SetActive(true);
            m_inputPanel.SetActive(false);
        }
    }
}