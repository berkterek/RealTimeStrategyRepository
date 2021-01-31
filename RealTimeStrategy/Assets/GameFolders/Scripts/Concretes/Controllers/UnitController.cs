using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using RealTimeStrategy.Abstracts.Controllers;
using RealTimeStrategy.Abstracts.Movements;
using RealTimeStrategy.Actions;
using RealTimeStrategy.Movements;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace RealTimeStrategy.Controllers
{
    public class UnitController : NetworkBehaviour, IEntityController
    {
        [SerializeField] SpriteRenderer _spriteRenderer;
        
        IMover _mover;
        SelectionAction _selectionAction;
        Vector3 _position;
        bool _isMousePressed;

        public event System.Action<bool> OnSelected; 
        
        private void Awake()
        {
            _mover = new Mover(GetComponent<NavMeshAgent>(), Camera.main);
            _selectionAction = new SelectionAction(_spriteRenderer);
        }

        private void Start()
        {
            _selectionAction.EnableDisableSprite(false);
        }

        private void OnEnable()
        {
            OnSelected += _selectionAction.EnableDisableSprite;
        }

        private void OnDisable()
        {
            OnSelected -= _selectionAction.EnableDisableSprite;
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

        public void Selection(bool isEnable)
        {
            OnSelected?.Invoke(isEnable);
        }
    }
}