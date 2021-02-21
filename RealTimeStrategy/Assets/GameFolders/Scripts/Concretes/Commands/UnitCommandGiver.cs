using System.Collections;
using System.Collections.Generic;
using RealTimeStrategy.Combats;
using RealTimeStrategy.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RealTimeStrategy.Commands
{
    public class UnitCommandGiver : MonoBehaviour
    {
        [SerializeField] LayerMask _layerMask;
        
        UnitSelectionHandler _unitSelectionHandler;
        Camera _camera;
        
        private void Awake()
        {
            _unitSelectionHandler = GetComponent<UnitSelectionHandler>();
            _camera = Camera.main;
        }

        private void Update()
        {
            if (!Mouse.current.rightButton.wasPressedThisFrame) return;

            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask)) return;

            if (hit.collider.TryGetComponent<Targetable>(out Targetable targetable))
            {
                if (targetable.hasAuthority)
                {
                    TryMove(hit.point);
                    return;
                }
                else
                {
                    TryTarget(targetable);
                    return;
                }
            }

            TryMove(hit.point);
        }

        private void TryTarget(Targetable targetable)
        {
            foreach (UnitController unitController in _unitSelectionHandler.SelectionUnits)
            {
                unitController.Targeter.SetTarget(targetable.gameObject);
            }
        }

        private void TryMove(Vector3 hitPoint)
        {
            foreach (UnitController unitController in _unitSelectionHandler.SelectionUnits)
            {
                unitController.MoveAction(hitPoint);
            }
        }
    }    
}

