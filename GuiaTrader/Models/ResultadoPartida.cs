using System;
namespace GuiaTrader.Models
{
	public class ResultadoPartida
	{
		public Partida partida { get; set; }
		public Boolean resultado { get; set; }
		public Double valor { get; set; }

		public ResultadoPartida()
		{
			this.partida = new Partida();
		}
	}
}

