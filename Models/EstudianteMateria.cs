using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace tpEstudiante
{
    public class EstudianteMateria
    {
        public int EstudianteId { get; set; }
        public int MateriaId { get; set; }
        public Estudiante Estudiante { get; set; }
        public Materia Materia { get; set; }
        [Display(Name = "Año Inscripcion")]
        public DateTime FechaInscripcion { get; set; }
    }
}
