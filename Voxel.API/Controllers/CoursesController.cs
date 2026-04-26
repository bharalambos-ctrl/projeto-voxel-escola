using Microsoft.AspNetCore.Mvc;
using Voxel.Application.DTOs;

namespace Voxel.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        // Endpoint simples retornando cursos estáticos para ligar ao frontend
        [HttpGet]
        public IActionResult GetAll()
        {
            var courses = new[] {
                new { id = 1, title = "Desenvolvimento Web", description = "Aprenda HTML, CSS e JavaScript" },
                new { id = 2, title = "Back-end", description = "Domine APIs e bancos de dados" },
                new { id = 3, title = "Data Science", description = "Introdução a Python e análise de dados" }
            };

            return Ok(courses);
        }
    }
}
