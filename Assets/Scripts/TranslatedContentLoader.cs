using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

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
    TranslatedContent m_TranslatedContent;
    public event Action TranslatedContentLoaded;

    string RepositoryURL => $"{k_BaseURL}/{k_UserName}/{k_Repository}/{k_Branch}/";
    string DataPath => Application.persistentDataPath;
    string JsonFilePath => $"{DataPath}/{k_JsonFileName}";
    public TranslatedContent TranslatedContent => m_TranslatedContent;

    IEnumerator Start()
    {
        string url = $"{RepositoryURL}/{k_PublicFolderPath}/{k_JsonFileName}";
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

        TranslatedContentLoaded?.Invoke();
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
                    else {
                        // download audio
                        yield return DownloadFileRi(
                            $"{RepositoryURL}/{k_PublicFolderPath}{media.FilePath}",
                            $"{DataPath}{media.FilePath}"
                        );
                    }
                }
            }
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
}

[Serializable]
public class Topic
{
    public string Name;
    public string Details;
    public List<Media> Media;
}

[Serializable]
public class Photo
{
    public string Path;
    public string Name;
}