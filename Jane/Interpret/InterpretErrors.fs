module InterpretErrors

open AST

type InterpretError(errorMessage : string, position : Position) =
    inherit Error(errorMessage, position)

    static member ArrayIndexOutOfRange (memb : Member) = 
        new InterpretError("Index out of range", memb.Position)

    static member nullReferenceExeption (obj : Expression) = 
        new InterpretError("null Reference Exeption", obj.Position)

    static member divisionByZero (expr : Expression) = 
        new InterpretError("division By Zero", expr.Position)