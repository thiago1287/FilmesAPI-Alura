using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{

    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);                                                              // Adiciona filme
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperarFilmePorId), new { id = filme.Id }, filme);        //retorna o que foi criado e onde encontrar o caminho do objeto criado
    }

    /// <summary>
    /// Consulta todos os filmes cadastrados no banco de dados
    /// </summary>
    /// <param name="skip">Quantos objetos a partir do primeiro eu vou pular</param>
    /// <param name="take">Quantos objetos após o skip eu vou pegar para exibir</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso busca seja feita com sucesso</response>

    [HttpGet]
    public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery] int skip = 0, [FromQuery] int take = 50) // Recupera TODOS so filmes, onde skip é quantos tem que pular e take quantos tem que pegar esses parametros sao passados via querry ex: "?skip=2" na url 
    {
        return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take));
    }

    /// <summary>
    /// Consulta um filme especifico cadastrado no banco de dados
    /// </summary>
    /// <param name="id">Objeto referente ao ID do filme que vai ser buscado</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso busca seja feita com sucesso</response>

    [HttpGet("{id}")]
    public IActionResult RecuperarFilmePorId(int id) // Recupera UM filme por id
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound("Filme não Econtrado");
        var filmeDto = _mapper.Map<ReadFilmeDto>(filme);
        return Ok(filmeDto);
    }

    /// <summary>
    /// Altera todas os campos relacionados a um filme cadastrado no banco de dados
    /// </summary>
    /// <param name="id">Objeto referente ao ID do filme que vai ser alterado</param>
    /// <param name="filmeDto">Objeto com os campos que vão ser alterados no filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Sucesso</response>
    /// <response code="204">Caso alteração seja feita com sucesso</response>

    [HttpPut("{id}")]
    public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        _mapper.Map(filmeDto, filme);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Altera uma ou mais campos especificos de um filme cadastrado no banco de dados
    /// </summary>
    /// <param name="id">Objeto referente ao ID do filme que vai ser alterado</param>
    /// <param name="patch">Objeto com o ou os campos que vão ser alterados</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Sucesso</response>
    /// <response code="204">Caso alteração seja feita com sucesso</response>

    [HttpPatch("{id}")]
    public IActionResult AtualizaFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme);

        patch.ApplyTo(filmeParaAtualizar, ModelState);

        if (!TryValidateModel(filmeParaAtualizar))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(filmeParaAtualizar, filme);
        _context.SaveChanges();
        return NoContent();
    }

    /// <summary>
    /// Deleta um filme do banco de dados
    /// </summary>
    /// <param name="id">Objeto referente ao ID do filme que vai ser deletado</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Sucesso</response>
    /// <response code="204">Caso a exclusão seja feita com sucesso</response>

    [HttpDelete("{id}")]
    public IActionResult DeletaFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();
        _context.Remove(filme);
        _context.SaveChanges();
        return NoContent();
    }
}
