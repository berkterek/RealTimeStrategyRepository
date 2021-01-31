using System.Collections;
using System.Collections.Generic;
using Mirror;
using RealTimeStrategy.Controllers;
using UnityEngine;

namespace RealTimeStrategy.Managers
{
    public class RealTimeStrategyNetworkManager : NetworkManager
    {
        [SerializeField] UnitSpawnerController _unitSpawnerController;
        
        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            //burda UnitSpanwerController'i server'a oyuncuyu eklerken spanwer'ida ekleriz
            base.OnServerAddPlayer(conn);

            UnitSpawnerController unitSpawnerInstance = Instantiate(_unitSpawnerController, conn.identity.transform.position, conn.identity.transform.rotation);
            
            NetworkServer.Spawn(unitSpawnerInstance.gameObject,conn);
        }
    }    
}

