module internal SA.ClassMember

open AST
open SA.Type

// GD ~ Gathering Data
// SA ~ Static Analysis

let GD_ClassMember (p : Program) (c : Class) (cm : ClassMember) =
    match cm with
    | :? ClassField as cf       -> GD_Type p cf.Type
    | :? ClassConstructor as cc -> cc.Parameters |> List.iter (fun par -> GD_Type p par.Type)
    | :? ClassMethod as cm      -> cm.Parameters |> List.iter (fun par -> GD_Type p par.Type)
                                   match cm with
                                   | :? ClassReturnMethod as crm -> GD_Type p crm.ReturnType 
                                   | _                           -> ()
    | _                         -> ()