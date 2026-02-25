public class CreateReposicionDto
{
    public string NombreResponsable { get; set; }
    public List<string> NombresMateriales { get; set; }
    public int? Cantidad { get; set; }
    public string? Comentarios { get; set; }
    public List<string>? FotosBase64 { get; set; }  
}
