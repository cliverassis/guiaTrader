using System;
namespace GuiaTrader.Models
{
    public class PartidaModelView
    {
        public List<Partida> listaPartidas { get; set; }
        public DateTime dataReferencia { get; set; }
        public Usuario usuarioAtual { get; set; }

        public PartidaModelView()
        {
            this.listaPartidas = new List<Partida>();
        }
    }
}

