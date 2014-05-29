module internal SA.InterfaceMember

open AST
open SA.Type

// GD ~ Gathering Data
// SA ~ Static Analysis

let GD_InterfaceMember (p : Program) (c : Interface) (cm : InterfaceMember) =
    match cm with
    | :? InterfaceField  as if' -> GD_Type p if'.Type
    | :? InterfaceMethod as im  -> im.Parameters |> List.iter (fun par -> GD_Type p par.Type)
                                   match im with
                                   | :? InterfaceReturnMethod as irm -> GD_Type p irm.ReturnType 
                                   | _                           -> ()
    | _                         -> ()