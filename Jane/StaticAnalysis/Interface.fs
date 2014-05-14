module internal SA.Interface

open System

open AST
open SA.HelperFunctions
open SA.Errors
open SA.Dictionary

// GD ~ Gathering Data
// SA ~ Static Analysis

let GD_Interface (p : Program) (i : Interface) = 

    // Добавление в словари одного метода/поля
    let addToDicts (m : InterfaceMember) = 
        // Функция создающая ошибку повтора имени
        let errorCreator () = p.AddError <| Error.DuplicateNode "interface member" m.Name
        // Проверка корректности имени и добавление ошибки 
        IncorrectName p m.Name "interface member"
        // Добавление в общий словарь методов и полей + ошибка повтора
        i.Members.TryAddWithAction m.Name.Value m errorCreator          
        // Добавление в нужные словари
        match m with
        | :? InterfaceField        as f  -> i.Fields.TryAddWithInaction f.Name.Value f
        | :? InterfaceReturnMethod as rm -> i.Methods.TryAddWithInaction rm.Name.Value rm
                                            i.ReturnMethods.TryAddWithInaction rm.Name.Value rm
        | :? InterfaceVoidMethod   as vm -> i.Methods.TryAddWithInaction vm.Name.Value vm
                                            i.VoidMethods.TryAddWithInaction vm.Name.Value vm
        | _                              -> ()
    
    // Заполняет словари данными
    List.iter addToDicts i.MemberList

    // Ошибки корректности названий наследуемых интерфесов
    List.iter (fun (id : ID) -> IncorrectName p id "extends interface") i.AncestorNames   
    
    // Ошибки существования наследуемых интерфейсов    
    let errorCreator id = p.AddError <| Error.ObjectIsNotExist id "Extend interface"
    List.iter (fun (id : ID) -> if not <| p.Interfaces.ContainsKey(id.Value) then errorCreator id) i.AncestorNames

    // Добавляет предков
    let rec addAllAncestor (id : ID) =
        
        // Предок, если существует и ещё не внесён
        let ancestorOption = 
            try  let ancestor = p.Interfaces.[id.Value]
                 i.AllAncestors.Add(id.Value, ancestor)
                 Some ancestor
            with | :? ArgumentException -> None
                 | :? KeyNotFound       -> None

        // Если этот потомок новый, добавить всех его предков
        match ancestorOption with
        | Some ancestor -> List.iter addAllAncestor ancestor.AncestorNames
        | _             -> ()

    // Добавляем всех предков 
    addAllAncestor i.Name
    
    // Интерфейс сам себе не предок
    ignore <| i.AllAncestors.Remove(i.Name.Value)
    ()


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

let SA_Interface (program : Program) (i : Interface) = () // Нечего анализировать