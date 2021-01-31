using System.Collections;
using System.Collections.Generic;
using Mirror;
using RealTimeStrategy.Abstracts.Controllers;
using RealTimeStrategy.Abstracts.Movements;
using RealTimeStrategy.Movements;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace RealTimeStrategy.Controllers
{
    public class UnitController : NetworkBehaviour, IEntityController
    {
        IMover _mover;
        Vector3 _position;
        bool _isMousePressed;

        private void Awake()
        {
            _mover = new Mover(GetComponent<NavMeshAgent>(), Camera.main);
        }
        
        private void Update()
        {
            if (!hasAuthority) return;
            
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                _position = Mouse.current.position.ReadValue();
                _isMousePressed = true;
            }
        }

        [ClientCallback]
        private void FixedUpdate()
        {
            if (_isMousePressed)
            {
                _mover.Move(_position);
                _isMousePressed = false;
            }
        }
    }
}