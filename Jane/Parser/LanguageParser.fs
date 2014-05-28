module LanguageParser

open AST
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
                  let lastToken = new System.String(lexbuf.Lexeme)
                  let message = sprintf "Parse failed at line %d, column %d:\nLast token: %s\n" line column lastToken
                  let error = new Error(message, new AST.Position(0,0,line,column))
                  let result = new Program([], new AST.Position(0,0,line,column))
                  result.AddError(error)
                  result
    ast   