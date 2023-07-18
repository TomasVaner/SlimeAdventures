/*
 * Copyright (c) Tomas Vaner
 * https://github.com/TomasVaner
*/

using System;
using UnityEditor;
using UnityEngine;
using Utility;

namespace BaseClasses
{
    public class MonoBehaviourId : MonoBehaviour
    {
        [SerializeField] private string _id;

        [ContextMenu("Generate ID")]
        private void GenerateID()
        {
            _id = Guid.NewGuid().ToString();
        }

        private void Reset()
        {
            GenerateID();
        }
        
        public SerializableGuid ID => _id;
        
        
    }
}