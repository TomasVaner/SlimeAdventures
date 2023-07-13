using UnityEditor;
using UnityEngine;

namespace Utility
{
    public class MonoBehaviourId : MonoBehaviour
    {
        [SerializeField] private GUID id;

        [ContextMenu("Generate ID")]
        private void GenerateID()
        {
            id = GUID.Generate();
        }
        
        public GUID ID => id;
    }
}