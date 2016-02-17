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
      // This is the only place the message retrieval is setup, so easy to swap out for a database one
      // No interface bloat like OOP, just function signature matters
      let ctlr = new MessengerController(getAllMessages)
      Some(ctlr :> IController)
    else None

  type MvcControllerResolver() = 
    inherit DefaultControllerFactory()

    // This could be passed in from Global so the true root of the application creates all dependencies and those
    // dependencies could be shared between MVC and API controllers.
    let getAllMessages = InMemoryMessageStore.getAllMessages
    
    override this.GetControllerInstance(requestContext, controllerType) = 
      match controllerType with
      | HomeController ctlr -> ctlr
      | MessengerController getAllMessages ctlr -> ctlr
      | _ -> raise <| ArgumentException((sprintf "Controller type not found: %A" controllerType))
