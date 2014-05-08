module SA.Program

open AST
open SA.Errors
open SA.Dictionary
open SA.HelperFunctions
open SA.ProgramMember

let SA_Program(program : Program) = 

    // Заполняет словари данными + Ошибки одинаковых названий
    let addToDicts (pm : ProgramMember) = 
        let error () = program.AddError <| Error.DuplicateNode "class" pm.Name // Ошибка повтора имени
        program.Members.TryAddWithAction pm.Name.Value pm error          
        match pm with
        | :? Class as c     -> program.Classes.TryAddWithInaction c.Name.Value c
        | :? Interface as i -> program.Interfaces.TryAddWithInaction i.Name.Value i
        | _                 -> ()
    List.iter addToDicts program.MemberList

    // Поиск ошибок в нижних уровнях
    List.iter (SA_ProgramMember program) program.MemberList

    // Ищет mainClass + Ошибка отсутствия
    let mainClass = program.Classes.GetOption(program.NameMainClass)

    // Добавляет ссылку на mainClass в program
    program.MainClass <- mainClass

    // Если mainClass найден, то ищет в нём main + Ошибка отсутствия
    let mainMethod =
        match mainClass with
        | Some mc -> mc.Methods.GetOption("main")
        | None    -> None

    // Добавляет ссылку на метод main
    program.MainMethod <- mainMethod  

    // Ошибки, связанные с main
    match mainClass with
    | None    -> program.AddError <| Error.MainClassNotFound program // Отсутствует main класс
    | Some mc -> match mainMethod with       
                    | None    -> program.AddError <| Error.MainNotFound mc // В main классе отсутствует метод main
                    | Some mm -> 
                        // main не void метод
                        if not (mm :? ClassVoidMethod) then program.AddError <| Error.MainIsNotVoid mm
                        // main не статический метод
                        if not mm.IsStatic then program.AddError <| Error.MainIsNotStatic mm
                        // main содержит параметры
                        if mm.Parameters.Length > 0 then program.AddError <| Error.MainContaintsArgs mm
