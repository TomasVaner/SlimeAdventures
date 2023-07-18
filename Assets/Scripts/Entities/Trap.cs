/*
 * Copyright (c) Tomas Vaner
 * https://github.com/TomasVaner
*/

using System;
using System.Linq;
using UnityEngine;

namespace Entities
{
    public class Trap : MonoBehaviour
    {
    #region Variables

        [SerializeField] private float _damage;
        [SerializeField] private float _knockback;

    #endregion

    #region Unity functions

        private void OnCollisionEnter2D(Collision2D col)
        {
            var creature = col.gameObject.GetComponent<Creature>();
            if (!(creature is null))
            {
                creature.Damage(_damage, col.contacts.Length > 0 ? col.contacts.First().point : null, _knockback);
            }
        }

    #endregion
        
    }
}
