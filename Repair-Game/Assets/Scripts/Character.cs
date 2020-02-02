using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected Animator animator;
    protected CharacterController controller;

    // Enemy List
    private GameObject[] enemyList;
    private GameObject target;

    [SerializeField] [Range(0, 90)] private float shotAngleTolerance;

    // Magic Attack
    [Header("Magic Attack")]
    public KeyCode magicAttackKey;
    public GameObject firePoint;
    public GameObject vfxMagic;
    [SerializeField] private float fireDelayTime;
    [SerializeField] private float fireRate;
    public bool canFire;

    // Magic Area Attack
    [Header("Magic Area Attack")]
    public KeyCode magicAreaAttackKey;
    public GameObject orbFirePoint;
    public GameObject vfxOrb;
    [SerializeField] private float orbFireDelayTime;
    [SerializeField] private float orbFireRate;

    // Singleton
    public static Character character;
    public static Character GetCharacter() { return character; }

    // Start is called before the first frame update
    void Start()
    {
        if(character == null)
        {
            character = this;
        }
        canFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Initialize null vars
        if (animator == null) animator = GetComponent<CharacterMovement>().animator;
        if (controller == null) controller = GetComponent<CharacterMovement>().controller;
        if (enemyList == null) enemyList = GameObject.FindGameObjectsWithTag("Enemy");

        if (Input.GetKey(magicAttackKey) && canFire)
        {
            //timeToFire = Time.time + 1 / fireRate;
            StartCoroutine(MagicAttack());
        }
        else if (Input.GetKey(magicAreaAttackKey) && canFire)
        {
            StartCoroutine(MagicAreaAttack());
        }
    }

    private IEnumerator MagicAttack()
    {
        if (firePoint != null)
        {
            canFire = false;

            animator.SetTrigger("magicAttack");
            yield return new WaitForSeconds(fireDelayTime);
            GameObject vfx = Instantiate(vfxMagic, firePoint.transform.position, GetCharacter().transform.rotation);

            // Set the target enemy for the spawned projectile
            vfx.GetComponent<ProjectileMove>().SetTargetEnemy = FindTargetEnemy();

            yield return new WaitForSeconds(fireRate);
            canFire = true;
        }
        else
        {
            Debug.Log("No Fire Point!");
        }
    }

    private IEnumerator MagicAreaAttack()
    {
        if (orbFirePoint != null)
        {
            canFire = false;

            // Stop character movement
            GetComponent<CharacterMovement>().ToggleInput(false);

            animator.SetTrigger("magicAreaAttack");
            GameObject orb = Instantiate(vfxOrb, orbFirePoint.transform.position, GetCharacter().transform.rotation);

            yield return 0;

            orb.GetComponent<ProjectileMove>().SetMovable(false);
            yield return new WaitForSeconds(orbFireDelayTime);
            orb.GetComponent<ProjectileMove>().SetMovable(true);
            // Set the target enemy for the spawned projectile
            orb.GetComponent<ProjectileMove>().SetTargetEnemy = FindTargetEnemy();
            // Enable character movement
            GetComponent<CharacterMovement>().ToggleInput(true);

            yield return new WaitForSeconds(orbFireRate);
            canFire = true;
        }
        else
        {
            Debug.Log("No Orb Fire Point!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyBullet")
        {
            animator.SetTrigger("damaged");
        }
    }

    private GameObject FindTargetEnemy()
    {
        target = null;
        for (int i = 0; i < enemyList.Length; ++i)
        {
            GameObject currEnemy = enemyList[i];
            if (currEnemy != null && currEnemy.GetComponent<Enemy>().state != Enemy.State.Dead)
            {
                float angleDiff = AngleDiff(currEnemy);
                if (angleDiff < shotAngleTolerance / 2f)
                {
                    if (target == null || DistDiff(currEnemy) < DistDiff(target))
                        target = currEnemy;
                }
            }
        }
        return target;
    }

    // Helper method
    private float AngleDiff(GameObject enemy)
    {
        return Vector3.Angle(enemy.transform.position - transform.position, transform.forward);
    }

    private float DistDiff(GameObject enemy)
    {
        return Vector3.Distance(enemy.transform.position, transform.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (target != null)
            Gizmos.DrawLine(transform.position, target.transform.position);

        // Draw tolerance angle
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -shotAngleTolerance / 2f, 0) * transform.forward * 50);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, shotAngleTolerance / 2f, 0) * transform.forward * 50);
    }
}
