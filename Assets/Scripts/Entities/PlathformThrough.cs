using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(PlatformEffector2D))]
    public class PlatformThrough : MonoBehaviour
    {
        private Collider2D coll;

    #region Unity Functions

        private void Awake()
        {
            coll = GetComponent<Collider2D>();
        }

    #endregion
    
    
    }
}
