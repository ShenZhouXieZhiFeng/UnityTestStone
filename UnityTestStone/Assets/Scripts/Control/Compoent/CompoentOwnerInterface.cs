using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CompoentOwnerInterface
{
    GameObject gameObject { get; }
    Transform transform { get; }

    Animator Aniamtor { get; }

    CharacterController CharacterController { get; }
}
