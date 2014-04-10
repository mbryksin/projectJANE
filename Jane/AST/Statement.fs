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

type DeclarationStatement(declarationType : Type, name : string, body : Initializer, pos : Position, parent : Block option) =
    inherit Statement(pos, parent)
    member x.Type = declarationType
    member x.Name = name
    member x.Body = body
   
    override x.ToString() = sprintf "%A %s = %A;\n" declarationType name body

    //add variable to the context
    override x.Interpret() = 
        let parentBlock = parent.Value
        parentBlock.Context <- Variable(name, declarationType, body.Interpret()) :: parentBlock.Context


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type AssignmentStatement(path : string list, body : Initializer, pos : Position, parent : Block option) =
    inherit Statement(pos, parent)
    member x.Path = path
    member x.Body = body

    override x.ToString() = sprintf "%s = %A;\n" (String.concat "." path) body

    //find variable in context and change value
    override x.Interpret() = 
        let parentBlockContent = parent.Value.Context
        let AssignVarName = path.Head             //using for simply imperative tests
        let valOfInitializer = body.Interpret()
        let currentVariable = List.find (fun (v: Variable) -> v.Name = AssignVarName) parentBlockContent
        currentVariable.Assign(valOfInitializer)

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
        if condition.Interpret().Bool.Value 
            then trueStatement.Interpret()
            else 
                 if falseStatement.IsSome 
                 then falseStatement.Value.Interpret()

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type WhileStatement(condition : Expression, body : Statement, pos : Position, parent : Block option) =
    inherit Statement(pos, parent)
    member x.Condition = condition
    member x.Body      = body

    //while condition is true - interpret body

    override x.ToString() = sprintf "while (%A) %A" condition body

    override x.Interpret() =
        while condition.Interpret().Bool.Value do
            body.Interpret()

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

type ForStatement(init : DeclarationStatement, condition : Expression, 
                  update : AssignmentStatement, body : Statement, pos : Position, parent : Block option) =
    inherit Statement(pos, parent)
    member x.Init      = init
    member x.Condition = condition
    member x.Update    = update
    member x.Body      = body

    override x.ToString() = sprintf "for (%A %s = %A; %A; %s = %A) %A" init.Type init.Name init.Body 
                                    condition (String.concat "." update.Path) update.Body body

    //interpret init, while condition is true interpret body and interpret update
    override x.Interpret() =   
        init.Interpret()    
        while condition.Interpret().Bool.Value do
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