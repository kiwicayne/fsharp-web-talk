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
type MessageController(formatMessage : MessageRequest -> RopResult<FormattedMessage,_>, sendMessage : FormattedMessage -> RopResult<FormattedMessage,_>) as this = 
  inherit ApiController()
  
  /// simple helpers to make code more readable.
  let okResult content = NegotiatedContentResult(HttpStatusCode.OK, content, this) :> IHttpActionResult

  let ok = map okResult  
  let format = bind formatMessage 
  let send = bind sendMessage
  let toHttpResult result = 
    result 
    |> valueOrDefault (ApiControllerHelper.toHttpResult this)

  // This is my preferred style, more typing, but where the actual work is done it is very easy to 
  // understand everything that is being done, not distracted with reading the operators being applied at
  // each step
  [<Route("")>]
  [<HttpPost>]
  member this.Send messageRequestDto =        
    // Very easy to read and a lot is being done!
    //  - includes validation, error handling, tracing, error logging! not a single try/catch/if/etc.

    messageRequestDto
    |> dtoToMessageRequest              // Create a MessageRequest domain type instance from the dto
    |> format                           // Create a formatted message from the MessageRequest
    |> logSuccess "Message to send {0}" // If success so far, log the formatted message (e.g. you could use this for tracing)
    |> send                             // Send the formatted message
    |> logSuccess "Message sent {0}"    // If success so far, log the formatted message that was sent

    // From here down, you would typically repeat this for any API call - its a pattern
    |> logFailure                       // Log any errors
    |> ok                               // If we are still on the success track create an OK http action result from the formatted message
    |> toHttpResult                     // Create an http result
                                        //   a different result is created depending on where a failure occurred
                                        //   if everything succeeded, the OK result in the last step of the success track is returned

  // This is ok, but I think the above is easier to understand at a glance without the operators (bind, map) 
  [<Route("send2")>]
  [<HttpPost>]
  member this.Send2 messageRequestDto =         
    dtoToMessageRequest messageRequestDto
    |> bind formatMessage
    |> logSuccess "Message to send {0}"
    |> bind sendMessage
    |> logSuccess "Message sent {0}"
    |> logFailure 
    |> ok
    |> toHttpResult


  // Another example, this time more realistic without the extra logging and comments.  
  // A typical api method would look similar to this.
  [<Route("send3")>]
  [<HttpPost>]
  member this.Send3 messageRequestDto =               
    messageRequestDto
    |> dtoToMessageRequest                
    |> format                           
    |> send                             
    |> logFailure                       
    |> ok                               
    |> toHttpResult                     
                                           