using UnityEngine;
namespace SLT
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instant = null;
        public static T Instant
        {
            get
            {
                if (_instant == null)
                {
                    Debug.LogWarning("==> Singleton dosent exist!!! <==");
                    _instant = FindAnyObjectByType<T>();
                }
                return _instant;
            }

        }  
        protected virtual void Awake()
        {
            if (_instant != null && _instant.gameObject.GetInstanceID() != this.gameObject.GetInstanceID()) 
                Destroy(this.gameObject);
            else
                _instant = this.GetComponent<T>();
        }
    }
}
