module internal SA.Type

open AST
open SA.Errors

// GD ~ Gathering Data

let GD_Type (p : Program) (t : Type) =
   
    match t with
    | :? CustomType as ct ->
    
        // Класс или интерфейс (если таковой существует), на который должен ссылаться тип
        let classOrInterface = p.MemberList
                            |> List.tryFind (fun m -> m.Name.Value = ct.Name)

        // Если таковой класс или интерфейс не существуют, то ошибка
        if classOrInterface.IsNone then p.AddError <| Error.ObjectIsNotExist (new ID(ct.Name, ct.Position)) "Type"

        // Добавление ссылки на класс или интерфейс
        ct.ClassOrInterface <- classOrInterface |> Option.map (fun pm -> pm :> Node)

    | _ -> ()
