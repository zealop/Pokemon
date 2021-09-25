using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator
{
    SpriteRenderer renderer;
    List<Sprite> frames;
    float frameRate;

    int currentFrame;
    float timer;

    public List<Sprite> Frames { get => frames; }
    public SpriteAnimator(List<Sprite> frames, SpriteRenderer renderer, float frameRate = 0.16f)
    {
        this.frames = frames;
        this.renderer = renderer;
        this.frameRate = frameRate;
    }

    public void Start()
    {
        currentFrame = 0;
        timer = 0;
        renderer.sprite = frames[0];
    }

    public void HandleUpdate()
    {
        timer += Time.deltaTime;
        if (timer > frameRate)
        {
            currentFrame = (currentFrame + 1) % frames.Count;
            renderer.sprite = frames[currentFrame];
            timer -= frameRate;
        }
    }

}
