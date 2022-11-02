using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public ControlType controlType;
    public Joystick joystick;
    public float speed;
    public float speedRoll = 2;
    public Animator animator;

    public enum ControlType { PC, Android }

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 moveVelocity;
    private Vector2 movePerk;
    private bool isPerk = false;
    private bool isClickDown = false;
    private GameObject Weapons;

    public float delay = 0.5f; //��� ��� ���� ���� int,double ��� float
    private bool delayState = true;

    IEnumerator Delay()
    {
        delayState = false;

        yield return new WaitForSeconds(delay); //�� ����� ��������� ��������.

        delayState = true;
    }


    void Start()

    {
        rb = GetComponent<Rigidbody2D>();
        Weapons = GameObject.Find("Weapons");
    }
    void idle()
    {

        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("DodgeRoll")) // �������� �� ���������� �� �������
        {
            if (moveInput.x != 0 || moveInput.y != 0) // �������� ������� �� ��������� ��������
            {


                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("PlayerRun")) //  ���� �� ������������� �������� ������ 
                    animator.Play("PlayerRun"); // ��������� �������� ������ 
            }
            else // ���� ����� �� ����� 
            {
                animator.Play("Idol"); // ��������� �������� ������� 
            }
        }
        if (moveInput.x != 0)
        {
              if (moveInput.x > 0) // ���� ��������� ���� -->
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false; // �������� �� �
                Weapons.transform.localScale = new Vector2(Math.Abs(Weapons.transform.localScale.x), Weapons.transform.localScale.y);


            }
            else // ���� ��������� ���� <--
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true; // �������� �� �
                Weapons.transform.localScale = new Vector2(-Math.Abs(Weapons.transform.localScale.x), Weapons.transform.localScale.y);
            }
        }
        

        if ((Input.GetKeyDown(KeyCode.LeftShift) || isClickDown ) && delayState) // ���� ������ ������ �������� 
        {
            isClickDown = false; // ������ ������ ��������
            if (moveInput.x != 0 || moveInput.y != 0) // ���� ���� ������������
            {
                StartCoroutine(Delay());
                DodgeRoll(); // ������� �������
            }

        }
        else // ���� �� ������ ������ �������� 
        {
            if (controlType == ControlType.PC)
            {
                moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            }
            else if (controlType == ControlType.Android)
            {
                moveInput = new Vector2(joystick.Horizontal, joystick.Vertical); // ������� ����� � �����������
            }
            moveVelocity = moveInput.normalized * speed; // ��������� ������������
        }

    }
    void Update()
    {
        idle();

        
    }


    private void DodgeRoll() // �������
    {
        if (controlType == ControlType.PC)
        {
            moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else if (controlType == ControlType.Android)
        {
            moveInput = new Vector2(joystick.Horizontal, joystick.Vertical); // ������� ������ � �����������
        }

        movePerk = moveInput.normalized * speedRoll; // ������ � ������������
        animator.Play("DodgeRoll"); // �������� ��������

        isPerk = true; // �������� �������
    }

    public void ClickDown ()
    {            
        isClickDown = true;
    }

    void FixedUpdate()
    {
        if (isPerk)
        {
            rb.MovePosition(rb.position + movePerk * Time.fixedDeltaTime);
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("DodgeRoll"))
            {
                isPerk = false;
            }
        }
        else
        {
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        }



    }
}