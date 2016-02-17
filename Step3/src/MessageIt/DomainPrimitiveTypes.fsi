module MessageIt.DomainPrimitiveTypes

open System
open MessageIt.Rop

type StringError = 
  | Missing
  | MustNotBeLongerThan of int
  | DoesntMatchPattern of string

module String20 = 
  type T  
  val create : string -> RopResult<T, StringError>
  val apply : (string -> 'a) -> T -> 'a

module String140 = 
  type T  
  val create : string -> RopResult<T, StringError>
  val apply : (string -> 'a) -> T -> 'a

module PersonName = 
  type T  
  val create : string -> RopResult<T, StringError>
  val apply : (string -> 'a) -> T -> 'a

module MessageText = 
  type T  
  val create : string -> RopResult<T, StringError>
  val apply : (string -> 'a) -> T -> 'a
