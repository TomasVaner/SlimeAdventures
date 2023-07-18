/*
 * Copyright (c) Tomas Vaner
 * https://github.com/TomasVaner
*/

using System;
using Manager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controller
{
    public class PlayerAudioController : MonoBehaviour
    {
    #region Variables

        [SerializeField] private AudioClip[] _jumpAudio;
    
    #endregion
    
    #region Private Fields
    #endregion

    #region Unity Methods
    #endregion

    #region Public Methods

        public void PlayJump()
        {
            var index = Random.Range(0, _jumpAudio.Length);
            AudioManager.Instance.PlayEffect(_jumpAudio[index]);
            
        }
    #endregion

    #region Private Methods
    #endregion
    }
}