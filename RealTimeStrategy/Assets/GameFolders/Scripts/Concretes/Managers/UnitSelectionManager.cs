using System.Collections;
using System.Collections.Generic;
using RealTimeStrategy.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RealTimeStrategy.Managers
{
    public class UnitSelectionManager : MonoBehaviour
    {
        [SerializeField] LayerMask _layerMask;
        
        private Camera _camera;

        private List<UnitController> _selectionUnit = new List<UnitController>();

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                foreach (UnitController unit in _selectionUnit)
                {
                    unit.Selection(false);
                }
            
                _selectionUnit.Clear();
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                ClearSelectionArea();
            }
        }

        private void ClearSelectionArea()
        {
            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _layerMask)) return;

            if (!hit.collider.TryGetComponent<UnitController>(out UnitController unitController)) return;

            if (!unitController.hasAuthority) return;
            
            _selectionUnit.Add(unitController);

            foreach (UnitController unit in _selectionUnit)
            {
                unit.Selection(true);
            }
        }
    }    
}

