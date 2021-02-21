using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace RealTimeStrategy.Combats
{
    public class Targetable : NetworkBehaviour
    {
        [SerializeField] Transform _aimAtPoint;

        public Transform AimPoint => _aimAtPoint;
        
        
    }    
}

