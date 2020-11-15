using System;
using System.Collections.Generic;
using System.Text;

namespace tpEstudiante
{
    public class Estudiante
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public string Legajo { get; set; }
        public int Año { get; set; }
        public string Calle { get; set; }
        public string Localidad { get; set; }
        public int? CarreraId { get; set; }
        public Carrera Carrera { get; set; }
        public List<EstudianteMateria> EstudianteMateria { get; set; }
        public string Foto { get; set; }

    }
}
