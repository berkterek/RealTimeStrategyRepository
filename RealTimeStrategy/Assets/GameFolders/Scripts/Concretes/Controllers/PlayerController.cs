using System.Collections;
using System.Collections.Generic;
using Mirror;
using RealTimeStrategy.Abstracts.Controllers;
using UnityEngine;

namespace RealTimeStrategy.Controllers
{
    public class PlayerController : NetworkBehaviour
    {
        [SerializeField] List<UnitController> _units = new List<UnitController>();

        public List<UnitController> Units => _units;

        public override void OnStartServer()
        {
            UnitController.OnServerUnitSpawned += OnServerUnitSpawned;
            UnitController.OnServerUnitDespawned += OnServerUnitDespawned;
        }

        public override void OnStopServer()
        {
            UnitController.OnServerUnitSpawned -= OnServerUnitSpawned;
            UnitController.OnServerUnitDespawned -= OnServerUnitDespawned;
        }

        public override void OnStartClient()
        {
            if (!isClientOnly) return;
            
            UnitController.OnAuthorityUnitSpawned += OnAuthorityUnitSpawned;
            UnitController.OnAuthorityUnitDespawned += OnAuthorityUnitDespawned;
        }

        public override void OnStopAuthority()
        {
            if (!isClientOnly) return;
            
            UnitController.OnAuthorityUnitSpawned -= OnAuthorityUnitSpawned;
            UnitController.OnAuthorityUnitDespawned -= OnAuthorityUnitDespawned;
        }
        
        private void OnAuthorityUnitSpawned(UnitController unitController)
        {
            if (!hasAuthority) return;
            
            _units.Add(unitController);
        }
        
        private void OnAuthorityUnitDespawned(UnitController unitController)
        {
            if (!hasAuthority) return;
            
            _units.Remove(unitController);
        }

        private void OnServerUnitSpawned(UnitController unitController)
        {
            if (unitController.connectionToClient.connectionId != this.connectionToClient.connectionId) return;
            
            _units.Add(unitController);
        }
        
        private void OnServerUnitDespawned(UnitController unitController)
        {
            if (unitController.connectionToClient.connectionId != this.connectionToClient.connectionId) return;
            
            _units.Remove(unitController);
        }
    }
}