namespace AST

[<AbstractClass>]
type Statement(pos : Position, parent : Block option) =
    inherit Node(pos)
    let mutable parent = parent
    member x.Parent
        with get() = parent
        and  set(value) = parent <- value

    abstract member Interpret: unit -> unit

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


and Block(statements : Statement list, pos : Position, context : Variable list, parent : Block option) =
    inherit Statement(pos, parent)
    let mutable context = context

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


type DeclarationStatement(declarationType : Type, name : ID, body : Initializer, pos : Position, parent : Block option) =
    inherit Statement(pos, parent)
    member x.Type = declarationType
    member x.Name = name
    member x.Body = body

    override x.ToString() = sprintf "%A %A = %A;\n" declarationType name body

    //add variable to the context
    override x.Interpret() = 
        let parentBlock = x.Parent.Value
        let context = x.Parent.Value.Context
        parentBlock.Context <- Variable(name.Value, declarationType, body.Interpret(context)) :: parentBlock.Context


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type AssignmentStatement(path : string list, body : Initializer, pos : Position, parent : Block option) =
    inherit Statement(pos, parent)
    member x.Path = path
    member x.Body = body

    override x.ToString() = sprintf "%s = %A;\n" (String.concat "." path) body

    //find variable in context and change value
    override x.Interpret() = 
        let parentBlockContext = x.Parent.Value.Context
        let AssignVarName = path.Head             //using for simply imperative tests
        let valOfInitializer = body.Interpret(parentBlockContext)
        let currentVariable = List.find (fun (v: Variable) -> v.Name = AssignVarName) parentBlockContext
        printfn "%s" <| "beforeAssign " + currentVariable.ToString() //for debugging
        currentVariable.Assign(valOfInitializer)
        printfn "%s" <| "AfterAssign " + currentVariable.ToString()

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type MemberCallStatement(body : Expression, pos : Position, parent : Block option) =
    inherit Statement(pos, parent)
    member x.Body = body
    
    override x.ToString() = sprintf "%A;\n" body

    override x.Interpret() = () // later

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type IfStatement(condition : Expression, trueStatement : Statement, 
                 falseStatement : Statement option, pos : Position, parent : Block option) =
    inherit Statement(pos, parent)
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
        if condition.Interpret(context).Bool.Value
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

type WhileStatement(condition : Expression, body : Statement, pos : Position, parent : Block option) =
    inherit Statement(pos, parent)
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
        while condition.Interpret(context).Bool.Value do
            body.Interpret()

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type ForStatement(init : DeclarationStatement, condition : Expression, 
                  update : AssignmentStatement, body : Statement, pos : Position, parent : Block option) =
    inherit Statement(pos, parent)
    member x.Init      = init
    member x.Condition = condition
    member x.Update    = update
    member x.Body      = body

    override x.ToString() = sprintf "for (%A %A = %A; %A; %s = %A) %A" init.Type init.Name init.Body 
                                    condition (String.concat "." update.Path) update.Body body

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
        while condition.Interpret(context).Bool.Value do
            body.Interpret()
            update.Interpret()
                            

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type BreakStatement(pos : Position, parent : Block option) =
    inherit Statement(pos, parent)

    override x.ToString() = "break;"

    override x.Interpret() = () // later

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type ContinueStatement(pos : Position, parent : Block option) =
    inherit Statement(pos, parent)

    override x.ToString() = "continue;"

    override x.Interpret() = () // later

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type ReturnStatement(expression : Expression option, pos : Position, parent : Block option) =
    inherit Statement(pos, parent)
    member x.Expression = expression

    override x.ToString() = sprintf "return %A;" expression

    override x.Interpret() = () //later

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type SuperStatement(arguments : Arguments, pos : Position, parent : Block option) =
    inherit Statement(pos, parent)
    member x.Arguments = arguments

    override x.ToString() = sprintf "super%A;" arguments

    override x.Interpret() = () //later
