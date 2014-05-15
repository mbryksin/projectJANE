﻿module Interpret

open ServiceFunction
open ExpressionFunctions
open ArrayFunctions

open AST
open SA.Program
open LanguageParser
open Errors

let mutable (currentProgram : Program option) = None

//***************************************************Program***************************************************//
//Find mainClass, find mainMethod and interpret it
let rec interpretProgram(program: Program) =
    let mainMethod = program.MainMethod.Value :?> ClassVoidMethod
    currentProgram <- Some program
    interpretMethod mainMethod [] |> ignore

//***************************************************ClassMembers***************************************************//

and interpretMethod (classMethod : ClassMethod) (args : Val list) = 
    let body = classMethod.Body
    let parameters = classMethod.Parameters
    //add arguments to method body context
    addArgsToMethodContext args parameters body    
    interpretStatement (classMethod.Body :> Statement)

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
                    let valueInterpret = interpretStatement currStatement 
                    statementsInterpret statements
             | :? ReturnStatement as rs -> interpretReturnStatement rs                              
             | _ -> interpretStatement currStatement |> ignore
                    statementsInterpret statements
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
    //add context if statement is block
    addToBlockContext body context

    while isTrueCondition(interpretExpression condition context) do
        interpretStatement body |> ignore
        interpretAssignmentStatement update |> ignore
    Empty

and interpretSuperStatement (ss : SuperStatement) = Empty             //DO IT

// Interpret expression and return it
and interpretReturnStatement (rs : ReturnStatement) = 
    let expressionReturn = rs.Expression.Value
    let context = rs.Parent.Value.Context
    let returnValue = interpretExpression expressionReturn context
    Return returnValue

and interpretBreakStatement (bs : BreakStatement) = Empty              //DO IT

and interpretContinueStatement (cs : ContinueStatement) = Empty        //DO IT

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
    let firstOpetandVal  = interpretExpression binOp.FirstOperand  context
    let secondOpetandVal = interpretExpression binOp.SecondOperand context

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
    | DIVISION         -> division firstOpetandVal secondOpetandVal
    | MODULUS          -> modul firstOpetandVal secondOpetandVal 
    | MEMBER_CALL      -> let progListClasses = currentProgram.Value.Classes
                          match firstOpetandVal, secondOpetandVal with
                          | ClassOrField className, MethodVal (methodName, elems) ->
                              let currClass = progListClasses.[className]
                              let methods = currClass.Methods
                              let currMethod = methods.[methodName]
                              let args = elems
                              interpretMethod currMethod args
                          | Object (fields, className), MethodVal (methodName, elems) ->
                              let currClass = progListClasses.[className]
                              let methods = currClass.Methods
                              let currMethod = methods.[methodName]
                              let args = elems
                              currMethod.Body.Context <- fields @ currMethod.Body.Context 
                              interpretMethod currMethod args
                          | Object (fields, className), ClassOrField fieldName ->
                              let field = List.find (fun (f : Variable) -> f.Name = fieldName) fields
                              field.Val
                          | ClassOrField className, ClassOrField fieldName ->
                              let currClass = progListClasses.[className]
                              let field = currClass.Fields.[fieldName]
                              interpretExpression field.Body context
                          | _, _ -> Empty

and interpretUnaryOperation (unOp: UnaryOperation) (context : Variable list) =
    let operandVal = interpretExpression unOp.Operand context 
    match unOp.Operator with
    | NOT              -> notLogical operandVal
    | MINUS            -> minus operandVal

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
    let classConstructorBody = classConstructor.Body
    let Fields = classforObject.Fields

    classConstructorBody.Context <- [] //clearOldContext
        
    let fieldsAsVar = 
        Seq.map (fun (f : ClassField) -> 
                      let varName = f.Name.Value
                      let varType = f.Type
                      let varVal = interpretExpression f.Body context
                      let var = new Variable(varName, varType, varVal)
                      var
                      ) Fields.Values 
        |> Seq.toList

    classConstructor.Body.Context <- fieldsAsVar @ classConstructor.Body.Context

    let parametersConstructor = classConstructor.Parameters;
    addArgsToMethodContext interpretArgs parametersConstructor classConstructorBody
    interpretBlock classConstructorBody |> ignore 
    Object (fieldsAsVar, name)
    

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
            getValueOfIndex currentVariable ValOfIndex     
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

//***************************************************TEST***************************************************//

let programText = "
    class Man {
            
        Man(int a, int e, int h)
        {
            Age = a;
            Energy = e;
            Height = h;
        }
        int Age = 0;
        int Energy = 100;
        int Height = 100;

        int getAge()
        {
            return Age;
        }

        int getEnergy()
        {
            return Energy;
        }

        void work()
        {
            Energy = Energy - 10;
        }

        static Man reproduct(Man father, Man mother)
        {
            let son = new Man(0,0,40);
            return son;
        }
    }

    class Ariphmetic {
        static int factor = 1;
            
        static int increment(int arg)
        {
            int b = arg + 1;
            return (b);
        }

        static int decrement(int arg)
        {
            int b = arg - 1;
            return (b);
        }

        static int sum(int a, int b)
        {
            if (0==0)
            {
            int s = a + b;
            return (s);
            }
        }

        static int fact(int n)
        {
            if (n < 2)
            {
                return 1;
            }
            else
            {
                int fact = Ariphmetic.fact(n-1) * n;
                return fact;
            }
        }

    }
    class myClass {

        myClass() {
        }

        static void main() {
            Man x = null;
            if (x == null)
            {
                x = new Man(1,2,3);
            }
            else
            {
                x = new Man(3,2,1);
            }
            x.work();
            int age = x.getAge();
            int age2 = x.Age;
        }
    }"

let myProg = ParseProgram programText
myProg.NameMainClass <- "myClass"  
SA_Program myProg
printfn "%A" myProg
interpretProgram myProg 


     