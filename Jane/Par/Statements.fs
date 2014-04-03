module Statements
open Microsoft.FSharp.Collections
open AST

type expr = 
  | Val of string 
  | Int of System.Int32


(*type Stmt =
   | Assign of string * expr *)