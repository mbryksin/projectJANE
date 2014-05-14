module SupportFunction

open FsUnit
open NUnit.Framework
open SA.Program
open LanguageParser
open Interpret

let getResult (textProgram : string)=
    let program = ParseProgram textProgram
    program.NameMainClass <- "mainClass"  
    SA_Program program
    interpretProgram program 