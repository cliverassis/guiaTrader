﻿using System;
using GuiaTrader.Business;
using GuiaTrader.Models;

namespace GuiaTrader.Facade
{
	public class Fachada
	{
		private static Fachada? _instance;
		private GuiaTraderBusiness guiaTraderBus;
		private Usuario usuario;

		public static Fachada getInstance()
		{
			if (_instance == null)
                _instance = new Fachada();

			return _instance;
		}

		private Fachada()
		{
			this.guiaTraderBus = new GuiaTraderBusiness();
		}

		public List<ResultadoPartida> GetResumoMes(DateTime dataReferencia)
        {
			return this.guiaTraderBus.GetResumoMes(dataReferencia);
		}

		public List<Partida> GetPartidasBTTS(DateTime dataReferencia)
		{
			return this.guiaTraderBus.GetPartidasBTTS(dataReferencia);
		}

        public Boolean verifyUser(String user, String pwd)
        {
			usuario = this.guiaTraderBus.verifyUser(user, pwd);

			return usuario != null;
		}

		public Usuario getUsuarioAtual()
        {
			return this.usuario;
        }

		public void Logout()
        {
			usuario = null;
        }

        public Boolean salvarResultado(Int64 idPartida, Boolean green, Double valor)
        {
            return this.guiaTraderBus.salvarResultado(idPartida, green, valor);
		}

		public Boolean salvarEntrada(Int64 idPartida)
		{
			return this.guiaTraderBus.salvarEntrada(idPartida);
		}
	}
}

