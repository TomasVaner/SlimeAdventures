/*
 * Copyright (c) Tomas Vaner
 * https://github.com/TomasVaner
*/

using System;
using BaseClasses;
using UnityEditor;
using UnityEngine;

namespace Manager
{
    public class AudioManager : SaveableMonoBehaviour
    {
        [SerializeField] public static AudioManager Instance;
        
    #region Variables

        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _effectSource;
        [SerializeField] private State _state;
        
    #endregion
    
    #region Private Fields
    #endregion

    #region Properties
        
        public float MusicVolume
        {
            get => _state._musicVolume;
            set 
            {
                _state._musicVolume = value;
                _musicSource.volume = _state._effectVolume;
            }
        }

        public float EffectVolume
        {
            get => _state._effectVolume;
            set
            {
                _state._effectVolume = value;
                _effectSource.volume = _state._effectVolume;
            }
        }

        public float MasterVolume
        {
            get => _state._masterVolume;
            set
            {
                _state._masterVolume = value;
                AudioListener.volume = _state._masterVolume;
            }
        }

    #endregion

    #region Unity Methods
        void Awake()
        {
            if (Instance is null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
            
            _musicSource.volume = _state._effectVolume;
            _effectSource.volume = _state._effectVolume;
            AudioListener.volume = _state._masterVolume;
        }
    #endregion

    #region Public Methods

        public void PlayEffect(AudioClip effect, float volume = 1f)
        {
            _effectSource.PlayOneShot(effect, volume);
        }

        protected override object GetSavaState()
        {
            return _state;
        }

        protected override void SetSavaState(object state)
        {
            if (state is State newState)
                _state = newState;
        }
        
    #endregion

    #region Private Methods
    #endregion

    #region Types

        [Serializable]
        private class State
        {
            [Header("Volume")]
            [Range(0f, 1f)] [SerializeField] public float _masterVolume = 1f;
            [Range(0f, 1f)] [SerializeField] public float _musicVolume = 1f;
            [Range(0f, 1f)] [SerializeField] public float _effectVolume = 1f;
        }

    #endregion
        
    }
}