using System;
using GuiaTrader.Models;
using Npgsql;

namespace GuiaTrader.DAO
{
	public class GuiaTraderDAOPgSql : IGuiaTraderDAO
	{
        private String strConnection = "Host=kesavan.db.elephantsql.com:5432;Username=ubmyzhkf;Password=07xX2s_Tt-J0gnCdJDbDMwLMpe72S6tO;Database=ubmyzhkf";

        public List<ResultadoPartida> GetResumoMes(DateTime dataReferencia)
        {
            NpgsqlConnection? conn = null;
            NpgsqlDataReader? reader = null;
            try
            {
                conn = new NpgsqlConnection(strConnection);
                conn.Open();
                String sql = "SELECT partida.campeonato, partida.casa, partida.fora, partida.id, partida.horario_partida, " +
                             "dados.\"mediaGolsMarcado_casa\", dados.\"mediaGolsMarcado_fora\", dados.\"mediaGolsSofrido_casa\", " +
                             "dados.\"mediaGolsSofrido_fora\", dados.\"mediaGolsMarcadosSofridos_casa\", dados.\"mediaGolsMarcadosSofridos_fora\", " +
                             "dados.\"mediaGolsMarcadosSofridosGeral_casa\", dados.\"mediaGolsMarcadosSofridosGeral_fora\",  " +
                             "dados.\"percentOver25_casa\", dados.\"percentOver25_fora\", resultado.resultado, resultado.valor " +
                             "FROM dados_partida as partida " +
                             "INNER JOIN estatistica_clubes as dados ON dados.fk_partida = partida.id " +
                             "INNER JOIN tb_resultado_entrada as resultado ON resultado.id_partida = partida.id " +
                             "WHERE horario_partida BETWEEN @dataInicio AND @dataFim ORDER BY partida.horario_partida;";
                NpgsqlCommand command = new NpgsqlCommand(sql, conn);
                command.Parameters.AddWithValue("dataInicio", new DateTime(2022, 06, 1));
                command.Parameters.AddWithValue("dataFim", dataReferencia.AddDays(1));
                reader = command.ExecuteReader();

                List<ResultadoPartida> toRtn = new List<ResultadoPartida>();
                while (reader.Read())
                {
                    Partida partida = new Partida();
                    if (reader["campeonato"] != DBNull.Value)
                        partida.campeonato = reader["campeonato"].ToString();

                    if (reader["horario_partida"] != DBNull.Value)
                        partida.horarioPartida = (DateTime)reader["horario_partida"];

                    if (reader["id"] != DBNull.Value)
                        partida.id = Int64.Parse(reader["id"].ToString());

                    Equipe casa = new Equipe();
                    if (reader["casa"] != DBNull.Value)
                        casa.nome = reader["casa"].ToString();
                    if (reader["mediaGolsMarcado_casa"] != DBNull.Value)
                        casa.mediaGolsMarcado = Double.Parse(reader["mediaGolsMarcado_casa"].ToString());
                    if (reader["mediaGolsSofrido_casa"] != DBNull.Value)
                        casa.mediaGolsSofrido = Double.Parse(reader["mediaGolsSofrido_casa"].ToString());
                    if (reader["mediaGolsMarcadosSofridos_casa"] != DBNull.Value)
                        casa.mediaGolsMarcadoSofrido = Double.Parse(reader["mediaGolsMarcadosSofridos_casa"].ToString());
                    if (reader["mediaGolsMarcadosSofridosGeral_casa"] != DBNull.Value)
                        casa.mediaGolsMarcadoSofridoGeral = Double.Parse(reader["mediaGolsMarcadosSofridosGeral_casa"].ToString());
                    if (reader["percentOver25_casa"] != DBNull.Value)
                        casa.percentOver25 = Double.Parse(reader["percentOver25_casa"].ToString());

                    Equipe visitante = new Equipe();
                    if (reader["fora"] != DBNull.Value)
                        visitante.nome = reader["fora"].ToString();
                    if (reader["mediaGolsMarcado_fora"] != DBNull.Value)
                        visitante.mediaGolsMarcado = Double.Parse(reader["mediaGolsMarcado_fora"].ToString());
                    if (reader["mediaGolsSofrido_fora"] != DBNull.Value)
                        visitante.mediaGolsSofrido = Double.Parse(reader["mediaGolsSofrido_fora"].ToString());
                    if (reader["mediaGolsMarcadosSofridos_fora"] != DBNull.Value)
                        visitante.mediaGolsMarcadoSofrido = Double.Parse(reader["mediaGolsMarcadosSofridos_fora"].ToString());
                    if (reader["mediaGolsMarcadosSofridosGeral_fora"] != DBNull.Value)
                        visitante.mediaGolsMarcadoSofridoGeral = Double.Parse(reader["mediaGolsMarcadosSofridosGeral_fora"].ToString());
                    if (reader["percentOver25_fora"] != DBNull.Value)
                        visitante.percentOver25 = Double.Parse(reader["percentOver25_fora"].ToString());

                    partida.casa = casa;
                    partida.visitante = visitante;

                    ResultadoPartida resultado = new ResultadoPartida();
                    resultado.partida = partida;

                    if (reader["resultado"] != DBNull.Value)
                        resultado.resultado = (Boolean)reader["resultado"];
                    if (reader["valor"] != DBNull.Value)
                        resultado.valor = Double.Parse(reader["valor"].ToString());

                    toRtn.Add(resultado);
                }

                return toRtn;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRO - " + e.Message);

                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();

                if (reader != null)
                    reader.Close();
            }
        }

        public List<Partida> GetPartidasBTTS(DateTime dataReferencia)
        {
            NpgsqlConnection? conn = null;
            NpgsqlDataReader? reader = null;
            try
            {
                conn = new NpgsqlConnection(strConnection);
                conn.Open();
                String sql = "SELECT partida.campeonato, partida.casa, partida.fora, partida.id, partida.horario_partida, " +
                             "dados.\"mediaGolsMarcado_casa\", dados.\"mediaGolsMarcado_fora\", dados.\"mediaGolsSofrido_casa\", " +
                             "dados.\"mediaGolsSofrido_fora\", dados.\"mediaGolsMarcadosSofridos_casa\", dados.\"mediaGolsMarcadosSofridos_fora\", " +
                             "dados.\"mediaGolsMarcadosSofridosGeral_casa\", dados.\"mediaGolsMarcadosSofridosGeral_fora\",  " +
                             "dados.\"percentOver25_casa\", dados.\"percentOver25_fora\", partida.entrada_feita " +
                             "FROM dados_partida as partida " +
                             "INNER JOIN estatistica_clubes as dados ON dados.fk_partida = partida.id " +
                             "WHERE horario_partida BETWEEN @dataInicio AND @dataFim ORDER BY partida.horario_partida;";
                NpgsqlCommand command = new NpgsqlCommand(sql, conn);
                command.Parameters.AddWithValue("dataInicio", dataReferencia);
                command.Parameters.AddWithValue("dataFim", dataReferencia.AddDays(1));
                reader = command.ExecuteReader();

                List<Partida> toRtn = new List<Partida>();
                while(reader.Read())
                {
                    Partida partida = new Partida();
                    if (reader["campeonato"] != DBNull.Value)
                        partida.campeonato = reader["campeonato"].ToString();

                    if (reader["horario_partida"] != DBNull.Value)
                        partida.horarioPartida = (DateTime)reader["horario_partida"];

                    if (reader["id"] != DBNull.Value)
                        partida.id = Int64.Parse(reader["id"].ToString());

                    if (reader["entrada_feita"] != DBNull.Value)
                        partida.entradaFeita = Boolean.Parse(reader["entrada_feita"].ToString());

                    Equipe casa = new Equipe();
                    if (reader["casa"] != DBNull.Value)
                        casa.nome = reader["casa"].ToString();
                    if (reader["mediaGolsMarcado_casa"] != DBNull.Value)
                        casa.mediaGolsMarcado = Double.Parse(reader["mediaGolsMarcado_casa"].ToString());
                    if (reader["mediaGolsSofrido_casa"] != DBNull.Value)
                        casa.mediaGolsSofrido = Double.Parse(reader["mediaGolsSofrido_casa"].ToString());
                    if (reader["mediaGolsMarcadosSofridos_casa"] != DBNull.Value)
                        casa.mediaGolsMarcadoSofrido = Double.Parse(reader["mediaGolsMarcadosSofridos_casa"].ToString());
                    if (reader["mediaGolsMarcadosSofridosGeral_casa"] != DBNull.Value)
                        casa.mediaGolsMarcadoSofridoGeral = Double.Parse(reader["mediaGolsMarcadosSofridosGeral_casa"].ToString());
                    if (reader["percentOver25_casa"] != DBNull.Value)
                        casa.percentOver25 = Double.Parse(reader["percentOver25_casa"].ToString());

                    Equipe visitante = new Equipe();
                    if (reader["fora"] != DBNull.Value)
                        visitante.nome = reader["fora"].ToString();
                    if (reader["mediaGolsMarcado_fora"] != DBNull.Value)
                        visitante.mediaGolsMarcado = Double.Parse(reader["mediaGolsMarcado_fora"].ToString());
                    if (reader["mediaGolsSofrido_fora"] != DBNull.Value)
                        visitante.mediaGolsSofrido = Double.Parse(reader["mediaGolsSofrido_fora"].ToString());
                    if (reader["mediaGolsMarcadosSofridos_fora"] != DBNull.Value)
                        visitante.mediaGolsMarcadoSofrido = Double.Parse(reader["mediaGolsMarcadosSofridos_fora"].ToString());
                    if (reader["mediaGolsMarcadosSofridosGeral_fora"] != DBNull.Value)
                        visitante.mediaGolsMarcadoSofridoGeral = Double.Parse(reader["mediaGolsMarcadosSofridosGeral_fora"].ToString());
                    if (reader["percentOver25_fora"] != DBNull.Value)
                        visitante.percentOver25 = Double.Parse(reader["percentOver25_fora"].ToString());

                    partida.casa = casa;
                    partida.visitante = visitante;

                    toRtn.Add(partida);
                }

                return toRtn;
            }
            catch(Exception e)
            {
                Console.WriteLine("ERRO - " +  e.Message);

                throw;
            }
            finally
            {
                if (conn != null)
                    conn.Close();

                if (reader != null)
                    reader.Close();
            }   
        }

        public Boolean salvarResultado(Int64 idPartida, Boolean green, Double valor)
        {
            NpgsqlConnection? conn = null;
            try
            {
                conn = new NpgsqlConnection(strConnection);
                conn.Open();
                String sql = "DELETE FROM tb_resultado_entrada WHERE id_partida = @idPartida;INSERT INTO tb_resultado_entrada(id_partida, resultado, valor) VALUES (@idPartida, @green, @valor);";
                NpgsqlCommand command = new NpgsqlCommand(sql, conn);
                command.Parameters.AddWithValue("idPartida", idPartida);
                command.Parameters.AddWithValue("green", green);
                command.Parameters.AddWithValue("valor", valor);
                command.ExecuteNonQuery();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRO - " + e.Message);

                return false;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        public Boolean salvarEntrada(Int64 idPartida)
        {
            NpgsqlConnection? conn = null;
            try
            {
                conn = new NpgsqlConnection(strConnection);
                conn.Open();
                String sql = "UPDATE dados_partida SET entrada_feita=@entrada WHERE id=@idPartida;";
                NpgsqlCommand command = new NpgsqlCommand(sql, conn);
                command.Parameters.AddWithValue("idPartida", idPartida);
                command.Parameters.AddWithValue("entrada", true);
                command.ExecuteNonQuery();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("ERRO - " + e.Message);

                return false;
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }
    }
}

