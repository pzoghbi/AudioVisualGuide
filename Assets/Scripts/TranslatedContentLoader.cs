using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static TranslatedContentLoader;

public class TranslatedContentLoader : MonoBehaviour
{
    //////////////////////////////////////////// Repository Info
    const string k_BaseURL = "https://raw.githubusercontent.com";
    const string k_UserName = "pzoghbi";
    const string k_Repository = "AudioVisualGuide";
    const string k_Branch = "main";
    //////////////////////////////////////////// Public Folder Info
    const string k_PublicFolderPath = "Assets/Resources/Public";
    const string k_JsonFileName = "resources.json";

    [SerializeField] TMP_Text m_LoadingText;
    [SerializeField] Image m_Logo;
    TranslatedContent m_TranslatedContent;
    public event Action ContentLoaded;
    public event Action ContentLoading;

    string RepositoryURL => $"{k_BaseURL}/{k_UserName}/{k_Repository}/{k_Branch}/";
    string DataPath => Application.persistentDataPath;
    string JsonFilePath => $"{DataPath}/{k_JsonFileName}";
    public TranslatedContent TranslatedContent => m_TranslatedContent;

    void OnEnable()
    {
        ContentLoaded += OnContentLoaded;
        ContentLoading += OnContentLoading;
    }

    void OnDisable ()
    {
        ContentLoaded -= OnContentLoaded;    
        ContentLoading -= OnContentLoading;    
    }

    void OnContentLoaded()
    {
        if (m_Logo.TryGetComponent<ActiveObjectRotator>(out var rotator))
        {
            rotator.ResetRotation();
            rotator.enabled = false;
        }
    }

    void OnContentLoading()
    {
        if (m_Logo.TryGetComponent<ActiveObjectRotator>(out var rotator))
        {
            rotator.enabled = true;
        }
        else
        {
            m_Logo.AddComponent<ActiveObjectRotator>();
        }
    }

    IEnumerator Start()
    {
        string url = $"{RepositoryURL}/{k_PublicFolderPath}/{k_JsonFileName}";
        ContentLoading?.Invoke();
        yield return DownloadFileRi(url, JsonFilePath, OnJsonManifestDownloaded);
    }

    IEnumerator DownloadFileRi(string url, string outputFilePath, Action callback = null)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(url))
        {
            webRequest.downloadHandler = new DownloadHandlerFile(outputFilePath);
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string loadingText = $"Successfully downloaded {outputFilePath.Split("/").Last()}.";
                m_LoadingText.text = loadingText;
                callback?.Invoke();
            }
            else
            {
                Debug.LogError(webRequest.error);
                Debug.LogError(webRequest.url);
                // Handle error (not doing)
                yield break;
            }
        }
    }

    void OnJsonManifestDownloaded()
    {
        StartCoroutine(OnJsonManifestDownloadedRi());
    }

    IEnumerator OnJsonManifestDownloadedRi()
    {
        string json = string.Empty;

        // Parse downloaded file
        try
        {
            json = File.ReadAllText(JsonFilePath);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            // Handle error (etc. use fallback)
            yield break;
        }

        // Cache translated content
        m_TranslatedContent = ParseTranslatedContent(json);
        if (m_TranslatedContent == null) yield break;

        yield return DownloadMedia(m_TranslatedContent);

        ContentLoaded?.Invoke();
    }

    TranslatedContent ParseTranslatedContent(string json)
    {
        return JsonUtility.FromJson<TranslatedContent>(json);
    }

    IEnumerator DownloadMedia(TranslatedContent translatedContent)
    {
        foreach (var language in translatedContent.TranslatedContents)
        {
            foreach (var topic in language.Topics)
            {
                foreach (var media in topic.Media)
                {
                    if (media.Photos.Count > 0)
                    {
                        foreach(Photo photo in media.Photos)
                        {
                            // download images
                            yield return DownloadFileRi(
                                $"{RepositoryURL}/{k_PublicFolderPath}{photo.Path}",
                                $"{DataPath}{photo.Path}"
                            );
                        }
                    }
                    else
                    {
                        // download audio
                        string url = $"{RepositoryURL}/{k_PublicFolderPath}{media.FilePath}";
                        string filePath = $"{DataPath}{media.FilePath}";
                        yield return DownloadFileRi(url, filePath);
                        yield return BindAudioClipRi(media);
                    }
                }
            }
        }
    }

    IEnumerator BindAudioClipRi(Media media)
    {
        string path = $"{DataPath}{media.FilePath}";
        if (!File.Exists(path))
        {
            Debug.Log($"File doesn't exist on {path}.");
            yield break;
        }

        var webRequest = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.UNKNOWN);
        using (webRequest)
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
                yield break;
            }

            media.AudioClip = DownloadHandlerAudioClip.GetContent(webRequest);
            media.AudioClip.name = media.Name;
        }
    }
}

[Serializable]
public class TranslatedContent
{
    public List<Language> TranslatedContents;
}

[Serializable]
public class Language
{
    public int LanguageId;
    public string LanguageName;
    public List<Topic> Topics;
}

[Serializable]
public class Media
{
    public string Name;
    public string FilePath;
    public List<Photo> Photos;
    public AudioClip AudioClip;
}

[Serializable]
public class Topic
{
    public string Name;
    public string Details;
    public List<Media> Media;

    public int Number;
}

[Serializable]
public class Photo
{
    public string Path;
    public string Name;
    public Sprite Sprite { get => GetSprite(); set => m_Sprite = value; }
    Sprite m_Sprite;

    public Sprite GetSprite()
    {
        if (m_Sprite == null)
        {
            string path = $"{Application.persistentDataPath}{Path}";
            if (File.Exists(path))
            {
                var textureBytes = File.ReadAllBytes($"{path}");
                Texture2D texture = new Texture2D(2, 2);
                if (texture.LoadImage(textureBytes))
                {
                    var rect = new Rect(0, 0, texture.width, texture.height);
                    m_Sprite = Sprite.Create(texture, rect, Vector2.zero);
                }
            }
            else
            {
                Debug.LogError($"Image doesn't exist on {path}.");
            }
        }

        return m_Sprite;
    }
}