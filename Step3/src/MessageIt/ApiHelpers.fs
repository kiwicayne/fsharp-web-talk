namespace MessageIt.ControllerHelpers

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

// =============================== 
// Helpers for the API controllers
// ===============================
module ApiControllerHelper = 
  open System.Web.Http.Results

  /// This type represents a reduced set of choices.
  /// Each DomainMessage is classified into one of these groups
  /// and converted into strings if appropriate.
  /// They are ordered from most important to least, so that 
  /// a NotFound beats a BadRequest, a BadRequest beats a InternalServerError, etc. 
  type ResponseMessage = 
    | NotFound
    | BadRequest of string
    | InternalServerError of string
    | DomainEvent of DomainMessage 
      
  // classify a domain message into one of the standard responses
  // If you add new domain messages this will fail to compile until you have extended it!
  let classify msg = 
    match msg with
    | PersonNameIsRequired 
    | PersonNameMustNotBeMoreThan20Chars 
    | PersonNameMustContainOnlyLettersAndSpaces
    | MessageTextIsRequired
    | MessageTextMustNotBeMoreThan140Chars
      // This will just show each of the above as a string.  You could
      // handle each one and have custom text, or even better create a mapping for 
      // each making it easy to translate to another language later on
      -> BadRequest(sprintf "%A" msg) 

    | EmergencyMessageSent _
      -> DomainEvent msg
      
   
  // return the most important response
  // in the list of messages
  let primaryResponse msgs = 
    msgs
    |> List.map classify
    |> List.sort
    |> List.head     
  
  // return all the BadRequests in the list of messages as a single string
  let badRequestsToStr msgs = 
    msgs
    |> List.map classify
    |> List.choose (function 
         | BadRequest s -> Some s
         | _ -> None)
    |> List.map (sprintf "ValidationError: %s; ")
    |> List.reduce (+)  
  
  // return all the DomainEvents in the list of messages as a single string
  let domainEventsToStr msgs = 
    msgs 
    |> List.map classify
    |> List.choose (function DomainEvent s -> Some s |_ ->  None)
    |> List.map (sprintf "DomainEvent: %A; ")
    |> List.reduce (+)

  // return an overall IHttpActionResult for a set of messages
  let toHttpResult (controller : ApiController) msgs : IHttpActionResult = 
    match primaryResponse msgs with
    | NotFound -> upcast NotFoundResult(controller)
    | BadRequest _ -> 
      // find all events and accumulate them
      let validationMsg = badRequestsToStr msgs
      upcast NegotiatedContentResult(HttpStatusCode.BadRequest, validationMsg, controller)
    | InternalServerError msg -> 
      upcast NegotiatedContentResult(HttpStatusCode.InternalServerError, msg, controller)    
    | DomainEvent _ -> 
      // find all events and accumulate them
      let eventsMsg = domainEventsToStr msgs
      upcast NegotiatedContentResult(HttpStatusCode.OK, eventsMsg,controller) 
