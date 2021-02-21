using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace RealTimeStrategy.Combats
{
    public class Targeter : NetworkBehaviour
    {
        [SerializeField] Targetable _targetable;
        
        [Command]
        public void SetTarget(GameObject targetGameObject)
        {
            if (!targetGameObject.TryGetComponent<Targetable>(out _targetable)) return;
        }

        [Server]
        public void ClearTarget()
        {
            _targetable = null;
        }
    }    
}

