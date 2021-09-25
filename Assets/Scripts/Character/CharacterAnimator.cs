using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkLeftSprites;
    [SerializeField] List<Sprite> walkRightSprites;


    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }

    SpriteRenderer spriteRenderer;

    //States
    SpriteAnimator walkDownAnimation;
    SpriteAnimator walkUpAnimation;
    SpriteAnimator walkLeftAnimation;
    SpriteAnimator walkRightAnimation;

    SpriteAnimator currentAnimation;

    bool wasMoving;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkDownAnimation = new SpriteAnimator(walkDownSprites, spriteRenderer);
        walkUpAnimation = new SpriteAnimator(walkUpSprites, spriteRenderer);
        walkLeftAnimation = new SpriteAnimator(walkLeftSprites, spriteRenderer);
        walkRightAnimation = new SpriteAnimator(walkRightSprites, spriteRenderer);

        currentAnimation = walkDownAnimation;
    }

    private void Update()
    {
        var previousAnimation = currentAnimation;

        if (MoveX == 1)
            currentAnimation = walkRightAnimation;
        else if (MoveX == -1)
            currentAnimation = walkLeftAnimation;
        else if (MoveY == 1)
            currentAnimation = walkUpAnimation;
        else if (MoveY == -1)
            currentAnimation = walkDownAnimation;

        if ((currentAnimation != previousAnimation) || (wasMoving ^ IsMoving))
            currentAnimation.Start();

        if (IsMoving)
            currentAnimation.HandleUpdate();
        else
            spriteRenderer.sprite = currentAnimation.Frames[0];

        wasMoving = IsMoving;
    }
}
