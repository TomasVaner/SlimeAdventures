/*
 * Copyright (c) Tomas Vaner
 * https://github.com/TomasVaner
*/

using UnityEngine;

public class KeepOnLoad : MonoBehaviour
{
#region Variables
#endregion
    
#region Private Fields
#endregion

#region Unity Methods
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
#endregion

#region Public Methods
#endregion

#region Private Methods
#endregion
}