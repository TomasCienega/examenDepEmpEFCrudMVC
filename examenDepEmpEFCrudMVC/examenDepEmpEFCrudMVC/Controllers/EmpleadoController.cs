using examenDepEmpEFCrudMVC.Context;
using examenDepEmpEFCrudMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace examenDepEmpEFCrudMVC.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly ExamenDepEmpEfcrudMvcContext _context;
        public EmpleadoController(ExamenDepEmpEfcrudMvcContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? id)
        {
            // 1. Instancias el ViewModel (La bolsa)
            var vm = new EmpVM();
            // 2. Llenas la lista de departamentos directamente en el VM
            vm.ListaDepartamentos = await _context.Departamentos.ToListAsync();
            // El ID seleccionado lo puedes dejar en ViewBag porque es un dato simple
            ViewBag.IdSeleccionado = id;
            try
            {
                if (id > 0)
                {
                    // Llenas la lista de empleados dentro del VM usando el SP
                    vm.ListaEmpleados = await _context.Empleados
                        .FromSqlRaw("exec sp_ListarEmpleadoPorDep {0}", id)
                        .ToListAsync();
                }
                else
                {
                    // Llenas la lista de empleados dentro del VM usando Include
                    vm.ListaEmpleados = await _context.Empleados
                        .Include(nD => nD.IdDepartamentoNavigation)
                        .ToListAsync();
                }
                // MANDAS EL VM (La bolsa)
                return View(vm);
            }
            catch (Exception ex) {
                // Si falla, asegúrate de inicializar la lista para que la vista no truene
                vm.ListaEmpleados = new();
                return View(vm);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Guardar(EmpVM empleadoVM)
        {
            try
            {
                if (_context.Empleados != null)
                {
                    await _context.Empleados.AddAsync(empleadoVM.RefModeloEmpleado);
                    await _context.SaveChangesAsync();
                    // Mensaje de éxito
                    TempData["Mensaje"] = "Usuario insertado correctamente.";
                    TempData["Tipo"] = "success";
                }
                else
                {
                    TempData["Mensaje"] = "Error: El formulario envió datos nulos.";
                    TempData["Tipo"] = "danger";
                }
            }
            catch (Exception ex) {
                TempData["Mensaje"] = "No se pudo insertar el usuario: " + ex.Message;
                TempData["Tipo"] = "danger";
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var vm = new EmpVM();
            try
            {
                vm.ListaDepartamentos = await _context.Departamentos.ToListAsync();

                var empleado = await _context.Empleados.FindAsync(id);
                // Opción B: Con FirstOrDefault (Más flexible)
                //var empleado = await _context.Empleados
                //    .FirstOrDefaultAsync(e => e.IdEmpleado == id);

                if (empleado == null)
                {
                    TempData["Mensaje"] = "Error al editar.";
                    TempData["Tipo"] = "danger";
                    return RedirectToAction("Index");
                }
                vm.RefModeloEmpleado = empleado;
                return View(vm);
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "El empleado no existe: " +ex.Message;
                TempData["Tipo"] = "danger";
                return RedirectToAction("Index");
            }    
        }
        [HttpPost]
        public async Task<IActionResult> Editar(EmpVM empleadoVM)
        {
            try
            {
                _context.Empleados.Update(empleadoVM.RefModeloEmpleado);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Datos actualizados correctamente.";
                TempData["Tipo"] = "success";
            }
            catch (Exception ex) {
                TempData["Mensaje"] = "Error al actualizar: " + ex.Message;
                TempData["Tipo"] = "danger";
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var empleado = await _context.Empleados.FindAsync(id);
                if (empleado == null)
                {
                    TempData["Mensaje"] = "El empleado no existe.";
                    TempData["Tipo"] = "danger";
                    return RedirectToAction("Index");
                }
                _context.Empleados.Remove(empleado);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Empleado eliminado correctamente.";
                TempData["Tipo"] = "success"; // Cambié a success para que el usuario sepa que funcionó
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = "Error al eliminar: " + ex.Message;
                TempData["Tipo"] = "danger";
                return RedirectToAction("Index");
            }
        }
    }
}
