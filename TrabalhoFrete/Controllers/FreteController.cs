using Microsoft.AspNetCore.Mvc;
using TrabalhoFrete.ClasseFrete;

[ApiController]
[Route("api/[controller]")]
public class FreteController : ControllerBase
{
    private const float TaxaPorCm3 = 0.01f;

    private static readonly Dictionary<string, float> TarifaPorEstado = new Dictionary<string, float>
    {
        { "SP", 50.0f },
        { "RJ", 60.0f },
        { "MG", 55.0f },
        { "OUTROS", 70.0f }
    };

    [HttpPost]
    public IActionResult CalcularFrete([FromBody] Classdadosfrete classdadosfrete)
    {
        if (classdadosfrete == null)
        {
            return BadRequest("Dados inválidos.");
        }

        float volume = classdadosfrete.Altura * classdadosfrete.Largura * classdadosfrete.Comprimento;
        float tarifaEstado = TarifaPorEstado.ContainsKey(classdadosfrete.UF.ToUpper()) ?
                              TarifaPorEstado[classdadosfrete.UF.ToUpper()] :
                              TarifaPorEstado["OUTROS"];

        float valorFrete = (volume * TaxaPorCm3) + tarifaEstado;

        return Ok(new
        {
            Classdadosfrete = classdadosfrete.Nome,
            Volume = volume,
            ValorFrete = valorFrete
        });
    }
}
