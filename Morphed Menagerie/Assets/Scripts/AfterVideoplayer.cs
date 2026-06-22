using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class AfterVideoplayer : MonoBehaviour
{
    public RawImage rawImage;
    public VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        rawImage.gameObject.SetActive(false);
    }
}
