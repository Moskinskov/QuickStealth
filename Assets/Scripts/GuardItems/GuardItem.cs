using UnityEngine;

namespace Assets.Scripts.GuardItems
{
    public abstract class GuardItem : MonoBehaviour
    {
        public abstract void OnStart();
        public abstract void OnUpdate();
    }
}