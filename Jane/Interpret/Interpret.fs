module Interpret

open AST
open StaticAnalysis
open Errors

let mutable (currentProgram : Program option) = None

//***************************************************ServiceFunction***************************************************//
let isTrueCondition (cond : Val) =
        match cond with
        | Bool true -> true
        | Int n when n <> 0L -> true
        | _ -> false

//take name and Type of Variable from Parameters and Value from Arguments
let rec addArgsToMethodContent (arguments : Val list) (parameters : FormalParameter list) (body : Block) =
    match arguments, parameters with
    | a :: args, p :: ps -> 
        let arg = arguments.Head
        let param = parameters.Head
        
        let VarName = param.Name.Value
        let VarType = param.Type
        let VarVal  = arg
        body.Context <- Variable(VarName, VarType, VarVal) :: body.Context
        addArgsToMethodContent arguments.Tail parameters.Tail body
    | _ -> () 
//***************************************************Program***************************************************//
//Find mainClass, find mainMethod and interpret it
let rec interpretProgram(program: Program) =
    let classWithMain = program.Classes.Head
    let mainMethod = classWithMain.VoidMethods.Head
    currentProgram <- Some program
    interpretClassVoidMethod mainMethod [] |> ignore

//***************************************************ClassMembers***************************************************//

and interpretClassConstructor (cons : ClassConstructor) = ()

and interpretClassField (field : ClassField) = ()

and interpretClassReturnMethod (returnmethod : ClassReturnMethod) (args : Val list) = 
    let body = returnmethod.Body
    let parameters = returnmethod.Parameters
    addArgsToMethodContent args parameters body
    
    //here is returnValue
    interpretStatement (returnmethod.Body :> Statement)

and interpretClassVoidMethod (voidmethod : ClassVoidMethod) (args : Val list) = 
    let body = voidmethod.Body
    let parameters = voidmethod.Parameters
    addArgsToMethodContent args parameters body
    interpretStatement (voidmethod.Body :> Statement)

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
    let rec statementsInterpret (stats : Statement list) = 
        match stats with
        | s :: ss ->
             s.Parent <- Some block
             match s with
                      | :? Block           as bl -> bl.Context <- block.Context
                                                    interpretStatement s |> ignore
                                                    statementsInterpret ss
                      | :? ReturnStatement as rs -> interpretReturnStatement rs                              
                      | _ -> interpretStatement s |> ignore
                             statementsInterpret ss
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
             match trueStatement with
             | :? Block as block -> block.Context <- context
             | _ -> ()
             interpretStatement trueStatement  |> ignore     
        else 
             if falseStatement.IsSome 
             then falseStatement.Value.Parent <- parent
                  match falseStatement.Value with
                  | :? Block as block -> block.Context <- context
                  | _ -> ()
                  interpretStatement falseStatement.Value |> ignore
    Empty

//While condition is true, interpret body
and interpretWhileStatement (whileStatement : WhileStatement) = 
    let context = whileStatement.Parent.Value.Context
    let body = whileStatement.Body
    let parent = whileStatement.Parent
    let condition = whileStatement.Condition
    body.Parent <- parent

    match whileStatement.Body with
    | :? Block as block -> block.Context <- context
    | _ -> ()

    while isTrueCondition(interpretExpression condition context) do
        interpretStatement body |> ignore
    Empty

//interpret init. if condition is true, interpret body and update
and interpretForStatement (forstatement : ForStatement) =   
    let context = forstatement.Parent.Value.Context
    let init = forstatement.Init
    let update = forstatement.Update
    let parent = forstatement.Parent
    let body = forstatement.Body
    let condition = forstatement.Condition
    init.Parent <- parent
    update.Parent <- parent

    interpretDeclarationStatement init |> ignore
    body.Parent <- parent
    match body with
    | :? Block as block -> block.Context <- context
    | _ -> ()    

    while isTrueCondition(interpretExpression condition context) do
        interpretStatement body |> ignore
        interpretAssignmentStatement update |> ignore
    Empty

//to do
and interpretSuperStatement (ss : SuperStatement) = Empty

and interpretReturnStatement (rs : ReturnStatement) = 
    let expressionReturn = rs.Expression.Value
    let context = rs.Parent.Value.Context
    interpretExpression expressionReturn context

and interpretBreakStatement (bs : BreakStatement) = Empty

and interpretContinueStatement (cs : ContinueStatement) = Empty

// Interpret body of method
and interpretMemberCallStatement (mc : MemberCallStatement) = 
    let parentContext = mc.Parent.Value.Context
    let expr = mc.Body
    interpretExpression expr parentContext

// Find variable in context and assign current value
and interpretAssignmentStatement (assign : AssignmentStatement) = 
    let parentBlockContext = assign.Parent.Value.Context
    let AssignVarName = assign.Name.Value             //using for simply imperative tests
    let valOfInitializer = interpretInitializer assign.Body parentBlockContext
    let currentVariable = List.find (fun (v: Variable) -> v.Name = AssignVarName) parentBlockContext
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


and interpretBinaryOperation (binOp: BinaryOperation) (context : Variable list) =

    let compare (operand1 : Val, operand2: Val, op : System.IComparable -> System.IComparable -> bool) = 
        match operand1, operand2 with
        | Bool log1, Bool log2     -> Bool((op) log1 log2)
        | Int num1, Int num2       -> Bool((op) num1 num2)
        | Float num1, Float num2   -> Bool((op) num1 num2)
        | Char char1, Char char2   -> Bool((op) char1 char2)
        | Str str1, Str str2 -> Bool((op) str1 str2)
        | _                        -> Empty

    let logical (operand1 : Val, operand2: Val, op : bool -> bool -> bool) = 
        match operand1, operand2 with
        | Bool log1, Bool log2     -> Bool((op) log1 log2)
        | _                        -> Empty

    let firstOpetandVal  = interpretExpression binOp.FirstOperand  context
    let secondOpetandVal = interpretExpression binOp.SecondOperand context

    match binOp.Operator with
    | OR               -> logical (firstOpetandVal, secondOpetandVal, (||))
    | AND              -> logical (firstOpetandVal, secondOpetandVal, (&&))
    | EQUAL            -> compare (firstOpetandVal, secondOpetandVal, (=))
    | NOT_EQUAL        -> compare (firstOpetandVal, secondOpetandVal, (<>))
    | GREATER          -> compare (firstOpetandVal, secondOpetandVal, (>))
    | GREATER_OR_EQUAL -> compare (firstOpetandVal, secondOpetandVal, (>=))
    | LESS             -> compare (firstOpetandVal, secondOpetandVal, (<))
    | LESS_OR_EQUAL    -> compare (firstOpetandVal, secondOpetandVal, (<=))

    | ADDITION         -> match firstOpetandVal, secondOpetandVal with
                            | Int intnum1, Int intnum2         -> Int(intnum1 + intnum2)
                            | Float floatnum1, Float floatnum2 -> Float(floatnum1 + floatnum2)
                            | Str str1, Str str2               -> Str (str1 + str2)
                            | _                                -> Empty

    | SUBSTRACTION     -> match firstOpetandVal, secondOpetandVal with
                            | Int intnum1, Int intnum2         -> Int(intnum1 - intnum2)
                            | Float floatnum1, Float floatnum2 -> Float(floatnum1 - floatnum2)
                            | _                                -> Empty

    | MULTIPLICATION   -> match firstOpetandVal, secondOpetandVal with
                            | Int intnum1, Int intnum2         -> Int(intnum1 * intnum2)
                            | Float floatnum1, Float floatnum2 -> Float(floatnum1 * floatnum2)
                            | _                                -> Empty

    | DIVISION         -> match firstOpetandVal, secondOpetandVal with
                            | Int intnum1, Int intnum2         -> Int(intnum1 / intnum2)
                            | Float floatnum1, Float floatnum2 -> Float(floatnum1 / floatnum2)
                            | _                                -> Empty

    | MODULUS          -> match firstOpetandVal, secondOpetandVal with
                            | Int intnum1, Int intnum2         -> Int(intnum1 % intnum2)
                            | Float floatnum1, Float floatnum2 -> Float(floatnum1 % floatnum2)
                            | _                                -> Empty

    | MEMBER_CALL      -> match firstOpetandVal, secondOpetandVal with
                          | ClassVal className, MethodVal (methodName, elems) ->
                              let progListClasses = currentProgram.Value.Classes
                              let currClass = List.find (fun (currClass: Class) -> currClass.Name.Value = className) progListClasses
                              let methods = currClass.Methods
                              let currMethod = List.find (fun (currMethod: ClassMethod) -> currMethod.Name.Value = methodName) methods
                              let args = elems
                              match currMethod with
                              | :? ClassReturnMethod as returnMethod -> interpretClassReturnMethod returnMethod args
                              | :? ClassVoidMethod as voidMethod     -> interpretClassVoidMethod voidMethod args
                              | _                                    -> Empty

                          | _, _ -> Empty


and interpretUnaryOperation (unOp: UnaryOperation) (context : Variable list) =
    let operandVal = interpretExpression unOp.Operand context 
    match unOp.Operator with
    | NOT              -> match operandVal with
                          | Bool log -> Bool (not log)
                          | _        -> Empty // error

    | MINUS            -> match operandVal with
                          | Int   number -> Int(- number)
                          | Float number -> Float(-number)
                          | _        -> Empty // error 



//***************************************************Primary***************************************************//

and interpretPrimary (primary : Primary) (context : Variable list) =
    match primary with
        | :? Literal     as lit   -> interpretLiteral lit context
        | :? Constructor as cons  -> interpretConstructor cons context
        | :? Identifier  as ident -> interpretIdentifier ident context
        | :? Member      as memb  -> interpretMember memb context
        | _                       -> Empty


and interpretConstructor (cons : Constructor) (context : Variable list) = Empty

//find var in context and return value
and interpretIdentifier (ident : Identifier ) (context : Variable list) =
    let IdName = ident.Name.Value   
    let currVar = List.tryFind (fun (var: Variable) -> var.Name = IdName) context
    match currVar with 
    | Some var -> var.Val
    | None     -> ClassVal IdName //for class and members


and interpretMember (memb : Member) (context : Variable list) =
    let memberName = memb.Name.Value
    let currentVariable = List.find (fun (v: Variable) -> v.Name = memberName) context

    let ValOfIndex = 
        match memb.Suffix with
        | :? ArrayElement  as arrayElem -> interpretExpression arrayElem.Index context
        | :? Arguments     as args      -> 
                                        let arguments = List.map (fun (a : Expression) -> interpretExpression a context) args.Arguments
                                        MethodVal (memberName, arguments)
        | _ -> Empty

    let IntValOfIndex = 
        match ValOfIndex with
        | Int num -> (int) num

    match currentVariable.Val with
    | Array array -> array.[IntValOfIndex]
    | Str str     -> Char str.[IntValOfIndex]
    | _           -> Empty


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



     