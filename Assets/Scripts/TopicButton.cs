using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopicButton : MonoBehaviour
{
    const int k_TopicNameMaxLength = 28;

    [SerializeField] TMP_Text m_TopicName;
    [SerializeField] TMP_Text m_TopicNumber;
    Topic m_Topic;
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
        // Go to Details Page
        MainManager.Instance.SelectedTopic = m_Topic;
        MainManager.Instance.GoToNextPage();
    }

    // Binds Topic data
    public void SetTopic(Topic topic, int order)
    {
        m_Topic = topic;
        m_Topic.Number = order;
        m_TopicName.text = HandleName(topic.Name);
        m_TopicNumber.text = order.ToString();
    }

    string HandleName(string name)
    {
        return name.Length > k_TopicNameMaxLength ?
            name.Substring(0, k_TopicNameMaxLength) + "..." :
            name;
    }
}
