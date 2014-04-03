module LanguageParser

open Parser
open Lexer
open Microsoft.FSharp.Text.Lexing
open Counter

let ParseProgram (expression : string) = 
    let lexbuf = LexBuffer<char>.FromString expression
    let ast  = Parser.start Lexer.tokenize lexbuf
    ast