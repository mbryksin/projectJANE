module internal SA.ClassMethod

open AST
open SA.Statement
open SA.Errors

// GD ~ Gathering Data
// SA ~ Static Analysis

let SA_ClassMethod (p : Program) (c : Class) (cm : ClassMethod) =
    
    // Собирает контекст
    let context1 = if cm.IsStatic then c.StaticContext else c.Context
    let context2 = cm.Parameters |> List.map (fun p -> new Variable(p.Name.Value, p.Type, Null))

    // Ищет переменные с одинаковыми именами
    cm.Parameters
    |> List.iter (fun par -> if context1 |> List.exists(fun v -> par.Name.Value = v.Name) then p.AddError <| Error.DuplicateNode "variable" par.Name)

    // Добавляет контекст
    cm.Body.Context <- context1 @ context2
    SA_Statement p c cm.Body

