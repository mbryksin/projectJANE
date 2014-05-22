namespace RunProject

open System
open AST
open SA
open LanguageParser
open Interpret

type ProjectResult(code: string, mainclass: string) =
    let textCode = code 
    let mutable errors:Collections.Generic.List<Error> = new Collections.Generic.List<Error>()
    let mutable runResultValue = ""
    let mainClassName = mainclass

    member this.Errors with get() = errors
    member this.NoErrors = this.Errors.Count = 0
    
    member this.RunResultValue with get() = runResultValue 
    
    member this.StartRunning() = 
        let program = ParseProgram textCode
        program.NameMainClass <- mainClassName
        StaticAnalysis.Analyze program
        let errs = program.Errors
        List.iter (fun x -> errors.Add(x)) errs
        if (this.NoErrors)
            then
                let interpretResult = interpretProgram program
                runResultValue <- "Program finished: " + interpretResult
