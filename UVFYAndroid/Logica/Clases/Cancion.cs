using Logica.DAO;
using Logica.Servicios;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica.Clases
{
    public class Cancion
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("nombre")]
        public string Nombre { get; set; }
        [JsonProperty("duracion")]
        public int Duracion { get; set; }
        [JsonProperty("fechaDeLanzamiento")]
        public DateTime FechaDeLanzamiento { get; set; }
        [JsonProperty("artista")]
        public Artista Artista { get; set; }
        [JsonProperty("album")]
        public Album Album { get; set; }
        [JsonProperty("imagen")]
        public byte[] Imagen { get; set; }
        public byte[] Audio { get; set; }
        public string DireccionDeCancion { get; set; }
        public string DireccionDeCaratula { get; set; }
        public bool CancionEstaDescargada()
        {
            bool respuesta;
            respuesta = ServiciosDeIO.CancionEstaGuardada(this.Id);
            return respuesta;
        }

        public bool CaratulaEstaDescargada()
        {
            bool respuesta;
            respuesta = ServiciosDeIO.CaratulaEstaGuardada(this.Id);
            return respuesta;
        }

        public void CargarDireccionDeCancion()
        {
            if (CancionEstaDescargada())
            {
                DireccionDeCancion = ServiciosDeIO.ConstruirDireccionDeCancion(Id);
            }
            else
            {
                DireccionDeCancion = ServiciosDeIO.ConstruirDireccionTemporalDeCancion(Id);
            }
        }
    }
}
