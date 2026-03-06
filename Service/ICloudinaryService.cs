namespace AmbustockBackend.Services
{
    //añadida interfaz para el testing
    public interface ICloudinaryService
    {
        Task<(string Url, string PublicId)> SubirImagenAsync(IFormFile archivo, string carpeta = "ambustock/materiales");
        Task EliminarImagenAsync(string publicId);
    }
}