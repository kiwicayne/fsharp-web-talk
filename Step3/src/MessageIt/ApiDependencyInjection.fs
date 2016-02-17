namespace MessageIt

open System
open System.Web.Http.Dispatcher
open System.Web.Http.Controllers
open System.Web.Mvc
open MessageIt.Infrastructure
open MessageIt.Controllers
open MessageIt.Domain

module ApiDependencyInjection = 

  let (|MessageController|_|) formatter writer emergencyAlertSystem type' = 
    if type' = typeof<MessageController> then 
      let ctlr = new MessageController(formatter, writer, emergencyAlertSystem)
      Some(ctlr :> IHttpController)
    else None
  
  type ApiControllerResolver() = 

    let writer = Writers.writeToDebug
    let formatter = createFormattedMessage
    let emergencyAlertSystem = Hubs.notifyEmergencyServices 

    interface IHttpControllerActivator with
      member this.Create(request, controllerDescriptor, controllerType) = 
        match controllerType with
        | MessageController formatter writer emergencyAlertSystem ctlr -> ctlr
        | _ -> raise <| ArgumentException((sprintf "API Controller type not found: %A" controllerType))
