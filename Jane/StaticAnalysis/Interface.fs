module internal SA.Interface

open AST
open SA.HelperFunctions
open SA.Errors
open SA.Dictionary

let SA_Interface (program : Program) (i : Interface) = 

    // Заполняет словари данными + Ошибки одинаковых названий
    let addToDicts (m : InterfaceMember) = 
        let error () = program.AddError <| Error.DuplicateNode "interface member" m.Name // Ошибка для случая повтора имени
        i.Members.TryAddWithAction m.Name.Value m error          
        match m with
        | :? InterfaceField        as f  -> i.Fields.TryAddWithInaction f.Name.Value f
        | :? InterfaceReturnMethod as rm -> i.Methods.TryAddWithInaction rm.Name.Value rm
                                            i.ReturnMethods.TryAddWithInaction rm.Name.Value rm
        | :? InterfaceVoidMethod   as vm -> i.Methods.TryAddWithInaction vm.Name.Value vm
                                            i.VoidMethods.TryAddWithInaction vm.Name.Value vm
        | _                              -> ()
    List.iter addToDicts i.MemberList
    
    // Ошибки корректности названий наследуемых интерфесов
    List.iter (fun (id : ID) -> IncorrectNameErrors program id.Value id.Position) i.AncestorNames       

    // Добавление ближайшего наследуемого интерфейса или добавление ошибки + ошибки дублирования и ошибки отсутствия
    let searchInterface (id : ID) = 
        let error() = program.AddError <| Error.DuplicateNode "extend interface" id // Ошибка повтора имени
        try i.NearestAncestors.TryAddWithAction id.Value program.Interfaces.[id.Value] error
        with :? KeyNotFound -> program.AddError <| Error.ObjectIsNotExist id.Value id.Position "Interface" // Отсутствия интерфейса

    // Добавление ближайших наследуемых интерфейсов
    List.iter searchInterface i.AncestorNames    