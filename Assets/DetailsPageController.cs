using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailsPageController : MonoBehaviour
{
    [Header("Topic")]
    [SerializeField] TMP_Text m_TopicNumber;
    [SerializeField] TMP_Text m_TopicName;
    [SerializeField] TMP_Text m_TopicDetails;
    [Header("Gallery")]
    [SerializeField] Image m_GalleryPhoto;
    [Header("Controls")]
    [SerializeField] Slider m_AudioSeeker;
    [SerializeField] Button m_PlayButton;
    [SerializeField] Sprite m_PlaySprite;
    [SerializeField] Sprite m_PauseSprite;

    AudioSource m_AudioSource;

    void Awake()
    {
        m_AudioSource = m_AudioSeeker.GetComponent<AudioSource>();
        m_AudioSeeker.onValueChanged.AddListener(SeekAudio);
    }
    
    public void SeekAudio(float value)
    {
        m_AudioSource.time = value;
    }

    void OnEnable()
    {
        if (MainManager.Instance.SelectedTopic != null)
        {
            SetTopic(MainManager.Instance.SelectedTopic);
        }
    }

    public void SetTopic(Topic topic)
    {
        if (m_TopicName) m_TopicName.text = topic.Name;
        if (m_TopicNumber) m_TopicNumber.text = topic.Number.ToString();
        if (m_TopicDetails) m_TopicDetails.text = topic.Details;
        m_GalleryPhoto.sprite = topic.Media[0].Photos[0].Sprite;
    }
}
