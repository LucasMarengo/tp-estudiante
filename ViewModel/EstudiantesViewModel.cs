using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tpEstudiante.ViewModel
{
    public class EstudiantesViewModel
    {
        public List<Estudiante> Estudiantes { get; set; }
        public SelectList ListCarreras { get; set; }
        public string busquedaEst { get; set; }
        public int? CarreraId { get; set; }
        public Paginador Paginador { get; set; } = new Paginador();
    }

    public class Paginador
    {
        public int PaginaActual { get; set; }
        public int TotalRegistros { get; set; }
        public int RegistrosPorPagina { get; set; }
        public int TotalPaginas => (int)Math.Ceiling((decimal)TotalRegistros / RegistrosPorPagina);

        public Dictionary<string, string> ValoresQueryString { get; set; } = new Dictionary<string, string>();
    }

}
