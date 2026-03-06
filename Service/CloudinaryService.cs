using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace AmbustockBackend.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var account = new Account(
                configuration["Cloudinary:CloudName"],
                configuration["Cloudinary:ApiKey"],
                configuration["Cloudinary:ApiSecret"]
            );

            _cloudinary = new Cloudinary(account);
            _cloudinary.Api.Secure = true;
        }

        //Sube una imagen a Cloudinary y devuelve la URL segura y el public_id
        public async Task<(string Url, string PublicId)> SubirImagenAsync(IFormFile archivo, string carpeta = "ambustock/materiales")
        {
            if (archivo == null || archivo.Length == 0)
                throw new ArgumentException("El archivo no puede estar vacío.");

            using var stream = archivo.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(archivo.FileName, stream),
                Folder = carpeta,
                //transforma la imagen a 800px de ancho
                Transformation = new Transformation().Width(800).Crop("limit").Quality("auto").FetchFormat("auto")
            };

            var resultado = await _cloudinary.UploadAsync(uploadParams);

            if (resultado.Error != null)
                throw new Exception($"Error subiendo imagen a Cloudinary: {resultado.Error.Message}");

            return (resultado.SecureUrl.ToString(), resultado.PublicId);
        }

        // Elimina una imagen de Cloudinary por su public_id
        public async Task EliminarImagenAsync(string publicId)
        {
            if (string.IsNullOrEmpty(publicId)) return;

            var deleteParams = new DeletionParams(publicId);
            await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}