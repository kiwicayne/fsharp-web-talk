module MessageIt.DomainPrimitiveTypes

open System
open MessageIt.Rop
open System.Text.RegularExpressions

type StringError = 
  | Missing
  | MustNotBeLongerThan of int
  | DoesntMatchPattern of string

module String20 = 
  // using T to define the type in a module is ONE "best practice" many other ways to structure your code
  type T = String20 of string
  
  let create (s : string) = 
    match s with
    | null -> fail StringError.Missing
    | _ when s.Length > 20 -> fail (MustNotBeLongerThan 20)
    | _ -> succeed (String20 s)
  
  let apply f (String20 s) = f s

module String140 = 
  type T = String140 of string
  
  let create (s : string) = 
    match s with
    | null -> fail StringError.Missing
    | _ when s.Length > 140 -> fail (MustNotBeLongerThan 140)
    | _ -> succeed (String140 s)
  
  let apply f (String140 s) = f s

/// A type for representing a persons name.  
/// Makes it imposible to have an invalid name after creation.
/// This is idomatic F# - very strong type system
module PersonName =
  type T = PersonName of String20.T

  let create (s: string) =
    match String20.create s with
    | Failure errs -> Failure errs
    | Success (s20,_) -> 
      if Regex.IsMatch(s, "^[a-zA-Z\\s]+$") then
        succeed (PersonName s20)
      else
        fail (DoesntMatchPattern "Letters and spaces only")
  
  let apply f (PersonName s20) = 
    // id is the identity function, it is a function that returns its input immediately
    // the String20.apply function above applies a funtion to the unwrapped string so this
    // applies the identity function to the unwrapped string which just returns the unwrapped string
    let s = s20 |> String20.apply id  
    f s

/// A type for representing the text of a message
/// Makes it imposible to have an invalid name after creation.
/// This is idomatic F# - very strong type system
module MessageText =
  type T = MessageText of String140.T

  let create (s: string) =
    match String140.create s with
    | Failure errs -> Failure errs
    | Success (s140,_) -> succeed (MessageText s140)     
  
  let apply f (MessageText s140) =    
    let s = s140 |> String140.apply id  
    f s