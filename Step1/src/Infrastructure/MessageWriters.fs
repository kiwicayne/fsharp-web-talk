namespace MessageIt.Infrastructure

open MessageIt.Domain

module ConsoleWriter =   
  let write formattedMessage = 
    System.Console.WriteLine formattedMessage.Content
    formattedMessage // return the input so this function can be chained with others

module DebugWriter = 
  let write formattedMessage = 
    System.Diagnostics.Debug.WriteLine formattedMessage.Content
    formattedMessage // return the input so this function can be chained with others
