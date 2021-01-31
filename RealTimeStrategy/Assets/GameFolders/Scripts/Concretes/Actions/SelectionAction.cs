using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace RealTimeStrategy.Actions
{
    public class SelectionAction
    {
        SpriteRenderer _spriteRenderer;
        
        public SelectionAction(SpriteRenderer spriteRenderer)
        {
            _spriteRenderer = spriteRenderer;
        }

        [Client]
        public void EnableDisableSprite(bool isEnable)
        {
            _spriteRenderer.enabled = isEnable;
        }
    }    
}