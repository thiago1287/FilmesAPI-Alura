using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Data.Dtos;

public class CreateFilmeDto
{
    [Required(ErrorMessage = "O título de filme é obrigatório")]
    public string Titulo { get; set; }
    [Required(ErrorMessage = "O nome do diretor do filme é obrigatório")]
    public string Diretor { get; set; }
    [Required(ErrorMessage = "O gênero de filme é obrigatório")]
    [StringLength(55, ErrorMessage = "O tamanho do gênero näo pode exeder 50 caracteres")]
    public string Genero { get; set; }
    [Required(ErrorMessage = "A duração do filme é obrigatório")]
    [Range(60, 700, ErrorMessage = "A duração deve ser ter entre 60 e 700 minutos")]
    public int Duracao { get; set; }
}
