using Mirror;
using RealTimeStrategy.Abstracts.Controllers;
using RealTimeStrategy.Abstracts.Movements;
using RealTimeStrategy.Combats;
using UnityEngine;
using UnityEngine.AI;

namespace RealTimeStrategy.Movements
{
    public class Mover : IMover
    {
        NavMeshAgent _navMeshAgent;
        Targeter _targeter;
        
        public Mover(IEntityController entityController)
        {
            _navMeshAgent = entityController.transform.GetComponent<NavMeshAgent>();
            _targeter = entityController.transform.GetComponent<Targeter>();
        }

        [Command]
        public void Move(Vector3 position)
        {
            if (!NavMesh.SamplePosition(position, out NavMeshHit navHit, 1f, NavMesh.AllAreas)) return;

            _targeter.ClearTarget();
            _navMeshAgent.SetDestination(navHit.position);
        }

        [ServerCallback]
        public void Tick()
        {
            if (!_navMeshAgent.hasPath) return;
            
            if (_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance) return;
            
            _navMeshAgent.ResetPath();
        }
    }
}