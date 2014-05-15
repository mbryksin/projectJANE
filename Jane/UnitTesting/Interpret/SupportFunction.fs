module SupportFunction

open FsUnit
open NUnit.Framework
open SA
open LanguageParser
open Interpret

let getResult (textProgram : string)=
    let program = ParseProgram textProgram
    program.NameMainClass <- "mainClass"  
    StaticAnalysis.Analyze program
    interpretProgram program 