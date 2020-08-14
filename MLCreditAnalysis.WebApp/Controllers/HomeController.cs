using CreditAnalysis.Model;
using CreditAnalysis.Service.Interfaces;
using Infrastructure.Layer.Web.Base;
using Microsoft.AspNetCore.Mvc;
using MLCreditAnalysis.WebApp.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MLCreditAnalysis.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ICreditAnalysisService _creditAnalysisService;

        public HomeController(ICreditAnalysisService creditAnalysisService)
        {
            this._creditAnalysisService = creditAnalysisService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult WebCam()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> IndexAsync(ClientCreditAnalysisModel clientCreditAnalysisModel)
        {
            await this._creditAnalysisService.DoCreditAnalysis(clientCreditAnalysisModel);

            return base.ApiResponse(clientCreditAnalysisModel, this._creditAnalysisService);
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
    }
}
