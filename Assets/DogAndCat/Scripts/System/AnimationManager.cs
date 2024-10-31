using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : SingletonManager<AnimationManager>
{
    public AnimalAnimation animalAnimation;
    protected override void Awake()
    {
        base.Awake();
    }
}
