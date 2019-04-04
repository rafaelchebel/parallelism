using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tasks._01.Models;
using Tasks._01.Services;

namespace Tasks._01.Controllers
{
    public class HomeController : Controller
    {
        ContaClienteService contaClienteService = null;
        CancellationTokenSource _cts = null;

        public HomeController()
        {
            contaClienteService = new ContaClienteService();
            _cts = new CancellationTokenSource();
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string[] accounts)
        {
            _cts = new CancellationTokenSource();

            var contas = contaClienteService.GetContaClientes();

            var inicio = DateTime.Now;

            string[] result = null;
            try
            {
                result = await contaClienteService.ConsolidarContas(contas, _cts.Token);

                var fim = DateTime.Now;
                AtualizarView(result, fim - inicio);
            }
            catch (OperationCanceledException)
            {
                //TxtTempo.Text = "Operação cancelada pelo usuário";
            }
            finally
            {
                //BtnProcessar.IsEnabled = true;
                //BtnCancelar.IsEnabled = false;
            }

            return View(result);
        }

        private void AtualizarView(IEnumerable<String> result, TimeSpan elapsedTime)
        {
            var tempoDecorrido = $"{ elapsedTime.Seconds }.{ elapsedTime.Milliseconds} segundos!";
            var mensagem = $"Processamento de {result.Count()} clientes em {tempoDecorrido}";

            ViewBag.TimeElapsed = mensagem;
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Process(string id)
        {
            var contas = contaClienteService.GetContaClientes();

            var inicio = DateTime.Now;

            string[] result = null;
            try
            {
                result = await contaClienteService.ConsolidarContas(contas, _cts.Token);

                var fim = DateTime.Now;
                AtualizarView(result, fim - inicio);
            }
            catch (OperationCanceledException)
            {
                //TxtTempo.Text = "Operação cancelada pelo usuário";
            }
            finally
            {
                //BtnProcessar.IsEnabled = true;
                //BtnCancelar.IsEnabled = false;
            }

            return View("Index", result);
        }

        public IActionResult Cancel()
        {
            _cts.Cancel();

            return View("Index");
        }
    }
}
