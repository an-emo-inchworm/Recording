using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecordingListManager : MonoBehaviour
{
    public GameObject videoPreview;
    public NewBehaviourScript newBehaviourScript;
    public RawImage rawImage;
    public int count = 0;
    List<GameObject> items = new List<GameObject>();

    void Update() {
        if(Input.GetKeyDown(KeyCode.M)) {
            PopulateGrid();
        }
        if(Input.GetKeyDown(KeyCode.R)) {
            foreach (var i in items) {
                i.SetActive(false);
            }
        }
    }

    void PopulateGrid()
{
    print("populating grid");
    int textureWidth = 1280;
    int textureHeight = 800;
    Color whiteColor = Color.white;

    Texture2D whiteTexture = new Texture2D(textureWidth, textureHeight);
    Color[] pixels = new Color[textureWidth * textureHeight];

    for (int i = 0; i < pixels.Length; i++) { 
     pixels[i] = whiteColor;
    }

    whiteTexture.SetPixels(pixels);
    whiteTexture.Apply();

    rawImage.texture = whiteTexture;
    // rawImage.enabled = false;
    // rawImage.color = new Color(rawImage.color.r, rawImage.color.g, rawImage.color.b, 0);
    
    foreach (var recording in newBehaviourScript.recordings)
    {
        foreach (var i in items) {
                i.SetActive(true);
        }
        int columns = 5;
        int row = count / columns;
        int column = count % columns;
        float xOffset = 150;
        float yOffset = 150;

        Vector3 position = new Vector3(column * xOffset, -row * yOffset, 0);

        GameObject item = Instantiate(videoPreview, position, Quaternion.identity);
        count++;
        items.Add(item);
    
        print(count);

        Image previewImage = item.transform.Find("Canvas/Image").GetComponent<Image>();
        TMP_Text durationText = item.transform.Find("Canvas/Text (TMP)").GetComponent<TextMeshProUGUI>();

        Texture2D firstFrame = recording.Frames[0];

        Sprite sprite = Sprite.Create(
            firstFrame,
            new Rect(0, 0, firstFrame.width, firstFrame.height),
            new Vector2(0.5f, 0.5f)
        );

        previewImage.sprite = sprite;
        durationText.text = recording.Duration.ToString("F2") + "s";
        Button button = item.transform.Find("Canvas/Button").GetComponent<Button>();
        if(button == null) {
            print("no button");
            return;
        }
        int index = count-1;
        button.onClick.AddListener(() => PlayRecording(index));
        }
    }

    void PlayRecording(int index)
    {
        foreach (var i in items) {
                i.SetActive(false);
        }
        if (index >= 0 && index < newBehaviourScript.recordings.Count)
        {
            newBehaviourScript.PlayRecording(newBehaviourScript.recordings[index]);
        }
    }
    
    }
