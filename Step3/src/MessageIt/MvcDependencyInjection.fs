namespace MessageIt

open System
open System.Web.Http.Dispatcher
open System.Web.Http.Controllers
open System.Web.Mvc
open MessageIt.Infrastructure
open MessageIt.Controllers

module MvcDependencyInjection = 
  let (|HomeController|_|) type' = 
    if type' = typeof<HomeController> then Some(new HomeController() :> IController)
    else None
  
  let (|MessengerController|_|) getAllMessages type' = 
    if type' = typeof<MessengerController> then      
      let ctlr = new MessengerController(getAllMessages)
      Some(ctlr :> IController)
    else None

  type MvcControllerResolver() = 
    inherit DefaultControllerFactory()
   
    let getAllMessages = InMemoryMessageStore.getAllMessages
    
    override this.GetControllerInstance(requestContext, controllerType) = 
      match controllerType with
      | HomeController ctlr -> ctlr
      | MessengerController getAllMessages ctlr -> ctlr
      | _ -> raise <| ArgumentException((sprintf "Controller type not found: %A" controllerType))
