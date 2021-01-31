using System.Collections;
using System.Collections.Generic;
using RealTimeStrategy.Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RealTimeStrategy.Commands
{
    public class UnitSelection : MonoBehaviour
    {
        [SerializeField] LayerMask _layerMask;
        
        private Camera _camera;

        public List<UnitController> SelectionUnits { get; } = new List<UnitController>();

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                foreach (UnitController unit in SelectionUnits)
                {
                    unit.Selection(false);
                }
            
                SelectionUnits.Clear();
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
            
            SelectionUnits.Add(unitController);

            foreach (UnitController unit in SelectionUnits)
            {
                unit.Selection(true);
            }
        }
    }    
}