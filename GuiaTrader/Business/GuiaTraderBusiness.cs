using System;
using GuiaTrader.DAO;
using GuiaTrader.Models;
using System.Security.Cryptography;
using System.Text;

namespace GuiaTrader.Business
{
    public class GuiaTraderBusiness
    {
        private IGuiaTraderDAO guiaTraderDAO;
        public GuiaTraderBusiness()
        {
            this.guiaTraderDAO = new GuiaTraderDAOPgSql();
        }

        public List<ResultadoPartida> GetResumoMes(DateTime dataReferencia)
        {
            return this.guiaTraderDAO.GetResumoMes(dataReferencia);
        }

            public List<Partida> GetPartidasBTTS(DateTime dataReferencia)
        {
            return this.FilterPartidasBTTS(dataReferencia);
        }

        private List<Partida> FilterPartidasBTTS(DateTime dataReferencia)
        {
            List<Partida> toRtn = new List<Partida>();
            List<Partida> model = this.guiaTraderDAO.GetPartidasBTTS(dataReferencia);

            foreach (Partida partida in model)
            {
                if (partida.casa.mediaGolsMarcado < 1 || partida.visitante.mediaGolsMarcado < 1)
                    continue;

                Boolean percentOver25 = (partida.casa.percentOver25 >= 60 && partida.visitante.percentOver25 >= 60);
                Boolean mediaGolsTotal = (partida.casa.mediaGolsMarcadoSofrido >= 3) && (partida.visitante.mediaGolsMarcadoSofrido >= 3);
                Boolean golsCasa = ((partida.casa.mediaGolsMarcado + partida.visitante.mediaGolsSofrido) / 2) >= 1.5;
                Boolean golsVisitante = ((partida.visitante.mediaGolsMarcado + partida.casa.mediaGolsSofrido) / 2) >= 1.5;

                if (percentOver25 && mediaGolsTotal && golsCasa && golsVisitante)
                {
                    partida.oddValor = Math.Round((((partida.casa.mediaGolsMarcado + partida.visitante.mediaGolsSofrido) / 2) +
                        ((partida.visitante.mediaGolsMarcado + partida.casa.mediaGolsSofrido) / 2)) / 2, 2);

                    partida.previsaoGols = Math.Round((((partida.casa.mediaGolsMarcadoSofrido + partida.casa.mediaGolsMarcadoSofridoGeral)) +
                        ((partida.visitante.mediaGolsMarcadoSofrido + partida.visitante.mediaGolsMarcadoSofridoGeral))) / 4, 2);

                    partida.probabilidadeOver25 = (partida.casa.percentOver25 + partida.visitante.percentOver25) / 2;

                    toRtn.Add(partida);
                }
            }

            return toRtn;
        }

        public Boolean salvarResultado(Int64 idPartida, Boolean green, Double valor)
        {
            return this.guiaTraderDAO.salvarResultado(idPartida, green, valor);
        }

        public Boolean salvarEntrada(Int64 idPartida)
        {
            return this.guiaTraderDAO.salvarEntrada(idPartida);
        }

        public Usuario verifyUser(String usuario, String senha)
        {
            StringBuilder builder = new StringBuilder();
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(senha));

                // Convert byte array to a string   
                
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
            }

            return this.guiaTraderDAO.verifyUser(usuario, builder.ToString());
        }
    }
}

