using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour
{
    [SerializeField] TMP_Text m_LanguageNameText;
    Button m_Button;
    Language m_Language;

    void Awake()
    {
        m_Button= GetComponent<Button>();
    }

    void Start()
    {
        m_Button.onClick.AddListener(ButtonClickedHandle);
    }

    void ButtonClickedHandle()
    {
        MainManager.Instance.SelectedLanguage = m_Language;
        MainManager.Instance.GoToNextPage();
    }

    public void SetLanguage(Language language)
    {
        m_Language = language;
        m_LanguageNameText.text = language.LanguageName.ToLower();
    }
}