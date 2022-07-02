using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GuiaTrader.Models;
using System.Net;
using System.Text.Json;
using Newtonsoft.Json;
namespace GuiaTrader.Controllers;

public static class SessionExtensions
{
    public static void SetObjectAsJson(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    public static T GetObjectFromJson<T>(this ISession session, string key)
    {
        var value = session.GetString(key);

        return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
    }
}

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
        Facade.Fachada fachada;
        if (HttpContext.Session.GetObjectFromJson<Facade.Fachada>("instance") != null)
            fachada = HttpContext.Session.GetObjectFromJson<Facade.Fachada>("instance");
        else
        {
            fachada = new Facade.Fachada();
            HttpContext.Session.SetObjectAsJson("instance", fachada);
        }
            
        if (SessionExtensions.GetObjectFromJson<Usuario>(HttpContext.Session, "user") != null)
            return View("InicioGuiaTrader", SessionExtensions.GetObjectFromJson<Usuario>(HttpContext.Session, "user"));
        else
            return View();
    }

    [HttpGet]
    public IActionResult Logout()
    {
        Facade.Fachada fachada;
        if (HttpContext.Session.GetObjectFromJson<Facade.Fachada>("instance") != null)
            fachada = HttpContext.Session.GetObjectFromJson<Facade.Fachada>("instance");
        else
        {
            fachada = new Facade.Fachada();
            HttpContext.Session.SetObjectAsJson("instance", fachada);
        }

        fachada.Logout();

        HttpContext.Session.Remove("user");

        return RedirectToAction("Index");
    }


    [HttpGet]
    public ViewResult InicioGuiaTrader(Usuario user)
    {
        Facade.Fachada fachada;
        if (HttpContext.Session.GetObjectFromJson<Facade.Fachada>("instance") != null)
            fachada = HttpContext.Session.GetObjectFromJson<Facade.Fachada>("instance");
        else
        {
            fachada = new Facade.Fachada();
            HttpContext.Session.SetObjectAsJson("instance", fachada);
        }

        if (user.id > 0)
            return View();

        if (fachada.verifyUser(user.usuario, user.senha))
        {
            HttpContext.Session.SetObjectAsJson("user", fachada.getUsuarioAtual());
            return View();
        }
        else
            return View("Index");
    }

    [HttpGet]
    public PartialViewResult getResumoMes(DateTime dataReferencia)
    {
        Facade.Fachada fachada;
        if (HttpContext.Session.GetObjectFromJson<Facade.Fachada>("instance") != null)
            fachada = HttpContext.Session.GetObjectFromJson<Facade.Fachada>("instance");
        else
        {
            fachada = new Facade.Fachada();
            HttpContext.Session.SetObjectAsJson("instance", fachada);
        }

        ResultadoPartidaViewModel model = new ResultadoPartidaViewModel();
        model.dataReferencia = new DateTime(dataReferencia.Year, dataReferencia.Month, DateTime.DaysInMonth(dataReferencia.Year, dataReferencia.Month));
        model.listaResultadoPartida = fachada.GetResumoMes(model.dataReferencia);

        return PartialView(model);
    }

    [HttpGet]
    public PartialViewResult getPartidas(DateTime dataReferencia)
    {
        Facade.Fachada fachada;
        if (HttpContext.Session.GetObjectFromJson<Facade.Fachada>("instance") != null)
            fachada = HttpContext.Session.GetObjectFromJson<Facade.Fachada>("instance");
        else
        {
            fachada = new Facade.Fachada();
            HttpContext.Session.SetObjectAsJson("instance", fachada);
        }

        PartidaModelView model = new PartidaModelView();
        model.dataReferencia = dataReferencia;
        model.listaPartidas = fachada.GetPartidasBTTS(dataReferencia);
        model.usuarioAtual = SessionExtensions.GetObjectFromJson<Usuario>(HttpContext.Session, "user");

        return PartialView(model);
    }

    [HttpPost]
    public Boolean salvarResultado(Int64 idPartida, Boolean green, Double valor)
    {
        Facade.Fachada fachada;
        if (HttpContext.Session.GetObjectFromJson<Facade.Fachada>("instance") != null)
            fachada = HttpContext.Session.GetObjectFromJson<Facade.Fachada>("instance");
        else
        {
            fachada = new Facade.Fachada();
            HttpContext.Session.SetObjectAsJson("instance", fachada);
        }

        return fachada.salvarResultado(idPartida, green, valor);
    }

    [HttpPost]
    public Boolean salvarEntrada(Int64 idPartida)
    {
        Facade.Fachada fachada;
        if (HttpContext.Session.GetObjectFromJson<Facade.Fachada>("instance") != null)
            fachada = HttpContext.Session.GetObjectFromJson<Facade.Fachada>("instance");
        else
        {
            fachada = new Facade.Fachada();
            HttpContext.Session.SetObjectAsJson("instance", fachada);
        }

        return fachada.salvarEntrada(idPartida);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

