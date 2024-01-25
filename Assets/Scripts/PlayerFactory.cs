using Fusion;
using UnityEngine;
using Zenject;

namespace Assets.Scripts
{
    public class PlayerFactory: NetworkBehaviour
    {
        [Inject] private WeaponManager _weaponManager;


        //public void Create(NetworkRunner runner, PlayerRef playerRef)
        //{
        //    if (runner.IsServer)
        //    {
        //        // Create a unique position for the player
        //        Vector2 spawnPosition = new Vector2(0, 0);
        //        NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
        //        // Keep track of the player avatars for easy access
        //        _spawnedCharacters.Add(playerRef, networkPlayerObject);
        //    }
        //}

    }
}
