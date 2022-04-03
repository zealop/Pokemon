using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator
{
    private readonly SpriteRenderer renderer;
    private readonly float frameRate;

    private int currentFrame;
    private float timer;

    public List<Sprite> Frames { get; }

    public SpriteAnimator(List<Sprite> frames, SpriteRenderer renderer, float frameRate = 0.16f)
    {
        Frames = frames;
        this.renderer = renderer;
        this.frameRate = frameRate;
    }

    public void Start()
    {
        currentFrame = 0;
        timer = 0;
        renderer.sprite = Frames[0];
    }

    public void HandleUpdate()
    {
        timer += Time.deltaTime;
        if (timer > frameRate)
        {
            currentFrame = (currentFrame + 1) % Frames.Count;
            renderer.sprite = Frames[currentFrame];
            timer -= frameRate;
        }
    }

}
