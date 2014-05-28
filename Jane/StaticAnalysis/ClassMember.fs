﻿module internal SA.ClassMember

open AST
open SA.Type
open SA.Expression
open SA.Dictionary

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

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

let SA_ClassMember (p : Program) (c : Class) (cm : ClassMember) =
    match cm with
    | :? ClassField as cf -> 
        let pos = new Position(0,0,0,0)
        let expectedTypes = 
            match cf.Type with
            | :? StringType  as st -> [st :> Type]
            | :? BooleanType as bt -> [bt :> Type]
            | :? CharType    as ct -> [ct :> Type]
            | :? IntType     as it -> [it :> Type; new FloatType(it.Dimension, pos) :> Type]
            | :? FloatType   as ft -> [ft :> Type; new IntType(ft.Dimension, pos) :> Type]
            | :? CustomType  as ct ->
                match ct.ClassOrInterface.Value with
                | :? Interface as i -> i.AllAncestors.ToListValues() 
                                       |> List.map (fun c -> new CustomType(c.Name.Value, ct.Dimension, pos) :> Type)
                                       |> fun l -> (List.Cons (cf.Type, l))
                | :? Class as c -> c.AllAncestors.ToListValues() 
                                   |> List.map (fun c -> new CustomType(c.Name.Value, ct.Dimension, pos) :> Type)
                                   |> fun l -> (List.Cons (cf.Type, l))
                | _  -> []
            | _ -> []
        let context = if cf.IsStatic then c.StaticContext else c.Context
        SA_Expression p cf.Body expectedTypes context

    | :? ClassConstructor as cc -> () // Заглушка
    | :? ClassMethod as cm      -> () // Заглушка
    | _                         -> () // Заглушка