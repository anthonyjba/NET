using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Servicios.Interfaces
{
    [ServiceContract]
    public interface IImagenServicio
    {
        [OperationContract]
        [WebGet(UriTemplate = "ObtenerParamsParcela?referencia={referencia}&distancia={distancia}", ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare)]
        string ObtenerParamsParcela(string referencia, int distancia = 25);
        
        [OperationContract]
        [WebGet(UriTemplate = "ObtenerDatos3D?referencia={referencia}", ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare)]
        string ObtenerDatos3D(string referencia);

        [OperationContract]
        [WebGet(UriTemplate = "ObtenerParcelaDEM?srid={srid}&bbox={bbox}&distancia={distancia}", ResponseFormat = WebMessageFormat.Json,
                   BodyStyle = WebMessageBodyStyle.Bare)]
        string ObtenerParcelaDEM(int srid, string bbox, int distancia);

        [OperationContract]
        [WebGet(UriTemplate = "ObtenerImagen?delegacion={delegacion}&mapa={mapa}&srid={srid}&bbox={bbox}&referencia={referencia}&ancho={ancho}&alto={alto}", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Wrapped)]
        Stream ObtenerImagen(int delegacion, int mapa, int srid, string bbox, string referencia, int ancho, int alto);
    }
}
