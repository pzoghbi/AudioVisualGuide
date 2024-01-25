using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainManager : Singleton<MainManager>
{
    [HideInInspector] public Language SelectedLanguage;

    [SerializeField] List<GameObject> m_Pages = new();
    int pageIndex = 0;

    new void Awake()
    {
        base.Awake();
        if (!m_Pages.Any())
        {
            Debug.LogError("Pages not assigned.");
            // Fallback method etc.
        }
    }

    void Start()
    {
        SetActivePage(pageIndex);    
    }

    void SetActivePage(int indexToSet)
    {
        m_Pages.ForEach(p => p.SetActive(false));
        m_Pages[pageIndex = indexToSet].SetActive(true);
    }

    public void GoToNextPage()
    {
        pageIndex = (pageIndex + 1) % m_Pages.Count; 
        SetActivePage(pageIndex);
    }

    public void GoToPreviouPage()
    {
        pageIndex = (pageIndex - 1 + m_Pages.Count) % m_Pages.Count;
        SetActivePage(pageIndex);
    }
}