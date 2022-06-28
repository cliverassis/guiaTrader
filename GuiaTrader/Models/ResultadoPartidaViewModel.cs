using System;
namespace GuiaTrader.Models
{
    public class ResultadoPartidaViewModel
    {
        public List<ResultadoPartida> listaResultadoPartida { get; set; }
        public DateTime dataReferencia { get; set; }

        public ResultadoPartidaViewModel()
        {
            this.listaResultadoPartida = new List<ResultadoPartida>();
        }
    }
}

