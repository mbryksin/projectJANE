module internal SA.Statement

open AST

// GD ~ Gathering Data
// SA ~ Static Analysis

let rec SA_Statement (p : Program) (c : Class) (s : Statement) =
    match s with
    | :? Block                as b  -> SA_Block p c b
    | :? DeclarationStatement as d  -> ()
    | :? AssignmentStatement  as a  -> ()
    | :? MemberCallStatement  as mc -> ()
    | :? IfStatement          as i  -> ()
    | :? WhileStatement       as w  -> ()
    | :? ForStatement         as f  -> ()
    | :? BreakStatement       as b  -> ()
    | :? ContinueStatement    as c  -> ()
    | :? ReturnStatement      as r  -> ()
    | :? SuperStatement       as s  -> ()
    | _  -> ()

and SA_Block (p : Program) (c : Class) (b : Block) =
    b.Statements
    |> List.iter (fun s -> s.Context <- b.Context
                           SA_Statement p c s
                           if (s :? DeclarationStatement) then 
                               let ds = s :?> DeclarationStatement
                               let newContexElem = new Variable(ds.Name.Value, ds.Type, Null)
                               b.Context <- newContexElem :: b.Context
                 )

    


    
    
    
    
    
    
    
    
    
    
    