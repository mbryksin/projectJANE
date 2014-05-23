module internal SA.Class

open System
open System.Collections.Generic

open AST
open SA.Errors
open SA.Interface
open SA.Dictionary
open SA.HelperFunctions

// GD ~ Gathering Data
// SA ~ Static Analysis

let GD_Class (p : Program) (c : Class) = 

    // Ошибки корректности названий реализуемых интерфесов
    List.iter (fun (id : ID) -> IncorrectName p id "implements interface") c.ImplementsInterfacesName
    
    // Ошибка корректности назавания наследуемого класса
    Option.iter (fun (id : ID) -> IncorrectName p id "extends class") c.AncestorName

    // Ошибка существования наследуемого класса
    match c.AncestorName with
    | Some id when not <| p.Classes.ContainsKey(id.Value) -> p.AddError <| Error.ObjectIsNotExist id "Extend class"
    | _                                                   -> ()

    // Ошибки существования реализуемых интерфейсов    
    let errorCreator id = p.AddError <| Error.ObjectIsNotExist id "Implement interface"
    List.iter (fun (id : ID) -> if not <| p.Interfaces.ContainsKey(id.Value) then errorCreator id) c.ImplementsInterfacesName

    // Добавляет предков
    let rec addAllAncestor (id : ID) =
        
        // Предок, если существует и ещё не внесён
        let ancestorOption = 
            try  let ancestor = p.Classes.[id.Value]
                 c.AllAncestors.Add(id.Value, ancestor)
                 c.AllAncestorsClasses.Add(id.Value, ancestor)
                 Some ancestor
            with | :? ArgumentException -> None
                 | :? KeyNotFound       -> None

        // Если этот потомок новый, добавить всех его предков
        match ancestorOption with
        | Some ancestor -> Option.iter addAllAncestor ancestor.AncestorName
        | _             -> ()

    // Добавляем всех предков 
    Option.iter addAllAncestor c.AncestorName

    // Ближайшие реализуемые интерфейсы
    let nearestImplementsInterfaces =
         c.AllAncestorsClasses.ToListValues()
        |> List.collect (fun c -> c.ImplementsInterfacesName)
        |> (@) c.ImplementsInterfacesName
        |> List.map string
        |> List.map p.Interfaces.GetOption
        |> List.filter Option.isSome
        |> List.map Option.get

    // Все реализуемые интерфейсы
    let allImplementsInterfaces = 
        nearestImplementsInterfaces
        |> List.collect (fun i -> i.AllAncestors.ToListPairs())
        |> (@) (List.map (fun (i : Interface) -> i.Name.Value, i) nearestImplementsInterfaces)


    // Добавляет все реализуемые интерфейсы в словарь реализуемых интерфейсов
    c.AllImplementsInterfaces.TryAddListWithInaction allImplementsInterfaces

    // Все реализуемые члены программы (интерфейсы)
    let allImplementsProgramMembers = List.map (fun (k, v) -> k, v :> ProgramMember) allImplementsInterfaces

    // Добавляет все реализуемые интерфейсы в словарь предков
    c.AllAncestors.TryAddListWithInaction allImplementsProgramMembers

    // Добавление в словари одного метода/поля
    let addToDicts (typeMember : string) (m : ClassMember) = 
        // Функция создающая ошибку повтора имени
        let errorCreator() = p.AddError <| Error.DuplicateNode typeMember m.Name
        // Проверка корректности имени и добавление ошибки 
        IncorrectName p m.Name typeMember
        // Добавление в общий словарь методов и полей + ошибка повтора
        c.Members.TryAddWithAction m.Name.Value m errorCreator        
        // Добавление в нужные словари
        match m with
        | :? ClassField        as f  -> c.Fields.TryAddWithInaction f.Name.Value f
        | :? ClassReturnMethod as rm -> c.Methods.TryAddWithInaction rm.Name.Value rm
                                        c.ReturnMethods.TryAddWithInaction rm.Name.Value rm
        | :? ClassVoidMethod   as vm -> c.Methods.TryAddWithInaction vm.Name.Value vm
                                        c.VoidMethods.TryAddWithInaction vm.Name.Value vm
        | _                          -> ()  
    
    // Добавляет методы всех предков
    c.AllAncestorsClasses.ToListValues()
    |> List.collect (fun c -> c.OwnMembers.ToListValues())
    |> List.iter (addToDicts "member of extend class")

    // Добавляет методы класса
    List.iter (addToDicts "class member") c.MemberList  

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

let SA_Class (p : Program) (c : Class) = () // Заглушка