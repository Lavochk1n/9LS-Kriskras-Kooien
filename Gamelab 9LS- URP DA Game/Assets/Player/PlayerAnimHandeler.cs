using Quarantine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimHandeler : MonoBehaviour
{
    private PlayerBehaviour PB;
    private CharacterController CC;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        PB = transform.parent.GetComponent<PlayerBehaviour>();
        CC = transform.parent.GetComponent<CharacterController>();

    }

    void Update()
    {
        if (PB.heldAnimal.type != AnimalTypes.Empty)
        {
            anim.SetBool("isHolding", true);
        }
        else
        {
            anim.SetBool("isHolding", false);
        }

        if (PB.IsMoving())
        {
            anim.SetBool("isMoving", true);

        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        anim.speed = PB.SprintSpeed();

    }
}
