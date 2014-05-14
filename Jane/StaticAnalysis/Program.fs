module internal SA.Program

open AST
open SA.Errors
open SA.Dictionary
open SA.HelperFunctions
open SA.ProgramMember

// GD ~ Gathering Data
// SA ~ Static Analysis

let GD_Program(p : Program) =

    // Добавление в словари одного класса
    let addToDicts (pm : ProgramMember) =
        // Функция создающая ошибку повтора имени
        let errorCreator() = p.AddError <| Error.DuplicateNode "class" pm.Name 
        // Добавление в общий словарь классов и интерфейсов + ошибка повтора
        p.Members.TryAddWithAction pm.Name.Value pm errorCreator        
        // Добавление в нужный словарь
        match pm with
        | :? Class as c     -> p.Classes.TryAddWithInaction c.Name.Value c
        | :? Interface as i -> p.Interfaces.TryAddWithInaction i.Name.Value i
        | _                 -> ()

    // Заполняет словари данными
    List.iter addToDicts p.MemberList

    // Сбор данных на нижних уровнях для интефейсов
    p.MemberList
    |> List.filter (fun i -> i :? Interface)
    |> List.iter (GD_ProgramMember p)

    // Сбор данных на нижних уровнях для классов
    p.MemberList
    |> List.filter (fun i -> i :? Class)
    |> List.iter (GD_ProgramMember p)

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

let SA_Program(p : Program) = 

    // Ищет mainClass
    let mainClass = p.Classes.GetOption(p.NameMainClass)

    // Ошибка отстутсвия mainClass
    if mainClass.IsNone then p.AddError <| Error.MainClassNotFound p

    // Добавляет ссылку на mainClass в program
    p.MainClass <- mainClass

    // Если mainClass найден, то ищет в нём main + Ошибки отсутствия
    let mainMethod = match mainClass with
                     | None    -> None
                     | Some mc -> let main = mc.Methods.GetOption("main")
                                  // Ошибка отстутсвия main в mainClass
                                  if main.IsNone then p.AddError <| Error.MainNotFound mc
                                  main

    // Добавляет ссылку на метод main
    p.MainMethod <- mainMethod

    // Ошибки, связанные с main
    match mainMethod with      
    | Some mm -> 
        // main не void метод
        if not (mm :? ClassVoidMethod) then p.AddError <| Error.MainIsNotVoid mm
        // main не статический метод
        if not mm.IsStatic then p.AddError <| Error.MainIsNotStatic mm
        // main содержит параметры
        if mm.Parameters.Length > 0 then p.AddError <| Error.MainContaintsArgs mm
    | _ -> ()

    // Поиск ошибок в нижних уровнях
    List.iter (SA_ProgramMember p) p.MemberList

    // Добавление i в список, если голова списка <> i
    let addWithoutRepeats list (e : Error) =
        match list with
        | (h::_) when h.ToString() = e.ToString() -> list
        | _                                       -> e :: list

    // Удаление повторных ошибок
    p.Errors <- p.Errors
               |> List.sortBy (fun (e : Error) -> e.ToString())
               |> List.fold addWithoutRepeats []


