using System;
namespace GuiaTrader.Models
{
	public class Equipe
	{
		public String nome { get; set; }
		public Double mediaGolsMarcado { get; set; }
		public Double mediaGolsSofrido { get; set; }
		public Double mediaGolsMarcadoSofrido { get; set; }
		public Double mediaGolsMarcadoSofridoGeral { get; set; }
		public Double percentOver25 { get; set; }

		public Equipe()
        {
			this.nome = "";
        }
	}
}

