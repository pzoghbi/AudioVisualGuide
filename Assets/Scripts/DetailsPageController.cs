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
    [SerializeField] TMP_Text m_AudioTime;
    [SerializeField] Button m_PlayButton;
    [SerializeField] Sprite m_PlaySprite;
    [SerializeField] Sprite m_PauseSprite;

    AudioSource m_AudioSource;
    int galleryImageIndex = 0;
    float m_GalleryChangePhotoDelay = 5;

    void Awake()
    {
        m_AudioSource = m_AudioSeeker.GetComponent<AudioSource>();
        m_AudioSeeker.onValueChanged.AddListener(SeekAudio);
        m_PlayButton.onClick.AddListener(m_AudioSource.Play);
    }

    void Update()
    {
        UpdateAudioSeeker();
    }

    void UpdateAudioSeeker()
    {
        if (m_AudioSource.clip)
        {
            m_AudioSeeker.value = m_AudioSource.time / m_AudioSource.clip.length;
            m_AudioTime.text = $"{GetFloatAsTime(m_AudioSource.time)} / {GetFloatAsTime(m_AudioSource.clip.length)}";
        }
    }

    string GetFloatAsTime(float time)
    {
        const float spm = 60;
        int minutes = (int) (time / spm);
        int seconds = (int) (time - minutes * spm);
        string str_minutes = minutes < 10 ? $"0{minutes}" : minutes.ToString();
        string str_seconds = seconds < 10 ? $"0{seconds}" : seconds.ToString();
        return $"{str_minutes}:{str_seconds}";
    }

    public void SeekAudio(float value)
    {
        m_AudioSource.time = value * m_AudioSource.clip.length;
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
        m_AudioSource.clip = topic.Media[1].AudioClip;
        StartCoroutine(SequenceGalleryPhotosRi());
    }

    IEnumerator SequenceGalleryPhotosRi()
    {
        yield return new WaitForSecondsRealtime(m_GalleryChangePhotoDelay);
        var media = MainManager.Instance.SelectedTopic.Media[0];
        if (media.Photos.Count == 0)
            yield break;

        galleryImageIndex = (galleryImageIndex + 1) % media.Photos.Count;
        m_GalleryPhoto.sprite = media.Photos[galleryImageIndex].Sprite;

        StartCoroutine(SequenceGalleryPhotosRi());
    }
}
