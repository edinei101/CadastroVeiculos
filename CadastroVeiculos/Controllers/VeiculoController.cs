using Microsoft.AspNetCore.Mvc;
using CadastroVeiculos.Models;
using CadastroVeiculos.Data;

namespace CadastroVeiculos.Controllers
{
    public class VeiculoController : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            return View(new VeiculoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VeiculoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            PlacaRepository.Adicionar(vm.Placa);

            TempData["Sucesso"] = "Veículo cadastrado com sucesso!";

            return RedirectToAction(nameof(Create));
        }
    }
}