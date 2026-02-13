using System;
using System.Collections.Generic;

namespace examenDepEmpEFCrudMVC.Models;

public partial class Departamento
{
    public int IdDepartamento { get; set; }

    public string NombreDepartamento { get; set; } = null!;

    public virtual ICollection<Empleado> Empleados { get; set; } = new List<Empleado>();
}
