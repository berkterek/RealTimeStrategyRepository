using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RealTimeStrategy.Abstracts.Movements
{
    public interface IMover
    {
        void Move(Vector3 position);
        void Tick();
    }    
}

