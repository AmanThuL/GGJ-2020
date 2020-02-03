using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    protected Animator animator;
    protected CharacterController controller;

    public GameObject UIManager;
    public GameObject sceneFader;

    private GameObject sceneFaderInScene;

    public Animator GetAnimator { get => animator; }

    // Enemy List
    private GameObject[] enemyList;
    private GameObject target;

    [SerializeField] [Range(0, 90)] private float shotAngleTolerance;

    // UI
    private Image healthUI;

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
        sceneFaderInScene = Instantiate(sceneFader, Vector3.zero, Quaternion.identity);
        Instantiate(UIManager, Vector3.zero, Quaternion.identity);

        if (character == null)
        {
            character = this;
        }
        canFire = true;

        healthUI = GameObject.FindGameObjectWithTag("HealthUI").GetComponent<Image>();
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

        if (GameStats.Health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        animator.SetTrigger("dead");
        yield return new WaitForSeconds(0.5f);
        sceneFaderInScene.SetActive(true);
        sceneFaderInScene.GetComponent<SceneFader>().FadeTo("Stage");    
    }

    private IEnumerator MagicAttack()
    {
        if (firePoint != null)
        {

            refillEnemyList();

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
            if (orb != null)
            {
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
                // Get damamged by the enemy's bullet when charging
                // Enable character movement
                yield return new WaitForSeconds(1.5f);
                GetComponent<CharacterMovement>().ToggleInput(true);
                yield return new WaitForSeconds(orbFireRate);
                canFire = true;
            }
        }
        else
        {
            Debug.Log("No Orb Fire Point!");
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

    public void TakeDamage(int damage)
    {
        GameStats.Health -= damage;

        Debug.Log("I take a damage of " + damage);

        // Update UI
        healthUI.fillAmount = (float)GameStats.Health / (float)GameStats.MaxHealth;
    }

    public void refillEnemyList()
    {
        enemyList = GameObject.FindGameObjectsWithTag("Enemy");
    }
}
