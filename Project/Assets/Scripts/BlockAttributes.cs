using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAttributes : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How many points the block grants on a successful landing")]
    private int _points;

    [SerializeField]
    [Tooltip("Pitch of the block's collision sound effect")]
    [Range(0.1f, 2f)]
    private float _soundPitch;

    #region Properties
    public int Points
    {
        get
        {
            return _points;
        }
        private set
        {
        }
    }

    public float SoundPitch
    {
        get
        {
            return _points;
        }
        private set
        {
        }
    }
    #endregion
}
