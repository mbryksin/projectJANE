namespace AST

[<AbstractClass>]
type Statement(pos : Position) =
    inherit Node(pos)
    let mutable parent = None
    member x.Parent
        with get() = parent
        and  set(value) = parent <- value

    abstract member Interpret: unit -> unit

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


and Block(statements : Statement list, pos : Position) =
    inherit Statement(pos)
    let mutable context = []

    member x.Statements = statements
    member x.Context 
        with get () = context
        and  set (value) = context <- value

    override x.ToString() = statements
                            |> List.map string
                            |> String.concat ""
                            |> sprintf "{\n%s}\n" 

    //Interpret all statement in block
    override x.Interpret() =
        List.iter (fun (s : Statement) -> 
                      s.Parent <- Some x
                      // if statenemt is block, copy content from parent
                      match s with
                          | :? Block as block -> block.Context <- x.Context
                          | _ -> ()
                      s.Interpret()
                  ) statements

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


type DeclarationStatement(declarationType : Type, name : ID, body : Initializer, pos : Position) =
    inherit Statement(pos)
    member x.Type = declarationType
    member x.Name = name
    member x.Body = body

    override x.ToString() = sprintf "%A %A = %A;\n" declarationType name body

    //add variable to the context
    override x.Interpret() = 
        let parentBlock = x.Parent.Value
        let context = x.Parent.Value.Context
        parentBlock.Context <- Variable(name.Value, declarationType, body.Interpret(context)) :: parentBlock.Context
        printfn "%s" <|  "declaration" + name.Value + parentBlock.Context.Head.Val.ToString()


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type AssignmentStatement(name : ID, body : Initializer, pos : Position) =
    inherit Statement(pos)
    member x.Name = name
    member x.Body = body

    override x.ToString() = sprintf "%A = %A;\n" name body

    //find variable in context and change value
    override x.Interpret() = 
        let parentBlockContext = x.Parent.Value.Context
        let AssignVarName = name.Value             //using for simply imperative tests
        let valOfInitializer = body.Interpret(parentBlockContext)
        let currentVariable = List.find (fun (v: Variable) -> v.Name = AssignVarName) parentBlockContext
        printfn "%s" <| "beforeAssign " + currentVariable.ToString() //for debugging
        currentVariable.Assign(valOfInitializer)
        printfn "%s" <| "AfterAssign " + currentVariable.ToString()  

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type MemberCallStatement(body : Expression, pos : Position) =
    inherit Statement(pos)
    member x.Body = body
    
    override x.ToString() = sprintf "%A;\n" body

    override x.Interpret() = () // later

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type IfStatement(condition : Expression, trueStatement : Statement, 
                 falseStatement : Statement option, pos : Position) =
    inherit Statement(pos)
    member x.Condition      = condition
    member x.TrueStatement  = trueStatement
    member x.FalseStatement = falseStatement
    
    override x.ToString() = let ifPart = sprintf "if (%A) %A" condition trueStatement
                            match falseStatement with
                            | None           -> ifPart
                            | Some statement -> sprintf "%s\nelse %A" ifPart statement

    //if condition is true - interpret trueStatement, else - falseStatement if it present
    override x.Interpret() =
        let context = x.Parent.Value.Context
        let isTrueCondition (cond : Val) =
            match cond with
            | Bool true -> true
            | Int n when n <> 0L -> true
            | _ -> false
        if isTrueCondition(condition.Interpret(context)) = true
            then trueStatement.Parent <- x.Parent
                 match trueStatement with
                 | :? Block as block -> block.Context <- x.Parent.Value.Context
                 | _ -> ()
                 trueStatement.Interpret()          
            else 
                 if falseStatement.IsSome 
                 then falseStatement.Value.Parent <- x.Parent
                      match falseStatement.Value with
                      | :? Block as block -> block.Context <- x.Parent.Value.Context
                      | _ -> ()
                      falseStatement.Value.Interpret()

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type WhileStatement(condition : Expression, body : Statement, pos : Position) =
    inherit Statement(pos)
    member x.Condition = condition
    member x.Body      = body

    //while condition is true - interpret body

    override x.ToString() = sprintf "while (%A) %A" condition body

    override x.Interpret() =
        let context = x.Parent.Value.Context
        body.Parent <- x.Parent
        match body with
        | :? Block as block -> block.Context <- x.Parent.Value.Context
        | _ -> ()

        let isTrueCondition (cond : Val) =
            match cond with
            | Bool true -> true
            | Int n when n <> 0L -> true
            | _ -> false

        while isTrueCondition(condition.Interpret(context)) do
            body.Interpret()

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type ForStatement(init : DeclarationStatement, condition : Expression, 
                  update : AssignmentStatement, body : Statement, pos : Position) =
    inherit Statement(pos)
    member x.Init      = init
    member x.Condition = condition
    member x.Update    = update
    member x.Body      = body

    override x.ToString() = sprintf "for (%A %A = %A; %A; %A = %A) %A" init.Type init.Name init.Body 
                                    condition update.Name update.Body body

    //interpret init, while condition is true interpret body and interpret update
    override x.Interpret() =   
        init.Parent <- x.Parent
        update.Parent <- x.Parent

        init.Interpret()  
        let context = x.Parent.Value.Context
        body.Parent <- x.Parent
        match body with
        | :? Block as block -> block.Context <- x.Parent.Value.Context
        | _ -> ()    

        let isTrueCondition (cond : Val) =
            match cond with
            | Bool true -> true
            | Int n when n <> 0L -> true
            | _ -> false

        while isTrueCondition(condition.Interpret(context)) do
            body.Interpret()
            update.Interpret()
                            

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type BreakStatement(pos : Position) =
    inherit Statement(pos)

    override x.ToString() = "break;"

    override x.Interpret() = () // later

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type ContinueStatement(pos : Position) =
    inherit Statement(pos)

    override x.ToString() = "continue;"

    override x.Interpret() = () // later

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type ReturnStatement(expression : Expression option, pos : Position) =
    inherit Statement(pos)
    member x.Expression = expression

    override x.ToString() = sprintf "return %A;" expression

    override x.Interpret() = () //later

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type SuperStatement(arguments : Arguments, pos : Position) =
    inherit Statement(pos)
    member x.Arguments = arguments

    override x.ToString() = sprintf "super%A;" arguments

    override x.Interpret() = () //later
