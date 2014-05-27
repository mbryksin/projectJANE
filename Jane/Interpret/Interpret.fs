module Interpret

open ServiceFunction
open ExpressionFunctions
open ArrayFunctions
open InterpretErrors
open JaneRuntime

open AST

let mutable (currentProgram : Program option) = None

//***************************************************Program***************************************************//

//Find mainClass, find mainMethod and interpret it
let rec interpretProgram (program: Program) =
    let mainMethod = program.MainMethod.Value
    currentProgram <- Some program
    let valInterpret = interpretMethod mainMethod []
    match valInterpret with
    | Err (message, pos) -> 
        let err = Error(message, pos)
        currentProgram.Value.Errors <- err :: currentProgram.Value.Errors
        err.ErrorMessage
    | _ -> currentProgram.Value.ReturnString
    

//***************************************************ClassMembers***************************************************//

and interpretMethod (classMethod : ClassMethod) (args : Val list) = 
    let body = classMethod.Body
    let parameters = classMethod.Parameters
    //add arguments to method body context
    addArgsToMethodContext args parameters body    
    let statementVal = interpretStatement (classMethod.Body :> Statement)
    match statementVal with
    | Return value -> value
    | Err(_,_)     -> statementVal
    | _            -> Empty


and interpretClassConstructor (constr : ClassConstructor) interpretArgs context =
    let name = constr.Name.Value
    let classConstructorBody = constr.Body
    let classforObject = currentProgram.Value.Classes.[name]
    let Fields = classforObject.Fields
    classConstructorBody.Context <- [] //clearOldContext
    
    //Fields -> Variables for Object
    let fieldsAsVar = 
        Seq.map (fun (f : ClassField) -> 
                      let varName = f.Name.Value
                      let varType = f.Type
                      let varVal = interpretExpression f.Body context
                      let var = new Variable(varName, varType, varVal)
                      var
                      ) Fields.Values 
        |> Seq.toList
    classConstructorBody.Context <- fieldsAsVar @ classConstructorBody.Context

    //Interpret constructor block and change fields for making object
    let parametersConstructor = constr.Parameters;
    addArgsToMethodContext interpretArgs parametersConstructor classConstructorBody
    let valBlock = interpretBlock classConstructorBody
    match valBlock with
    | Err(_,_) -> valBlock
    | _ -> Object (fieldsAsVar, name)
    

//***************************************************Statements***************************************************//

and interpretStatement(statement : Statement) =
    match statement with
        | :? Block                as bl  -> interpretBlock bl
        | :? DeclarationStatement as ds  -> interpretDeclarationStatement ds
        | :? AssignmentStatement  as As  -> interpretAssignmentStatement  As
        | :? MemberCallStatement  as mc  -> interpretMemberCallStatement mc
        | :? IfStatement          as is  -> interpretIfStatement is
        | :? WhileStatement       as ws  -> interpretWhileStatement ws
        | :? ForStatement         as fs  -> interpretForStatement fs
        | :? ContinueStatement    as cs  -> interpretContinueStatement cs
        | :? BreakStatement       as bs  -> interpretBreakStatement bs
        | :? ReturnStatement      as rs  -> interpretReturnStatement rs
        | :? SuperStatement       as ss  -> interpretSuperStatement ss
        | _ -> Empty

//Interpret all statements in block
and interpretBlock (block : Block) =
    let statements = block.Statements
    let rec statementsInterpret (statements : Statement list) = 
        match statements with
        | currStatement :: statements ->
             //add block to statement parent
             currStatement.Parent <- Some block
             match currStatement with
             | :? Block as bl -> 
                    bl.Context <- block.Context
                    interpretBlock bl         
             | :? ReturnStatement as rs -> interpretReturnStatement rs             
             | _ -> let valueInterpret = interpretStatement currStatement 
                    match valueInterpret with
                    | Return v -> valueInterpret
                    | Err (_,_) -> valueInterpret
                    | Continue  -> valueInterpret
                    | _        -> statementsInterpret statements
        | [] -> Empty
    statementsInterpret statements

//If condition is true, interpret trueStatement else interpret elseStatement, if it possible
and interpretIfStatement (ifStatement : IfStatement) =
    let context = ifStatement.Parent.Value.Context
    let condition = ifStatement.Condition
    let parent = ifStatement.Parent
    let trueStatement = ifStatement.TrueStatement
    let falseStatement = ifStatement.FalseStatement

    if isTrueCondition(interpretExpression condition context) = true
        then trueStatement.Parent <- parent
             //add context if statement is block
             addToBlockContext trueStatement context
             interpretStatement trueStatement 
        else 
             if falseStatement.IsSome 
                 then falseStatement.Value.Parent <- parent
                      //add context if statement is block
                      addToBlockContext falseStatement.Value context
                      interpretStatement falseStatement.Value 
                 else Empty

//While condition is true, interpret body
and interpretWhileStatement (whileStatement : WhileStatement) = 
    let context = whileStatement.Parent.Value.Context
    let body = whileStatement.Body
    let parent = whileStatement.Parent
    let condition = whileStatement.Condition
    body.Parent <- parent

    //add context if statement is block
    addToBlockContext body context

    let rec startWhile () =
        let isTrueCond = isTrueCondition(interpretExpression condition context)
        match isTrueCond with
        | true  -> 
            let bodyVal = interpretStatement body
            match bodyVal with
            | Return v -> bodyVal
            | Err(_,_) -> bodyVal
            | _        -> startWhile ()   //Continue included
        | false -> Empty
    startWhile ()

//interpret init. if condition is true, interpret body and update
and interpretForStatement (forstatement : ForStatement) =   
    let init = forstatement.Init
    let update = forstatement.Update
    let parent = forstatement.Parent
    let body = forstatement.Body
    let condition = forstatement.Condition
    init.Parent <- parent
    update.Parent <- parent
    body.Parent <- parent
    //Add init var to context
    interpretDeclarationStatement init |> ignore 
    let context = forstatement.Parent.Value.Context
    //add context if statement is block
    addToBlockContext body context

    let rec startFor () =
        let isTrueCond = isTrueCondition(interpretExpression condition context)
        match isTrueCond with
        | true  -> 
            let bodyVal = interpretStatement body
            match bodyVal with
            | Return v -> bodyVal
            | Err(_,_) -> bodyVal
            | _        -> interpretAssignmentStatement update |> ignore
                          startFor ()
        | false -> Empty
    startFor ()

and interpretSuperStatement (ss : SuperStatement) = Empty             //DO IT

// Interpret expression and return it
and interpretReturnStatement (rs : ReturnStatement) = 
    let expressionReturn = rs.Expression.Value
    let context = rs.Parent.Value.Context
    let returnValue = interpretExpression expressionReturn context
    match returnValue with
    | Err(_,_) -> returnValue
    | _ -> Return returnValue

and interpretBreakStatement (bs : BreakStatement) = Return Empty   

and interpretContinueStatement (cs : ContinueStatement) = Continue

// Interpret body of method
and interpretMemberCallStatement (mc : MemberCallStatement) = 
    let parentContext = mc.Parent.Value.Context
    let expr = mc.Body
    interpretExpression expr parentContext

// Find variable in context and assign current value
and interpretAssignmentStatement (assign : AssignmentStatement) = 
    let parentBlockContext = assign.Parent.Value.Context
    let AssignVarName = assign.Name.Value             //using for simply imperative tests
    let currentVariable = List.find (fun (v: Variable) -> v.Name = AssignVarName) parentBlockContext
    let valOfInitializer = interpretInitializer assign.Body parentBlockContext
    match valOfInitializer with
    | Err(_,_) -> valOfInitializer
    | _ -> 
        printfn "%s" <| "beforeAssign " + currentVariable.ToString() //for debugging
        currentVariable.Assign(valOfInitializer)
        printfn "%s" <| "AfterAssign " + currentVariable.ToString()  //for debugging
        Empty

//Add variable in context
and interpretDeclarationStatement (declaration : DeclarationStatement) =
    let parentBlock = declaration.Parent.Value
    let context = declaration.Parent.Value.Context
    let declVarName = declaration.Name.Value
    let declVarType = declaration.Type
    let declVarValue = interpretInitializer declaration.Body context
    match declVarValue with
    | Err(_,_) -> declVarValue
    | _ -> 
        parentBlock.Context <- Variable(declVarName, declVarType, declVarValue) :: parentBlock.Context
        printfn "%s" <|  "declaration " + parentBlock.Context.Head.ToString() //for debugging
        Empty

//***************************************************Initializers***************************************************//

and interpretInitializer (initializer : Initializer) (context : Variable list) =
    match initializer with      
        | :? ArrayInitializer     as ai  -> interpretArrayInitializer ai context
        | :? Expression           as ex  -> interpretExpression ex context
        | _                              -> Empty

//Create Val: Array of elements of initialize
and interpretArrayInitializer (arrayinit: ArrayInitializer) (context : Variable list) =
    let elemsVal = List.map (fun (i : Initializer) -> interpretInitializer i context) arrayinit.Elements 
    Array (List.toArray elemsVal)

//***************************************************Expressions***************************************************//

and interpretExpression (expression : Expression) (context : Variable list) =
    match expression with
        | :? Primary         as primary  -> interpretPrimary primary context
        | :? InstanceOf      as instance -> interpretInstanceOf instance context
        | :? BinaryOperation as binOp    -> interpretBinaryOperation binOp context
        | :? UnaryOperation  as unOp     -> interpretUnaryOperation unOp  context
        | _                              -> Empty

and interpretInstanceOf (instance: InstanceOf) (context : Variable list) = Empty

and interpretUnaryOperation (unOp: UnaryOperation) (context : Variable list) =
    let operandVal = interpretExpression unOp.Operand context 
    match unOp.Operator with
    | NOT              -> notLogical operandVal
    | MINUS            -> minus operandVal

and interpretBinaryOperation (binOp: BinaryOperation) (context : Variable list) =
    let firstOpetandVal  = interpretExpression binOp.FirstOperand  context
    let secondOpetandVal = interpretExpression binOp.SecondOperand context
    let position = binOp.Position

    match binOp.Operator with
    | OR               -> logicalOperation (firstOpetandVal, secondOpetandVal, (||))
    | AND              -> logicalOperation (firstOpetandVal, secondOpetandVal, (&&))
    | EQUAL            -> compareOperation (firstOpetandVal, secondOpetandVal, (=))
    | NOT_EQUAL        -> compareOperation (firstOpetandVal, secondOpetandVal, (<>))
    | GREATER          -> compareOperation (firstOpetandVal, secondOpetandVal, (>))
    | GREATER_OR_EQUAL -> compareOperation (firstOpetandVal, secondOpetandVal, (>=))
    | LESS             -> compareOperation (firstOpetandVal, secondOpetandVal, (<))
    | LESS_OR_EQUAL    -> compareOperation (firstOpetandVal, secondOpetandVal, (<=))
    | ADDITION         -> addition firstOpetandVal secondOpetandVal
    | SUBSTRACTION     -> substraction firstOpetandVal secondOpetandVal
    | MULTIPLICATION   -> multyplication firstOpetandVal secondOpetandVal
    | DIVISION         -> division firstOpetandVal secondOpetandVal position
    | MODULUS          -> modul firstOpetandVal secondOpetandVal 
    | MEMBER_CALL      -> memberCall firstOpetandVal secondOpetandVal context position

//*********************************************Member Call******************************************//
and memberCall objectOrClass classMember context position = 
    match objectOrClass, classMember with
    | ClassOrField className, MethodVal (methodName, args) ->
        staticMethodCall className methodName args context
    | Object (fields, className), MethodVal (methodName, args) ->
        methodCall fields className methodName args
    | Object (fields, className), ClassOrField fieldName ->
        fieldGet fields className fieldName
    | ClassOrField className, ClassOrField fieldName ->
        staticFieldGet className fieldName context
    | Null, _ -> Err ("Null referense Exeption", position)
    | _, _ -> Empty
    
and staticMethodCall className methodName args context =
    let progListClasses = currentProgram.Value.Classes
    match className, methodName with
        // костыль, жду пока Саша нормальный Console.Writeline сделает
        | "Console", "writeLine" -> currentProgram.Value.ReturnString <- writeValue args.Head
                                    Empty
        | libraryClassName, _ when not <| progListClasses.ContainsKey(libraryClassName) ->
            Runtime.callStaticMethod (className, methodName, args)          
        | _ ->
            let currClass = progListClasses.[className]
            let methods = currClass.Methods
            let currMethod = methods.[methodName]
            interpretMethod currMethod args

and methodCall fields className methodName args =
    let progListClasses = currentProgram.Value.Classes
    let currClass = progListClasses.[className]
    let methods = currClass.Methods
    let currMethod = methods.[methodName]
    currMethod.Body.Context <- []
    currMethod.Body.Context <- fields @ currMethod.Body.Context 
    interpretMethod currMethod args

and staticFieldGet className fieldName context =
    let progListClasses = currentProgram.Value.Classes
    let currClass = progListClasses.[className]
    let field = currClass.Fields.[fieldName]
    interpretExpression field.Body context

and fieldGet fields className fieldName =
    let field = List.find (fun (f : Variable) -> f.Name = fieldName) fields
    field.Val

//***************************************************Primary***************************************************//

and interpretPrimary (primary : Primary) (context : Variable list) =
    match primary with
        | :? Literal     as lit   -> interpretLiteral lit context
        | :? Constructor as cons  -> interpretConstructor cons context
        | :? Identifier  as ident -> interpretIdentifier ident context
        | :? Member      as memb  -> interpretMember memb context
        | _                       -> Empty


and interpretConstructor (cons : Constructor) (context : Variable list) =
    let args = cons.Arguments.Arguments
    let name = cons.Name.Value
    let interpretArgs = List.map (fun (expr : Expression) -> interpretExpression expr context) args
    let classforObject = currentProgram.Value.Classes.[name]

    let classConstructor = classforObject.Constructor;
    interpretClassConstructor classConstructor interpretArgs context

    
//find var in context and return value
and interpretIdentifier (ident : Identifier ) (context : Variable list) =
    let classes = currentProgram.Value.Classes
    let IdName = ident.Name.Value   
    let currVar = List.tryFind (fun (var: Variable) -> var.Name = IdName) context
    let currClass = classes.ContainsKey(IdName)
    match currVar with 
    | Some var -> var.Val
    | None     -> ClassOrField IdName //for class and fields


and interpretMember (memb : Member) (context : Variable list) =
    let memberName = memb.Name.Value
    let MemberVal = 
        match memb.Suffix with
        //Достаем элемент по индексу из массива
        | :? ArrayElement  as arrayElem -> 
            let currentVariable = List.find (fun (v: Variable) -> v.Name = memberName) context
            let ValOfIndex =  interpretExpression arrayElem.Index context
            getValueOfIndex currentVariable ValOfIndex memb.Position    
        //Получаем как значение MethodVal с аргументами и именем
        | :? Arguments     as args      -> 
                let arguments = List.map (fun (a : Expression) -> interpretExpression a context) args.Arguments
                MethodVal (memberName, arguments)
        | _ -> Empty
    MemberVal

//***************************************************Literals***************************************************//

and interpretLiteral (literal : Literal) (context : Variable list) =
    match literal with
        | :? NullLiteral    as nullLiteral   -> Null
        | :? CharLiteral    as charLiteral   -> Char charLiteral.Get
        | :? IntegerLiteral as intLiteral    -> Int intLiteral.Get
        | :? BooleanLiteral as boolLiteral   -> Bool boolLiteral.Get
        | :? StringLiteral  as stringLiteral -> Str stringLiteral.Get
        | :? FloatLiteral   as floatLiteral  -> Float floatLiteral.Get
        | _                                  -> Empty