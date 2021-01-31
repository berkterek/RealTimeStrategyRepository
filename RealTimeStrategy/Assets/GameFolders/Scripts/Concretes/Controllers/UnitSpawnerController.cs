using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.EventSystems;

namespace RealTimeStrategy.Controllers
{
    public class UnitSpawnerController : NetworkBehaviour, IPointerClickHandler
    {
        [SerializeField] UnitController _unitPrefab;
        [SerializeField] Transform _spawnPoint;

        [Command]
        private void SpawnUnit()
        {
            UnitController unit = Instantiate(_unitPrefab, _spawnPoint.position, Quaternion.identity);
            
            //RealTimeStrategyNetworkManager uzerinden bu nesne olusurken oyuncuya atandi ve unit spawn'lanirken connectionToClient ile olusan oyuncuya atanir
            NetworkServer.Spawn(unit.gameObject, connectionToClient);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || !hasAuthority) return;
            
            SpawnUnit();
        }
    }    
}

