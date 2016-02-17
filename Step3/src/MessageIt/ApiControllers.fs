namespace MessageIt.Controllers

open System
open System.Web.Http
open System.Net.Http
open System.Net
open MessageIt
open DomainPrimitiveTypes
open Domain
open MessageIt.Dtos
open Rop
open DtoConverter
open MessageIt.ControllerHelpers
open System.Web.Http.Results
open MessageIt.Infrastructure.Logging

// ============================== 
// api/Message
// ============================== 
[<RoutePrefix("api/message")>]
type MessageController
  (
    formatMessage : MessageRequest -> RopResult<FormattedMessage,_>, 
    sendMessage : FormattedMessage -> RopResult<FormattedMessage,_>,
    alertEmergencyServices: (string * string) -> _
  ) as this = 
  inherit ApiController()
  
  /// simple helpers to make code more readable.
  let okResult content = NegotiatedContentResult(HttpStatusCode.OK, content, this) :> IHttpActionResult
  let toHttpResult result = 
    result 
    |> valueOrDefault (ApiControllerHelper.toHttpResult this)

  let ok = map okResult  
  let format = bind formatMessage 
  let send = bind sendMessage

  //==============================================
  // event handler
  //==============================================
  let notifyEmergencyServicesWhenEmergencyMessageSent = 
    // helper function to filter events
    let detectEvent = function
      | EmergencyMessageSent (name,msg) -> Some (name,msg)
      | _ -> None

    // Perform the notification
    let notifyEmergencyServices (name,msg) =
      alertEmergencyServices (name,msg)    

    // If on success path, find the EmergencyMessageSent event(s)
    // and perform the notification. There can be more than one, we
    // could have choosen to only do it once if the event occurred one
    // or more times.
    successTee (fun (_,msgs) ->
      msgs
      |> List.choose detectEvent
      |> List.iter notifyEmergencyServices
      )

  [<Route("")>]
  [<HttpPost>]
  member this.Send messageRequestDto =        
    messageRequestDto
    |> dtoToMessageRequest              
    |> format                           
    |> send                             
    |> logSuccess "Message sent {0}"    
    |> notifyEmergencyServicesWhenEmergencyMessageSent    
    |> logSuccess "Emergency Message sent {0}"    
    |> logFailure                           
    |> ok                               
    |> toHttpResult