module LanguageParser

open Parser
open Lexer
open Microsoft.FSharp.Text.Lexing

let ParseProgram (expression : string) = 
    let lexbuf = LexBuffer<char>.FromString expression
    let ast  = try
                  Parser.start Lexer.tokenize lexbuf
               with e ->
                  let pos = lexbuf.EndPos
                  let line = pos.Line
                  let column = pos.Column
                  let message = e.Message
                  let lastToken = new System.String(lexbuf.Lexeme)
                  printf "Parse failed at line %d, column %d:\n" line column
                  printf "Last loken: %s" lastToken
                  printf "\n"
                  exit 1
    ast   