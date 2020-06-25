using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace UVFYCliente
{
    public interface IControladorDeCambioDePantalla
    {
        void RegresarAInicioDeSesion();

        void CambiarANuevaPage(Page page);

        void Regresar();

        void CambiarNombreDeVentana(string nombre);
    }
}
