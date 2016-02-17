namespace MessageIt

open System
open System.Net.Http
open System.Web
open System.Web.Http
open System.Web.Mvc
open System.Web.Routing
open System.Web.Optimization
open MessageIt.MvcDependencyInjection
open System.Web.Http.Dispatcher

type BundleConfig() = 
  static member RegisterBundles(bundles : BundleCollection) = 
    bundles.Add(ScriptBundle("~/bundles/jquery").Include([| "~/Scripts/jquery-{version}.js" |]))
    bundles.Add(ScriptBundle("~/bundles/modernizr").Include([| "~/Scripts/modernizr-*" |]))
    bundles.Add(ScriptBundle("~/bundles/toastr").Include([| "~/Scripts/toastr.js" |]))
    bundles.Add(ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js", "~/Scripts/respond.js"))
    bundles.Add(StyleBundle("~/Content/css").Include("~/Content/bootstrap.css", "~/Content/toastr.css", "~/Content/site.css"))

/// Route for ASP.NET MVC applications
type Route = 
  { controller : string
    action : string
    id : UrlParameter }

type HttpRoute = 
  { controller : string
    id : RouteParameter }

type Global() = 
  inherit System.Web.HttpApplication()
  
  static member RegisterWebApi(config : HttpConfiguration) = 
    // Configure routing
    config.MapHttpAttributeRoutes()
    config.Routes.MapHttpRoute("DefaultApi", // Route name
                               "api/{controller}/{id}", // URL with parameters
                               { controller = "{controller}"
                                 id = RouteParameter.Optional } // Parameter defaults
    ) |> ignore

    // Configure serialization
    config.Formatters.XmlFormatter.UseXmlSerializer <- true
    config.Formatters.JsonFormatter.SerializerSettings.ContractResolver <- Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()

    // Additional Web API settings
    config.Services.Replace(typeof<IHttpControllerActivator>, ApiDependencyInjection.ApiControllerResolver())
  
  static member RegisterFilters(filters : GlobalFilterCollection) = filters.Add(new HandleErrorAttribute())
  
  static member RegisterRoutes(routes : RouteCollection) = 
    routes.IgnoreRoute("{resource}.axd/{*pathInfo}")
    routes.MapRoute("Default", // Route name
                    "{controller}/{action}/{id}", // URL with parameters
                    { controller = "Home"
                      action = "Index"
                      id = UrlParameter.Optional } // Parameter defaults
    ) |> ignore
  
  member x.Application_Start() = 
    AreaRegistration.RegisterAllAreas()
    GlobalConfiguration.Configure(Action<_> Global.RegisterWebApi)
    Global.RegisterFilters(GlobalFilters.Filters)
    Global.RegisterRoutes(RouteTable.Routes)
    BundleConfig.RegisterBundles BundleTable.Bundles

    // Set our custom controller factory to be used to create controllers
    ControllerBuilder.Current.SetControllerFactory(MvcControllerResolver())
