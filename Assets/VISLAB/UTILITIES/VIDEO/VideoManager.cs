using SFB;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

public class Resolution
{
    public int x { get; set; }
    public int y { get; set; }
}


[RequireComponent(typeof(VideoPlayer))]

/**
 * Responsible for loading videos, starting, and pausing.
 * When a video is loaded, it will update the render texture size to match the video, and
 * apply it to the material rendering the texture.
 */
public class VideoManager : NetworkBehaviour
{
    [SerializeField]
    RenderTexture renderTexture;
    VideoPlayer video;

    [SerializeField]
    Material material;

    /// <summary>
    /// When set to true the video ready event will update the render texture
    /// </summary>
    bool resetTexture = true;

    ExtensionFilter[] extensions = new[] {
                    new ExtensionFilter("Video", "mov", "avi", "asf", "m4v", "mp4", "mpeg", "ogv", "webm", "wmv" ),
                };

    void Awake()
    {
        video = GetComponent<VideoPlayer>();
        video.prepareCompleted += onVideoReady;

    }

    /// <summary>
    /// Create a new copy of the current render texture, and apply the provided resolution and updates the
    /// material displaying the video with the new render texture.
    /// Stops the video before applying new render texture.
    /// </summary>
    /// <param name="resolution">resolution to set</resolutionaram>
    void SetRenderResolution(Resolution resolution)
    {
        if (video.renderMode == VideoRenderMode.RenderTexture)
        {
            video.Stop();
            var renderA = new RenderTexture(video.targetTexture);
            renderA.width = resolution.x;
            renderA.height = resolution.y;
            renderA.Create();
            video.targetTexture = renderA;
            material.SetTexture("_MainTex", renderA);
            video.Play();
        }

    }

    void Update()
    {
        if (isServer)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                resetTexture = false;
                if (video.isPlaying)
                {
                    video.Pause();
                }
                else
                {
                    video.Play();

                }
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                resetTexture = true;
                var path = StandaloneFileBrowser.OpenFilePanel("Select a video", "", extensions, false);
                if (path.Length > 0)
                {
                    video.url = path[0];
                    video.Play();
                }


            }
        }

    }

    /// <summary>
    /// Updates the render texture when resetTexture is true, else it does nothing.
    /// </summary>
    /// <param name="videoPlayer">the target video player which is ready</param>
    void onVideoReady(VideoPlayer videoPlayer)
    {
        if (resetTexture)
        {
            SetRenderResolution(new Resolution { x = (int)videoPlayer.width, y = (int)videoPlayer.height });
        }
    }
}
