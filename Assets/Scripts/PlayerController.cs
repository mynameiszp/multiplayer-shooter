using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    //[SerializeField] private SpriteRenderer renderer;
    [SerializeField] private NetworkMecanimAnimator _animator;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private NetworkPrefabRef _objectPool;
    private WeaponManager _weaponManager;
    private NetworkObject _weapon;

    public override void Spawned()
    {
        ConfigureWeapon();
    }
    private void ConfigureWeapon()
    {
        //if (!HasStateAuthority) return;
        _weaponManager = WeaponManager.Instance;
        WeaponData weaponData = _weaponManager.GetWeapon();
        _weapon = Runner.Spawn(weaponData.prefab);
        _weapon.transform.parent = gameObject.transform;
        _weaponManager.AddWeaponToList(_weapon);
        _weapon.GetComponent<Weapon>().Initialize(_weaponManager.Bullet, weaponData.attackDistance, weaponData.harm, weaponData.attackingEnemyNumber);
    }
    public override void FixedUpdateNetwork()
    {
        var input = GetInput(out NetworkInputData data);
        if (Runner.IsForward)
        {
            if (data.direction.magnitude > 0)
            {
                data.direction.Normalize();
                gameObject.transform.position += Runner.DeltaTime * _speed * (Vector3)data.direction;
                if (data.direction.x > 0)
                {
                    //flip
                }
                else if (data.direction.x < 0)
                {
                    //flip
                }
                Run();
            }
            if (data.aim.magnitude > 0) _weaponManager.StartPlayersAttack(_weapon, data.aim); //not now
            if (data.direction.magnitude == 0) Idle();
        }
        
    }
    public void Run()
    {
        _animator.Animator.SetBool("isRunning", true);
    }
    public void Idle()
    {
        _animator.Animator.SetBool("isRunning", false);
    }
    public void Die()
    {
        _animator.Animator.SetTrigger("Dead");
    }
}
