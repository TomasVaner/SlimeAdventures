/*
 * Copyright (c) Tomas Vaner
 * https://github.com/TomasVaner
*/

using System;
using Interfaces;
using UnityEditor;
using UnityEngine;
using Utility;

namespace BaseClasses
{
    public abstract class SaveableMonoBehaviour : MonoBehaviourId, ISaveable
    {
        public Tuple<SerializableGuid, object> Save()
        {
            return Tuple.Create(ID, GetSavaState());
        }

        protected abstract object GetSavaState();

        public void Load(Tuple<SerializableGuid, object> state)
        {
            if (state.Item1 == ID)
                SetSavaState(state.Item2);
        }
        
        protected abstract void SetSavaState(object state);
    }
}