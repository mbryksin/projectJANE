namespace RunProject

open AST
open SA
open LanguageParser
open Interpret

type ProjectResult(code: string) =
    let textCode = code
    member this.getResult = "Program finished!"