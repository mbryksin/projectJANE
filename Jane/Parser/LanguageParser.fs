module LanguageParser

open Parser
open Lexer
open Microsoft.FSharp.Text.Lexing

let ParseProgram (expression : string) = 
    let lexbuf = LexBuffer<char>.FromString expression
    let ast  = Parser.start Lexer.tokenize lexbuf
    ast