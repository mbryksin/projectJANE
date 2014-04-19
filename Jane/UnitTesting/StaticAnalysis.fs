namespace UnitTesting

open FsUnit
open NUnit.Framework
open AST
open StaticAnalysis
open LanguageParser

[<TestFixture>]
type TestingStaticAnalusis() =

    let existsError (program : Program) (errorMessage : string) =
        List.exists (fun (a : Error) -> a.ErrorMessage = errorMessage) program.Errors

    [<Test>]
    member x. ``Отсутствует главный класс`` ()=

        let textProgram = "interface a {}"
        let program = ParseProgram textProgram
        program.NameMainClass <- "MainClass"
        SA_Program program
        existsError program "Main class is \"MainClass\" not found." |> should be True
        