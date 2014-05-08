module internal SA.Class

open AST
open SA.Errors
open SA.Interface
open SA.Dictionary
open SA.HelperFunctions

let SA_Class (program : Program) (c : Class) = 

    // Заполняет словари данными + Ошибки одинаковых названий
    let addToDicts (m : ClassMember) = 
        let error () = program.AddError <| Error.DuplicateNode "class member" m.Name // Ошибка для случая повтора имени
        c.Members.TryAddWithAction m.Name.Value m error          
        match m with
        | :? ClassField        as f  -> c.Fields.TryAddWithInaction f.Name.Value f
        | :? ClassReturnMethod as rm -> c.Methods.TryAddWithInaction rm.Name.Value rm
                                        c.ReturnMethods.TryAddWithInaction rm.Name.Value rm
        | :? ClassVoidMethod   as vm -> c.Methods.TryAddWithInaction vm.Name.Value vm
                                        c.VoidMethods.TryAddWithInaction vm.Name.Value vm
        | _                          -> ()
    List.iter addToDicts c.MemberList