using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts.Utils
{
    public static class Constantes
    {
        public const string FORMATO_EXIBICAO_VIDA = "000";
        public const string FORMATO_EXIBICAO_PORCENTAGEM = "0.0#";

        public const float SEGUNDOS_PREPARACAO__ATAQUE  = 5f;         //tempo necessário para carregar ataque
        public const float SEGUNDOS_EXECUCAO__ATAQUE    = 3f;         //tempo de execução do ataque (animação)

        public const float SEGUNDOS_EXECUTANDO_ACAO     = 3F;

        public enum ESTADO_JOGADOR
        {
            OCIOSO = 1,
            PREPARANDO = 2,
            EXECUTANDO = 3
        }

        public enum ACAO
        {
            ATAQUE = 1,
            MAGIA = 2,
            ITEM = 3
        }


    }
}
