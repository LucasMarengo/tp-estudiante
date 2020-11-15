using System;
using System.Collections.Generic;
using System.Text;

namespace tpEstudiante
{
    public class Materia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<EstudianteMateria> EstudianteMateria { get; set; }
    }
}
