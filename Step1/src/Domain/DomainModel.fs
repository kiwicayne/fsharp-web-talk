module MessageIt.Domain

// ============================== 
// Domain models
// ============================== 

type Message = 
  { Text : string }

type Person = 
  { Name : string }

type FormattedMessage =
  { Content : string }

// ============================== 
// Domain functions
// ============================== 

// With a large domain, you would wrap related functions into modules and move them
// into their own files.  Despite being in the same file, the data and functions that 
// operate on that data are separate, unlike OOP.  

let format message recipient =
    { Content = sprintf "%s %s." message.Text recipient.Name }