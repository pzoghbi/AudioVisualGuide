# AudioVisualGuide
A mobile (Android) UI Flow Application Made with Unity and programmed in C#.  
The project features UI workflow, data loading and more.  
The **language** and **topic** menus are populated programmatically through the received data.

## Installation
- Requires Unity 2021.3.5f1
1. Clone the project repository to your local drive
2. Open Unity Hub → Add → Find and open project directory
3. Open AudioVisualGuide

### Packages
Based on Unity 2D Mobile template
- Adaptive performance (Mobile feature) - 3.0.3
- TextMeshPro 3.0.7
- Unity UI - 1.0.0

### Setup
Heavily depends on the `Assets/Resources/Public/resources.json` manifest file.  
The file contains information about how the data will be loaded and deserialized.  
Several classes support deserialization:  
- `TranslatedContent`
- `Language`
- `Media`
- `Topic`
- `Photo` 

### Mobile preview
![image](https://github.com/pzoghbi/AudioVisualGuide/assets/10575726/4998ed8b-b839-4b2c-9414-d870de809cda)

### Tablet preview
![image](https://github.com/pzoghbi/AudioVisualGuide/assets/10575726/6e8f39dd-0321-4fed-911f-b9882abee74b)

# Documentation
#### IPopulatable
Interface which has two methods: `ClearMenu` and `PopulateMenu.`  
Use it on menus which are dynamically created (populated).  
- _For example, topic links are populated based on the content in the selected language._ 
- _For example, it can be used by a higher-level class to automatically perform clear and populate._

```cs
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
```
