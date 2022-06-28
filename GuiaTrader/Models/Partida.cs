using System;
namespace GuiaTrader.Models
{
	public class Partida
	{

		public Int64 id { get; set; }
		public Equipe casa { get; set; }
		public Equipe visitante { get; set; }
		public String campeonato { get; set; }
		public DateTime horarioPartida { get; set; }
		public Double oddValor { get; set; }
		public Double previsaoGols { get; set; }
		public Double probabilidadeOver25 { get; set; }
		public Boolean entradaFeita { get; set; }


		public Partida()
        {
			this.casa = new Equipe();
			this.visitante = new Equipe();
			this.campeonato = "";
			this.horarioPartida = new DateTime();
        }
	}
}


