using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Negocio
{
    public class ImagenNegocio
    {
        #region Persistencia

        /// <summary>
        /// Definimos su Repositorio
        /// </summary>
        private Persistencias.ImagenPersistencia Persistencia
        {
            get { return new Persistencias.ImagenPersistencia(); }
        }

        #endregion

        #region Metodos Publicos

        public string ObtenerParamsParcela(string referencia, int distancia)
        {
            if (!validaParametros.PorReferenciaCatastral(referencia))
            {
                return string.Empty;
            }

            if (distancia == 0) { distancia = 25; }

            return Persistencia.ObtenerParamsParcela(referencia, distancia);
        }

        public string ObtenerDatos3D(string referencia)
        {
            if (!validaParametros.PorReferenciaCatastral(referencia))
            {
                return string.Empty;
            }
            return Persistencia.ObtenerDatos3D(referencia);
        }

        public string ObtenerParcelaDEM(int srid, string bbox, int distancia)
        {
            if (!validaParametros.PorBoundingBox(bbox))
            {
                return string.Empty;
            }

            if (distancia == 0) { distancia = 25; }

            return Persistencia.ObtenerParcelaDEM(srid, bbox, distancia);
        }

        public MemoryStream ObtenerImagen(int delegacion, int mapa, int srid, string bbox, string referencia, int ancho, int alto)
        {
            
            string url = Url_Parcela_Ortofoto(ancho, alto, bbox, srid); //Ahora pide autentication
            //string url = Url_Parcela_Ortofoto(delegacion, mapa, bbox, ancho, alto, srid, referencia);

            MemoryStream outStream = new MemoryStream();

            using (Bitmap b = new Bitmap(ImagenFromUrl(url, true)))
            {
                
                using (Graphics g = Graphics.FromImage(b))
                {
                    url = Url_SEDE_CATASTRO(delegacion, mapa, bbox, ancho, alto, srid, referencia);

                    try
                    {
                        Stream imgSede = ImagenFromUrl(url, false);
                        if (!imgSede.CanSeek)
                        {
                            Bitmap b_lin = new Bitmap(imgSede);
                            g.DrawImage(b_lin, 0, 0, b.Width, b.Height);
                        }
                        
                    }
                    catch { 
                    }

                }

                b.Save(outStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                /*ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

                //ImageCodecInfo pngEncoder = GetEncoder(ImageFormat.Png);
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 70L);
                myEncoderParameters.Param[0] = myEncoderParameter;

                b.Save(@"C:\27034A04600475.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

                b.Save(@"C:\27034A04600475_comp.jpg", jpgEncoder, myEncoderParameters);
                */
                
                //b.Save(@"C:\AVR\Wrk\PNG_Transp\PngTransp\pngs\27034A04600475.png", jpgEncoder, myEncoderParameters);

            }

            return outStream;
        }

        #endregion

        #region Metodos Privados

        private static string Url_Parcela_Ortofoto(int ancho, int alto, string bbox, int srid)
        {
            //Temnplate: //"http://www.mapabase.es/arcgis/rest/services/Raster/MapaBase_o_ETRS89_30N/MapServer/export?F=image&FORMAT=PNG32&TRANSPARENT=true&SIZE=" + ancho + "%2C" + alto + "&BBOX=" + bbox + "&BBOXSR=" + srid + "&IMAGESR=" + srid + "&DPI=90";

            string url = ConfigurationManager.AppSettings["PARCELA_ortofoto"];

            url = url.Replace("{ancho}", ancho.ToString());
            url = url.Replace("{alto}", alto.ToString());
            url = url.Replace("{bbox}", bbox.ToString(CultureInfo.InvariantCulture));
            url = url.Replace("{srid}", srid.ToString(CultureInfo.InvariantCulture));
            return url;
        }
        
        private static string Url_SEDE_CATASTRO(int delegacion, int mapa, string bbox, int ancho, int alto, int srid, string referencia)
        {
            //Template:  //"https://www1.sedecatastro.gob.es/Cartografia/GeneraMapa.aspx?del=" + delegacion + "&mapa=" + mapa + "&formato=png&XMin=" + bb[0] + "&YMin=" + bb[1] + "&XMax=" + bb[2] + "&YMax=" + bb[3] + "&AnchoPixels=" + ancho + "&AltoPixels=" + alto + "&Transparente=S&layers=CATASTRO&huso=" + srid + "&RefCat=" + referencia;

            string url = ConfigurationManager.AppSettings["SEDE_CATASTRO_mapa"];
            var bb = bbox.Split(',');

            url = url.Replace("{delegacion}", delegacion.ToString());
            url = url.Replace("{mapa}", mapa.ToString());
            url = url.Replace("{bbox.0}", bb[0].ToString(CultureInfo.InvariantCulture));
            url = url.Replace("{bbox.1}", bb[1].ToString(CultureInfo.InvariantCulture));
            url = url.Replace("{bbox.2}", bb[2].ToString(CultureInfo.InvariantCulture));
            url = url.Replace("{bbox.3}", bb[3].ToString(CultureInfo.InvariantCulture));
            url = url.Replace("{ancho}", ancho.ToString());
            url = url.Replace("{alto}", alto.ToString());
            url = url.Replace("{srid}", srid.ToString(CultureInfo.InvariantCulture));
            url = url.Replace("{referencia}", referencia.ToString());
            return url;
        }

        private Stream ImagenFromUrl(string url, bool asignaProxy)
        {
            Stream dataStream = new MemoryStream();
            try
            {
                WebRequest request = WebRequest.Create(url);

                if (asignaProxy) {
                    WebProxy proxyObject = new WebProxy("http://proxy.catastro.minhac.es:80/");
                    proxyObject.UseDefaultCredentials = true;

                    // If required by the server, set the credentials.
                    request.Proxy = proxyObject;
                }

                // Request mutual authentication.
                request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
                request.Credentials = CredentialCache.DefaultCredentials;
                // Get the response.

                WebResponse response = request.GetResponse();

                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();

              /*  if (response.ContentType == "image/png")
                {                    
                    dataStream = response.GetResponseStream();
                }
                else
                {
                    Logger.filePath = @"c:\Compartida\log1.txt";
                    Logger.log("Error: " + response);

                }
*/
            }
            catch (Exception ex)
            {
                //Logger.filePath = @"c:\Logs\logServ.txt";
                //Logger.log("Error: " + ex.Message + "  " +ex.StackTrace);
            }
         

            return dataStream;
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        #endregion

        
    }
}
