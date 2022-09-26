using static Assets.scripts.Utils.Constantes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{

    public GameObject Player;

    private PlayerController playerController;
    private PlayerStatus playerStatus;
    private Text HealthBarText;
    private Text PlayerName;
    private Text BarraDeAcao;
    // Start is called before the first frame update
    void Start()
    {
        this.playerStatus = Player.GetComponent<PlayerStatus>();
        this.playerController = Player.GetComponent<PlayerController>();
        this.HealthBarText = GameObject.Find("HealthDisplay").GetComponent<Text>();
        this.PlayerName = GameObject.Find("NameDisplay").GetComponent<Text>();
        this.BarraDeAcao = GameObject.Find("ActionDisplay").GetComponent<Text>();
        this.HealthBarText.text = $"HP {playerStatus.CurrentHealth.ToString(FORMATO_EXIBICAO_VIDA)}/{playerStatus.MaxHealth.ToString(FORMATO_EXIBICAO_VIDA)}";
        this.PlayerName.text = playerStatus.name.ToUpper();
    }

    // Update is called once per frame
    void Update()
    {
        this.HealthBarText.text = $"HP {playerStatus.CurrentHealth.ToString(FORMATO_EXIBICAO_VIDA)}/{playerStatus.MaxHealth.ToString(FORMATO_EXIBICAO_VIDA)}";
        switch (playerController.EstadoAtual)
        {
            case ESTADO_JOGADOR.OCIOSO:
            case ESTADO_JOGADOR.PREPARANDO:
                this.BarraDeAcao.text = $"{playerController._porcentagemAcao.ToString(FORMATO_EXIBICAO_PORCENTAGEM)}%";
                break;
            case ESTADO_JOGADOR.EXECUTANDO:
                this.BarraDeAcao.text = $"ATACANDO";
                break;
            default:
                break;
        }
        
    }
}
