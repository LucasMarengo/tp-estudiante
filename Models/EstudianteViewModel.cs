using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tpEstudiante.Models
{
    public class EstudianteViewModel
    {
        public List<Estudiante> Estudiantes { get; set; }
        public SelectList CarrerasSelectList { get; set; }
        public int? CarreraID { get; set; }
        public string nombreBuscar { get; set; }
        public tpEstudiante.ViewModel.Paginador Paginador { get; set; } = new tpEstudiante.ViewModel.Paginador();
    }


}
