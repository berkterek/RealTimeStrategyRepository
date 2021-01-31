using System.Collections;
using System.Collections.Generic;
using Mirror;
using RealTimeStrategy.Abstracts.Movements;
using UnityEngine;
using UnityEngine.AI;

namespace RealTimeStrategy.Movements
{
    public class Mover : IMover
    {
        NavMeshAgent _navMeshAgent;
        Camera _camera;

        public Mover(NavMeshAgent navMeshAgent, Camera camera)
        {
            _navMeshAgent = navMeshAgent;
            _camera = camera;
        }

        [Command]
        public void Move(Vector3 position)
        {
            Ray ray = _camera.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (!NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 1f, NavMesh.AllAreas)) return;

                _navMeshAgent.SetDestination(navHit.position);
            }
        }
    }
}