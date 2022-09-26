using Assets.scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Assets.scripts.Utils.Constantes;

public class PlayerController : MonoBehaviour
{   
    // Start is called before the first frame update
    public CharacterController controller;
    public Transform cam;
    private Animator animator;

    public ESTADO_JOGADOR EstadoAtual;

    public float _velocidadeBase = 1f;
    private bool movimentoTravado;

    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;

    //O quanto a velocidade do personsagem deve aumentar se ele tiver correndo
    public float runningModifier = 1.0f;


    
    public bool isExecutingAction = false;

    private bool EstahOcioso { get { return this.EstadoAtual == ESTADO_JOGADOR.OCIOSO; } }

    private bool EstahPreparandoAcao { get { return this.EstadoAtual == ESTADO_JOGADOR.PREPARANDO; } }

    private bool EstahExecutandoAcao { get { return this.EstadoAtual == ESTADO_JOGADOR.EXECUTANDO; } }

    public float _tempoDecorridoPreparandoAcao;
    public float _porcentagemAcao { get { return (this._tempoDecorridoPreparandoAcao / Constantes.SEGUNDOS_PREPARACAO__ATAQUE) * 100; }  }

    public float _tempoDecorridoExecutandoAcao;



    void Start()
    {
        this._tempoDecorridoPreparandoAcao = 0f;
        this.movimentoTravado = false;
        this.EstadoAtual = ESTADO_JOGADOR.OCIOSO;
        GameObject model = GameObject.Find("HeroModel");
        animator = model.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
        UpdateAction();
    }

    private void UpdateAction()
    {

        if (EstahExecutandoAcao)
        {
            float tempoDecorridoAtualizado = this._tempoDecorridoExecutandoAcao + Time.deltaTime;
            this._tempoDecorridoExecutandoAcao = Mathf.Clamp(tempoDecorridoAtualizado, 0, Constantes.SEGUNDOS_EXECUTANDO_ACAO);

            if (this._tempoDecorridoExecutandoAcao >= Constantes.SEGUNDOS_EXECUTANDO_ACAO)
            {
                Debug.Log("AÇÃO EXECUTADA!");
                this.EstadoAtual = ESTADO_JOGADOR.OCIOSO;
                this._tempoDecorridoExecutandoAcao = 0f;
                this.movimentoTravado = false;
            }
            //bloquear movimento (provavelmente diminuir a velocidade pra zero)
            //começar a executar a animação de ataque
        }

        if (EstahPreparandoAcao)
        {
            float tempoDecorridoAtualizado = this._tempoDecorridoPreparandoAcao + Time.deltaTime;
            this._tempoDecorridoPreparandoAcao = Mathf.Clamp(tempoDecorridoAtualizado, 0, Constantes.SEGUNDOS_PREPARACAO__ATAQUE);

            if(this._tempoDecorridoPreparandoAcao >= Constantes.SEGUNDOS_PREPARACAO__ATAQUE)
            {
                Debug.Log("AÇÃO CARREGADA!");
                this._tempoDecorridoPreparandoAcao = 0f;
                this.EstadoAtual = ESTADO_JOGADOR.EXECUTANDO;
                this.movimentoTravado = true;
            }
        }

        if (EstahOcioso)
        {
            bool pressedSpace = Input.GetKeyDown(KeyCode.Space);
            bool iniciarPreparacao = (this.EstahOcioso && pressedSpace);

            if (iniciarPreparacao)
            {
                Debug.Log("PREPARAÇÃO INICIADA!");
                this.EstadoAtual = ESTADO_JOGADOR.PREPARANDO;
                this._tempoDecorridoPreparandoAcao = 0f;
            }
        }
    }

    void UpdatePosition()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(x, 0, y).normalized;

        bool isWalking = animator.GetBool("isWalking");
        bool isRunning = animator.GetBool("isRunning");
        bool isPressingForward = direction.magnitude >= .1f;
        bool isPressingRunning = Input.GetKey(KeyCode.LeftShift);

        //Debug.Log($"direction x: {direction.x} | y: {direction.y} | z: {direction.z}");
        //Debug.Log($"isWalking: {isWalking} | isPressingForward: {isPressingForward}");

        float runningFactor = isPressingRunning ? this.runningModifier : 1f;

        if (isPressingForward && !movimentoTravado)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle , 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection * _velocidadeBase * runningFactor * Time.deltaTime);
        }

        //Começar andar p/ frente quando apertar pra frente
        if(!isWalking && isPressingForward)
        {
            animator.SetBool("isWalking", true);
        }

        //Se tiver andando e pressionar shift trocar pra correndo
        if(isWalking && isPressingForward && isPressingRunning)
        {
            animator.SetBool("isRunning", true);
        }

        //Parar de correr e passar a andar se parar de apertar shift
        if(isRunning && isPressingForward && !isPressingRunning)
        {
            animator.SetBool("isRunning", false);
        }

        //Trocar animação p/ idle quanto parar de apertar W
        if(isWalking && !isPressingForward)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }
}
