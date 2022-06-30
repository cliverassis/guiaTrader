﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GuiaTrader.Models;
using System.Net;

namespace GuiaTrader.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (Facade.Fachada.getInstance().getUsuarioAtual() != null)
            return View("InicioGuiaTrader", Facade.Fachada.getInstance().getUsuarioAtual());
        else
            return View();
    }

    [HttpGet]
    public IActionResult Logout()
    {
        Facade.Fachada.getInstance().Logout();

        return RedirectToAction("Index");
    }


    [HttpGet]
    public ViewResult InicioGuiaTrader(Usuario user)
    {
        if (user.id > 0)
            return View();

        if (Facade.Fachada.getInstance().verifyUser(user.usuario, user.senha))
            return View();
        else
            return View("Index");
    }

    [HttpGet]
    public PartialViewResult getResumoMes(DateTime dataReferencia)
    {
        ResultadoPartidaViewModel model = new ResultadoPartidaViewModel();
        model.dataReferencia = new DateTime(dataReferencia.Year, dataReferencia.Month, DateTime.DaysInMonth(dataReferencia.Year, dataReferencia.Month));
        model.listaResultadoPartida = Facade.Fachada.getInstance().GetResumoMes(model.dataReferencia);

        return PartialView(model);
    }

    [HttpGet]
    public PartialViewResult getPartidas(DateTime dataReferencia)
    {
        PartidaModelView model = new PartidaModelView();
        model.dataReferencia = dataReferencia;
        model.listaPartidas = Facade.Fachada.getInstance().GetPartidasBTTS(dataReferencia);

        return PartialView(model);
    }

    [HttpPost]
    public Boolean salvarResultado(Int64 idPartida, Boolean green, Double valor)
    {
        return Facade.Fachada.getInstance().salvarResultado(idPartida, green, valor);
    }

    [HttpPost]
    public Boolean salvarEntrada(Int64 idPartida)
    {
        return Facade.Fachada.getInstance().salvarEntrada(idPartida);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

