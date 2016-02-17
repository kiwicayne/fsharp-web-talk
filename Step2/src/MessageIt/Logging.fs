namespace MessageIt.Infrastructure

open MessageIt.Rop

module Logging = 
  let log format (objs : obj []) = 
    System.Diagnostics.Debug.WriteLine("[LOG] " + format, objs)
  
  // log errors on the failure path    
  let logFailure result = 
    // helper to log one error
    let logError err = log "Error: {0}" [| sprintf "%A" err |]
    result |> failureTee (Seq.iter logError) // loop through all errors

  // log values on the success path
  let logSuccess format result = 
    // helper to log the value
    let logSuccess obj = log ("Info: " + format) [| sprintf "%A" obj |]
    result |> successTee logSuccess 