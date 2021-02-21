using Mirror;
using RealTimeStrategy.Abstracts.Movements;
using UnityEngine;
using UnityEngine.AI;

namespace RealTimeStrategy.Movements
{
    public class Mover : IMover
    {
        NavMeshAgent _navMeshAgent;

        public Mover(NavMeshAgent navMeshAgent)
        {
            _navMeshAgent = navMeshAgent;
        }

        [Command]
        public void Move(Vector3 position)
        {
            if (!NavMesh.SamplePosition(position, out NavMeshHit navHit, 1f, NavMesh.AllAreas)) return;

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