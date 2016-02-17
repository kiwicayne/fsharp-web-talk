namespace MessageIt.Infrastructure

open MessageIt.Domain
open MessageIt.Rop

module ConsoleWriter =   
  let write formattedMessage = 
    System.Console.WriteLine formattedMessage.Content
    succeed formattedMessage // return the input so this function can be chained with others

module DebugWriter = 
  let write formattedMessage = 
    System.Diagnostics.Debug.WriteLine formattedMessage.Content
    succeed formattedMessage // return the input so this function can be chained with others
