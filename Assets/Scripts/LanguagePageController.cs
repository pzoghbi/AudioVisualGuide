using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LanguagePageController : MonoBehaviour, IPopulatableMenu
{
    [SerializeField] GameObject m_LanguageMenu;
    [SerializeField] LanguageButton m_LanguageButtonPrefab;
    [SerializeField] TranslatedContentLoader m_ContentLoader;
    [SerializeField] TMP_Text m_LoadingText;
    [SerializeField] TMP_Text m_LoadingInfoText;
    IPopulatableMenu m_IMenu;
    
    void Awake()
    {
        m_IMenu = this;
    }

    void Start()
    {
        m_IMenu.ClearMenu();    
    }

    void OnEnable()
    {
        m_ContentLoader.ContentLoaded += OnContentLoaded;
    }

    void OnDisable()
    {
        m_ContentLoader.ContentLoaded -= OnContentLoaded;
    }

    void OnContentLoaded()
    {
        m_IMenu.PopulateMenu();
        // Hide loading text
        m_LoadingText.gameObject.SetActive(false);  
        m_LoadingInfoText.gameObject.SetActive(false);
    }

    void IPopulatableMenu.PopulateMenu()
    {
        CreateLanguageButtons();
    }

    void IPopulatableMenu.ClearMenu()
    {
        foreach(LanguageButton button in m_LanguageMenu.transform.GetComponentsInChildren<LanguageButton>())
        {
            Destroy(button.gameObject);
        }
    }

    void CreateLanguageButtons()
    {
        foreach (var language in m_ContentLoader.TranslatedContent.TranslatedContents)
        {
            Instantiate(m_LanguageButtonPrefab, m_LanguageMenu.transform)
                .SetLanguage(language);
        }
    }

}

public interface IPopulatableMenu
{
    void PopulateMenu();
    void ClearMenu();
}