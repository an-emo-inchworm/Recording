using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    int camIndex = 0;
    WebCamTexture webcam;
    public RawImage display;
    public float delay = 0.01f;
    public Text startStopText;
    private List<Texture2D> frames = new List<Texture2D>();
    private bool isRecording = false;
    private bool isPlaying = false;
    public Renderer renderer;

    public List<Recording> recordings = new List<Recording>();

    void Start()
    {
        recordings = new List<Recording>();
        webcam = new WebCamTexture();
        webcam.Play();
    }

    void StartRecording()
    {
        print("Start recording");
        display.texture = webcam;
        frames.Clear();
        isRecording = true;
    }

    void StopRecording()
    {
        print("Stop recording");
        isRecording = false;
        SaveFramesToVideo();
    }

    void SaveFramesToVideo()
    {
        print("saved frames to video");
        for (int i = 0; i < frames.Count; i++)
        {
            byte[] bytes = frames[i].EncodeToPNG();
        }
        float videoDuration = frames.Count * delay;
        recordings.Add(new Recording { Frames = new List<Texture2D>(frames), Duration = videoDuration });
    }

    void RecordFrame()
    {
        if (isRecording)
        {
            Texture2D frame = new Texture2D(webcam.width, webcam.height);
            frame.SetPixels(webcam.GetPixels());
            frame.Apply();
            frames.Add(frame);
        }
    }

    void DisplayFrame(Texture2D frame)
    {
        display.texture = frame;
    }

    void StartPlayback()
    {
        print("starting playback");
        if (!isPlaying && frames.Count > 0)
        {
            StartCoroutine(Playback());
        }
    }

    IEnumerator Playback()
    {
        print("playbacking");
        isPlaying = true;
        for (int i = 0; i < frames.Count; i++)
        {
            DisplayFrame(frames[i]);
            yield return new WaitForSeconds(delay);
        }
        isPlaying = false;
    }

    public void PlayRecording(Recording recording)
    {
        if (!isPlaying && recording.Frames.Count > 0)
        {
            StartCoroutine(PlaybackRecording(recording));
        }
    }

    IEnumerator PlaybackRecording(Recording recording)
    {
        print("playing recording");
        isPlaying = true;
        foreach (var frame in recording.Frames)
        {
            DisplayFrame(frame);
            yield return new WaitForSeconds(delay);
        }
        isPlaying = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartRecording();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            StopRecording();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartPlayback();
        }
    }

    void LateUpdate()
    {
        RecordFrame();
    }

    public class Recording
    {
        public List<Texture2D> Frames { get; set; }
        public float Duration { get; set; }
    }
}
