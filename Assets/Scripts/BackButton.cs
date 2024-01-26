using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour
{
    Button m_Button;

    void Awake()
    {
        m_Button = GetComponent<Button>();    
    }

    void Start()
    {
        m_Button.onClick.AddListener(HandleButtonClicked);    
    }

    void HandleButtonClicked()
    {
        MainManager.Instance.GoToPreviouPage();
    }
}
