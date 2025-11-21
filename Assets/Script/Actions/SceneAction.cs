using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneAction : MonoBehaviour
{
    [SerializeField] private Sprite actionIcon = null;

    public abstract void interaction();

    public Sprite GetActionIcon()
    {
        return actionIcon;
    }


}
