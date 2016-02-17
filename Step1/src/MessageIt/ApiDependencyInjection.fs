namespace MessageIt

open System
open System.Web.Http.Dispatcher
open System.Web.Http.Controllers
open System.Web.Mvc
open MessageIt.Infrastructure
open MessageIt.Controllers
open MessageIt.Domain

module ApiDependencyInjection = 

  let (|MessageController|_|) formatter writer type' = 
    if type' = typeof<MessageController> then 
      let ctlr = new MessageController(formatter, writer)
      Some(ctlr :> IHttpController)
    else None
  
  type ApiControllerResolver() = 

    // These could be passed in from Global so the true root of the application creates all dependencies and those
    // dependencies could be shared between MVC and API controllers.  
    let writer = DebugWriter.write
    let formatter = format

    interface IHttpControllerActivator with
      member this.Create(request, controllerDescriptor, controllerType) = 
        match controllerType with
        | MessageController formatter writer ctlr -> ctlr
        | _ -> raise <| ArgumentException((sprintf "API Controller type not found: %A" controllerType))
