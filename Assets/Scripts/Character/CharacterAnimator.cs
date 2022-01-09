using System;
using System.Collections.Generic;
using UnityEngine;

public enum FacingDirection { Up, Down, Left, Right }
public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] private List<Sprite> walkDownSprites;
    [SerializeField] private List<Sprite> walkUpSprites;
    [SerializeField] private List<Sprite> walkLeftSprites;
    [SerializeField] private List<Sprite> walkRightSprites;
    [SerializeField] private FacingDirection defaultDirection = FacingDirection.Down;

    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }
    public FacingDirection DefaultDirection => defaultDirection;
    private SpriteRenderer spriteRenderer;

    //States
    private SpriteAnimator walkDownAnimation;
    private SpriteAnimator walkUpAnimation;
    private SpriteAnimator walkLeftAnimation;
    private SpriteAnimator walkRightAnimation;

    private SpriteAnimator currentAnimation;

    private bool wasMoving;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkDownAnimation = new SpriteAnimator(walkDownSprites, spriteRenderer);
        walkUpAnimation = new SpriteAnimator(walkUpSprites, spriteRenderer);
        walkLeftAnimation = new SpriteAnimator(walkLeftSprites, spriteRenderer);
        walkRightAnimation = new SpriteAnimator(walkRightSprites, spriteRenderer);

        SetFacingDirection(defaultDirection);

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

        if (currentAnimation != previousAnimation || wasMoving ^ IsMoving)
            currentAnimation.Start();

        if (IsMoving)
            currentAnimation.HandleUpdate();
        else
            spriteRenderer.sprite = currentAnimation.Frames[0];

        wasMoving = IsMoving;
    }

    private void SetFacingDirection(FacingDirection direction)
    {
        switch (direction)
        {
            case FacingDirection.Up:
                MoveY = 1;
                break;
            case FacingDirection.Down:
                MoveY = -1;
                break;
            case FacingDirection.Left:
                MoveX = -1;
                break;
            case FacingDirection.Right:
                MoveX = 1;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
}
