using Entities;
using Exceptions;
using UnityEngine;

namespace Managers
{
    public sealed class PlayerManager : BaseManager<PlayerManager>
    {
        [field: SerializeField] 
        public Player Player { get; private set; }


        protected override void Awake()
        {
            base.Awake();
            
            Player ??= FindObjectOfType<Player>() ?? throw new ObjectNotFoundOnSceneException(Player.name);
        }
    }
}