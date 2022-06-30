using System;
using GuiaTrader.Models;

namespace GuiaTrader.DAO
{
	public interface IGuiaTraderDAO
	{
		public List<ResultadoPartida> GetResumoMes(DateTime dataReferencia);
		public List<Partida> GetPartidasBTTS(DateTime dataReferencia);
		public Boolean salvarResultado(Int64 idPartida, Boolean green, Double valor);
		public Boolean salvarEntrada(Int64 idPartida);
		public Usuario verifyUser(String user, String pwd);
	}
}

