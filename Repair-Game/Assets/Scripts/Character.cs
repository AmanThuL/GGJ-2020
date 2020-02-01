﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected Animator animator;
    protected CharacterController controller;

    // Magic Attack
    public GameObject firePoint;
    public GameObject vfxMagic;
    [SerializeField] private float fireDelayTime;

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
        if (animator == null) animator = GetComponent<CharacterMovement>().animator;
        if (controller == null) controller = GetComponent<CharacterMovement>().controller;

        if (Input.GetKeyDown(KeyCode.Space))
        {
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
            GameObject vfx = Instantiate(vfxMagic, firePoint.transform.position, Quaternion.Euler(GetCharacter().transform.forward));
        }
        else
        {
            Debug.Log("No Fire Point!");
        }
    }

}
