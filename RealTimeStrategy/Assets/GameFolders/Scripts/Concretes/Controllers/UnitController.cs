using System.Collections;
using System.Collections.Generic;
using Mirror;
using RealTimeStrategy.Abstracts.Controllers;
using RealTimeStrategy.Abstracts.Movements;
using RealTimeStrategy.Actions;
using RealTimeStrategy.Combats;
using RealTimeStrategy.Movements;
using UnityEngine;

namespace RealTimeStrategy.Controllers
{
    public class UnitController : NetworkBehaviour, IEntityController
    {
        [SerializeField] SpriteRenderer _spriteRenderer;
        
        IMover _mover;
        SelectionAction _selectionAction;
        Vector3 _position;
        Targeter _targeter;
        bool _isMousePressed;

        public Targeter Targeter => _targeter;

        public event System.Action<bool> OnSelected;
        public static event System.Action<UnitController> OnServerUnitSpawned;
        public static event System.Action<UnitController> OnServerUnitDespawned;
        public static event System.Action<UnitController> OnAuthorityUnitSpawned;
        public static event System.Action<UnitController> OnAuthorityUnitDespawned;
        
        private void Awake()
        {
            _mover = new Mover(this);
            _selectionAction = new SelectionAction(_spriteRenderer);
            _targeter = GetComponent<Targeter>();
        }

        private void Start()
        {
            _selectionAction.EnableDisableSprite(false);
        }

        private void OnEnable()
        {
            OnSelected += _selectionAction.EnableDisableSprite;
        }

        private void OnDisable()
        {
            OnSelected -= _selectionAction.EnableDisableSprite;
        }

        void Update()
        {
            _mover.Tick();
        }

        public override void OnStartServer()
        {
            OnServerUnitSpawned?.Invoke(this);
        }

        public override void OnStopServer()
        {
            OnServerUnitDespawned?.Invoke(this);
        }

        public override void OnStartClient()
        {
            if (!isClient || !hasAuthority) return;
            
            OnAuthorityUnitSpawned?.Invoke(this);
        }

        public override void OnStopClient()
        {
            if (!isClient || !hasAuthority) return;
            
            OnAuthorityUnitDespawned?.Invoke(this);
        }

        public void Selection(bool isEnable)
        {
            OnSelected?.Invoke(isEnable);
        }

        public void MoveAction(Vector3 position)
        {
            _mover.Move(position);
        }
    }
}