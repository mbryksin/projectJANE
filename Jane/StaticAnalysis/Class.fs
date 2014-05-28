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

    // Добавляет предка
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

        // Добавление ошибки дублирования 
        if typeMember <> "" then
            let name = m.Name.Value;
            if c.Members.ContainsKey(name) then
                let oldMember = c.Members.[name]
                match oldMember with
                | :? ClassField as oldF when (m :? ClassField) ->
                    let newF = m :?> ClassField
                    if oldF.Type <> newF.Type then errorCreator()
                | :? ClassMethod as oldM when (m :? ClassMethod) -> 
                    let newM = m :?> ClassMethod
                    let oldParameters = List.map (fun (p : FormalParameter) -> p.Type) oldM.Parameters
                    let newParameters = List.map (fun (p : FormalParameter) -> p.Type) newM.Parameters
                    if oldM.IsStatic && newM.IsStatic && oldParameters.Length = newParameters.Length && List.forall2 (=) oldParameters newParameters then                                                       
                        match oldM with
                        | :? ClassVoidMethod when (newM :? ClassVoidMethod) -> ()
                        | :? ClassReturnMethod as oldRM when (newM :? ClassReturnMethod) ->
                            let newRM = newM :?> ClassReturnMethod
                            if newRM.ReturnType <> oldRM.ReturnType then errorCreator()
                        | _ -> errorCreator()
                    else errorCreator()
                | _ -> errorCreator()

        // Проверка корректности имени и добавление ошибки 
        IncorrectName p m.Name typeMember
        // Добавление в общий словарь методов и полей
        c.Members.AddOrUpdate m.Name.Value m        
        // Добавление в нужные словари
        match m with
        | :? ClassField        as f  -> c.Fields.AddOrUpdate f.Name.Value f
        | :? ClassReturnMethod as rm -> c.Methods.AddOrUpdate rm.Name.Value rm
                                        c.ReturnMethods.AddOrUpdate rm.Name.Value rm
        | :? ClassVoidMethod   as vm -> c.Methods.AddOrUpdate vm.Name.Value vm
                                        c.VoidMethods.AddOrUpdate vm.Name.Value vm
        | _                          -> ()  
    
    // Добавляет методы всех предков
    c.AllAncestorsClasses.ToListValues()
    |> List.rev
    |> List.collect (fun c -> c.OwnMembers.ToListValues())
    |> List.iter (addToDicts "")

    // Добавляет методы класса
    List.iter (addToDicts "class member") c.MemberList
    
    // Все поля класса
    let fieldList = c.Fields.ToListValues();
    
    // Собирает контекст для методов
    c.Context <- fieldList 
              |> List.map (fun f -> new Variable(f.Name.Value, f.Type, Val.Null, IsFinal = f.IsFinal)) 
    
    // Собирает контекст для статический методов          
    c.StaticContext <- fieldList
                    |> List.filter (fun f -> f.IsStatic)
                    |> List.map (fun f -> new Variable(f.Name.Value, f.Type, Val.Null, IsFinal = f.IsFinal))

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

let SA_Class (p : Program) (c : Class) =

    // Проверяет, реализует ли класс метод интерфейса
    let ImplementMember (i : Interface) (im : InterfaceMember) =
        
        let errorCreator() = p.AddError <| Error.InterfaceMemberIsNotImplemented im i c
        let name = im.Name.Value

        if not <| c.OwnMembers.ContainsKey(name) then errorCreator()
        else let cm = c.OwnMembers.[name]
             match im with
             | :? InterfaceField as iField when (cm :? ClassField) -> 
                let cField = cm :?> ClassField
                if iField.Type <> cField.Type then errorCreator()
             | :? InterfaceMethod as im when (cm :? ClassMethod) ->
                let cm = cm :?> ClassMethod
                let imParameters = List.map (fun (p : FormalParameter) -> p.Type) im.Parameters
                let cmParameters = List.map (fun (p : FormalParameter) -> p.Type) cm.Parameters
                if cm.IsStatic = im.IsStatic && imParameters.Length = cmParameters.Length && List.forall2 (=) imParameters cmParameters then                                                       
                    match im with
                    | :? InterfaceVoidMethod when (cm :? ClassVoidMethod) -> ()
                    | :? InterfaceReturnMethod as irm when (cm :? ClassReturnMethod) ->
                        let crm = cm :?> ClassReturnMethod
                        if crm.ReturnType <> irm.ReturnType then errorCreator()
                    | _ -> errorCreator()
                else errorCreator()
             | _ -> errorCreator()

    // Проверяет, реализует ли класс весь интерфейс
    let ImplementInterface (i : Interface) =
        i.MemberList
        |> List.iter (ImplementMember i)

    // Проверяет, реализует ли класс все свои интерфейсы (с учётом предков интерфейсов)
    c.AllImplementsInterfaces.ToListValues()
    |> List.iter ImplementInterface

    
