module internal SA.ClassConstructor

open AST
open SA.Errors
open SA.Statement

// GD ~ Gathering Data
// SA ~ Static Analysis

let SA_ClassConstructor (p : Program) (c : Class) (cc : ClassConstructor) =
    
    // Собирает контекст
    let context1 =  c.Context
    let context2 = cc.Parameters |> List.map (fun p -> new Variable(p.Name.Value, p.Type, Null))

    // Ищет переменные с одинаковыми именами
    cc.Parameters
    |> List.iter (fun par -> if context1 |> List.exists(fun v -> par.Name.Value = v.Name) then p.AddError <| Error.DuplicateNode "variable" par.Name)

    // Добавляет контекст
    cc.Body.Context <- context1 @ context2
    SA_Statement p c cc.Body

    