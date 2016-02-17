module MessageIt.DomainPrimitiveTypes

open System
open MessageIt.Rop

// This is a signature file for DomainPrimitiveTypes.fs, it has the same name but with a fsi extension.
// There must be an entry in this sigature file for everything in DomainPrimitiveTypes.fs that can be visible to the
// rest of the application.  A signature file allows specific types to be associated with functions outside of the main
// code file, and visibility of functions and types to be controlled. It is a subset of the namespaces, modules, types, members
// in the impelementation file.  By ommitting something, the compiler will no longer
// allow its use elsewhere.  
// This allows for the equivalent of a private constructor on PersonName for example, as type T
// is exposed, but not its definition (PersonName of String20), so you can construct an instance without calling create.
// If this wasn't done you could do this PersonName.T.PersonName (String20.T.String20 "myname") 
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
