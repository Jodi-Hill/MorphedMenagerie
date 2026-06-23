using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AfterVideoplayer : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public Button button;

    private bool buttonShown = false;

    private void Start()
    {
        button.gameObject.SetActive(false);
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void Update()
    {
        if (!buttonShown && videoPlayer.isPlaying)
        {
            button.gameObject.SetActive(true);
            buttonShown = true;
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        rawImage.gameObject.SetActive(false);
    }
}
