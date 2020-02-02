using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected Animator animator;
    protected CharacterController controller;

    // Enemy List
    private GameObject[] enemyList;

    // Magic Attack
    public GameObject firePoint;
    public GameObject vfxMagic;
    [SerializeField] private float fireDelayTime;
    [SerializeField] private float fireRate;
    [SerializeField] private float timeToFire = 0;
    [SerializeField] [Range(0, 90)] private float shotAngleTolerance;

    // Singleton
    public static Character character;
    public static Character GetCharacter() { return character; }

    // Start is called before the first frame update
    void Start()
    {
        character = this;
    }

    // Update is called once per frame
    void Update()
    {
        // Initialize null vars
        if (animator == null) animator = GetComponent<CharacterMovement>().animator;
        if (controller == null) controller = GetComponent<CharacterMovement>().controller;
        if (enemyList == null) enemyList = GameObject.FindGameObjectsWithTag("Enemy");
        

        if (Input.GetKey(KeyCode.Space) && Time.time >= timeToFire)
        {
            timeToFire = Time.time + 1 / fireRate;
            StartCoroutine(MagicAttack());
        }
    }


    private IEnumerator MagicAttack()
    {
        if (firePoint != null)
        {
            Debug.Log("Magic!!!");
            animator.SetTrigger("magicAttack");
            yield return new WaitForSeconds(fireDelayTime);
            GameObject vfx = Instantiate(vfxMagic, firePoint.transform.position, GetCharacter().transform.rotation);
            
            // Set the target enemy for the spawned projectile
            vfx.GetComponent<ProjectileMove>().SetTargetEnemy = FindTargetEnemy();
        }
        else
        {
            Debug.Log("No Fire Point!");
        }
    }

    private GameObject FindTargetEnemy()
    {
        GameObject target = null;
        for (int i = 0; i < enemyList.Length; ++i)
        {
            GameObject currEnemy = enemyList[i];
            float angleDiff = AngleDiff(currEnemy);
            if (angleDiff < shotAngleTolerance / 2f)
            {
                if (target == null || DistDiff(currEnemy) < DistDiff(target))
                    target = currEnemy;
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
}
