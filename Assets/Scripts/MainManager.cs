using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainManager : Singleton<MainManager>
{
    [HideInInspector] public Language SelectedLanguage;
    public Action<Topic> OnTopicSelected;

    [SerializeField] List<GameObject> m_Pages = new();
    int pageIndex = 0;

    public Topic SelectedTopic { get; set; } = null;

    new void Awake()
    {
        base.Awake();
        if (!m_Pages.Any())
        {
            Debug.LogWarning("Pages not assigned.");
            // Fallback method ... Example:
            foreach(Transform transform in FindObjectOfType<Canvas>().transform)
            {
                m_Pages.Add(transform.gameObject);
            }
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