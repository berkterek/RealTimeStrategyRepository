using System.Collections;
using System.Collections.Generic;
using RealTimeStrategy.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RealTimeStrategy.Commands
{
    public class UnitCommandGiver : MonoBehaviour
    {
        [SerializeField] LayerMask _layerMask;
        
        UnitSelection _unitSelection;
        Camera _camera;
        
        private void Awake()
        {
            _unitSelection = GetComponent<UnitSelection>();
            _camera = Camera.main;
        }

        private void Update()
        {
            if (!Mouse.current.rightButton.wasPressedThisFrame) return;

            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask)) return;

            TryMove(hit.point);
        }

        private void TryMove(Vector3 hitPoint)
        {
            foreach (UnitController unitController in _unitSelection.SelectionUnits)
            {
                unitController.MoveAction(hitPoint);
            }
        }
    }    
}

