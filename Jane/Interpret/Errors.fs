module Errors

open AST

exception ArrayOutOfIndex of string * Position
exception NullReferenceExeption of string * Position
exception DivisionByZero of string * Position
