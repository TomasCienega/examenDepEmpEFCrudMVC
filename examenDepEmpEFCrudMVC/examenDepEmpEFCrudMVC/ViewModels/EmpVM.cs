using examenDepEmpEFCrudMVC.Models;

namespace examenDepEmpEFCrudMVC.ViewModels
{
    public class EmpVM
    {
        public List<Empleado> ListaEmpleados { get; set; } = new();
        public Empleado RefModeloEmpleado { get; set; } = new();
        public List<Departamento> ListaDepartamentos { get; set; } = new();
    }
}
