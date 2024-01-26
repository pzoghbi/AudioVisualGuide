using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopicMenu : MonoBehaviour, IPopulatableMenu
{
    [SerializeField] TopicButton m_TopicButtonPrefab;
    List<Topic> m_Topics = new List<Topic>();
    IPopulatableMenu m_IMenu;

    void Awake()
    {
        m_IMenu = this;    
    }

    void Start()
    {
        MainManager.Instance.OnLanguageSelected += SetTopics;
    }

    void SetTopics(List<Topic> topics)
    {
        m_IMenu.ClearMenu();
        m_Topics = topics;
        m_IMenu.PopulateMenu();
    }

    void IPopulatableMenu.ClearMenu()
    {
        foreach(var topicButton in transform.GetComponentsInChildren<TopicButton>())
        {
            Destroy(topicButton.gameObject);
        }
    }

    void IPopulatableMenu.PopulateMenu()
    {
        int order = 1;
        foreach(var topic in m_Topics)
        {
            Instantiate(m_TopicButtonPrefab, transform)
                .SetTopic(topic, order++);
        }
    }

    void OnDestroy()
    {
        MainManager.Instance.OnLanguageSelected -= SetTopics;    
    }
}
