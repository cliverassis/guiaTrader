using System;
namespace GuiaTrader.Models
{
    public class Usuario
    {
        public enum PRIVILAEGIO
        {
            MONITOR = 0,
            ADMIN = 1
        };
        public Int64 id { get; set; }
        public String usuario { get; set; }
        public String senha { get; set; }
        public PRIVILAEGIO perfil { get; set; }
    }
}

