namespace AST

type Statement =
    interface 
        abstract member Interpret: unit -> unit
    end

type Block(statements : Statement list) =
    member x.Statements = statements
    //Interpret all statement in block
    interface Statement with
        member x.Interpret() =
            List.iter (fun (s : Statement) -> s.Interpret()) statements

type DeclarationStatement(declarationType : Type, name : string, body : Initializer) =
    member x.Type = declarationType
    member x.Name = name
    member x.Body = body
    //add variable to the context
    interface Statement with
        member x.Interpret() = () // do this

type AssignmentStatement(path : string list, body : Initializer) =
    member x.Path = path
    member x.Body = body
    //find variable in context and change value
    interface Statement with
        member x.Interpret() = () // do this

type MemberCallStatement(body : Expression) =
    member x.Body = body
    interface Statement with
        member x.Interpret() = () // later
    
type IfStatement(condition : Expression, trueStatement : Statement, falseStatement : Statement option) =
    member x.Condition      = condition
    member x.TrueStatement  = trueStatement
    member x.FalseStatement = falseStatement
    //if condition is true - interpret trueStatement, else - falseStatement if it present
    interface Statement with
        member x.Interpret() =
            if condition.Interpret().Bool.Value 
                then trueStatement.Interpret()
                else 
                     if falseStatement.IsSome 
                     then falseStatement.Value.Interpret()

type WhileStatement(condition : Expression, body : Statement) =
    member x.Condition = condition
    member x.Body      = body
    //while condition is true - interpret body
    interface Statement with
        member x.Interpret() =
            while condition.Interpret().Bool.Value do
                body.Interpret()

type ForStatement(init : DeclarationStatement list, condition : Expression option, update : AssignmentStatement list, body : Statement) =
    member x.Init      = init
    member x.Condition = condition
    member x.Update    = update
    member x.Body      = body
    //interpret init, while condition is true interpret body and interpret update
    interface Statement with
        member x.Interpret() =   
            List.iter (fun (dS : DeclarationStatement) -> (dS :> Statement).Interpret()) init
            if condition.IsSome
                then           
                    let valCondition = condition.Value
                    while valCondition.Interpret().Bool.Value do
                        body.Interpret()
                        List.iter (fun (aS : AssignmentStatement) -> (aS :> Statement).Interpret()) update
                else
                    while true do
                        body.Interpret()


type BreakStatement() =
    interface Statement with
        member x.Interpret() = () //later

type ContinueStatement() =
    interface Statement with
        member x.Interpret() = () //later

type ReturnStatement(expression : Expression option) =
    member x.Expression = expression
    interface Statement with
        member x.Interpret() = () //later

type SuperStatement(arguments : Expression list) =
    member x.Arguments = arguments
    interface Statement with
        member x.Interpret() = () //later
