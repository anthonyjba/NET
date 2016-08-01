using Negocio;
using System.ServiceModel.Web;
using Servicios.Interfaces;

namespace Servicios
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ImagenServicio" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ImagenServicio.svc or ImagenServicio.svc.cs at the Solution Explorer and start debugging.
    public class ImagenServicio : IImagenServicio
    {
        #region Negocio

        private ImagenNegocio Negocio
        {
            get
            {
                return new ImagenNegocio();
            }
        }
        #endregion

        #region Métodos de Interface

        public string ObtenerParamsParcela(string referencia, int distancia = 25)
        {
            return Negocio.ObtenerParamsParcela(referencia, distancia);
        }

        public string ObtenerParcelaDEM(int srid, string bbox, int distancia)
        {
            return Negocio.ObtenerParcelaDEM(srid, bbox, distancia);
        }


        //Metodo para quitar porque se reemplaza con el ObtenerParcelaDEM
        public string ObtenerDatos3D(string referencia)
        {
            return Negocio.ObtenerDatos3D(referencia);
        }

        public System.IO.Stream ObtenerImagen(int delegacion, int mapa, int srid, string bbox, string referencia, int ancho, int alto)
        {
            var m = new System.IO.MemoryStream();
            m = Negocio.ObtenerImagen(delegacion, mapa, srid, bbox, referencia, ancho, alto);

            // very important!!! otherwise the client will receive content-length:0
            m.Position = 0;

            WebOperationContext.Current.OutgoingResponse.ContentType = "image/png";
            WebOperationContext.Current.OutgoingResponse.ContentLength = m.Length;
            return m;
        }

        #endregion
        
    }
}
