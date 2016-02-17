namespace MessageIt.Infrastructure

open MessageIt.DomainPrimitiveTypes
open MessageIt.Domain
open MessageIt.Rop

module Writers =   
  let EmergencyMessage = "HELP!"

  let succeed formattedMessage =
    let isEmergency = MessageText.apply (fun msg -> msg = EmergencyMessage) 
    if isEmergency formattedMessage.MessageRequest.Message.Text then
      let valueOf = PersonName.apply id
      let event = EmergencyMessageSent (valueOf formattedMessage.MessageRequest.Person.Name, formattedMessage.Content)
      succeedWithMsg formattedMessage event
    else
      succeed formattedMessage

  let writeToConsole formattedMessage = 
    System.Console.WriteLine formattedMessage.Content
    succeed formattedMessage

  let writeToDebug formattedMessage = 
    System.Diagnostics.Debug.WriteLine formattedMessage.Content
    succeed formattedMessage
