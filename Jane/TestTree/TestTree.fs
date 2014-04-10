module Test

open AST

//class myClass {
//
//    myClass() {
//    }
//
//    static int main() {
//        string helloWorld = "Hello";
//        helloWorld = helloWorld + " World!"
//        stdout.print(helloWorld);
//    }
//
//}

/// p              - фиктивная нулевая позиция

//Сначала все контексты пусты и все Родители - None. Эти поля заполняются при интерпретиции
let p              = new Position(0, 0, 0, 0)

let myTypeString   = new StringType(0, p)

let myHello        = new StringLiteral("Hello", p)
let myDecl         = new DeclarationStatement(myTypeString, "helloWorld", myHello, p, None)

let myWorld        = new StringLiteral(" World!", p)
let myMemberCall1  = new BinaryOperation(new Identifier("helloWorld", p), ADDITION, myWorld, p)
let myAssign       = new AssignmentStatement(["helloWorld"], myMemberCall1, p, None)
                           
let myMember       = new Member("print", new Arguments([new Identifier("helloWorld", p)], p), p)
let myMemberCall2  = new BinaryOperation(new Identifier("stdout", p), MEMBER_CALL, myMember, p)
let myPrint        = new MemberCallStatement(myMemberCall2, p, None)

let myBlock        = new Block([myDecl; myAssign; myPrint], p, [], None)
let myMethod       = new ClassReturnMethod(true, new IntType(0, p), "main", [], myBlock, p)

let myClassMembers = List.map (fun a -> a :> ClassMember) [myMethod]
let myConstructor  = new ClassConstructor("myClass", [], new Block([], p, [], None), p)
let myClass        = new Class("myClass", None, [], Some myConstructor, myClassMembers, p)
let myClasses      = List.map (fun a -> a :> ProgramMember) [myClass]
let myProg         = new Program(myClasses, "MyClass", p)

printfn "%A" myProg


//let x = new Variable("x", IntType(0, Position(0,0,0,0)), Val(5L))
//let l = [x; x]
//let mutable k = l
//k <- x :: k
//l.Head.Assign(Val(10L)) 
//List.iter (fun (x : Variable) -> x.Val.Int.Value |> printf "%A") k
//l.Head.Assign(Val(47L)) 
//List.iter (fun (x : Variable) -> x.Val.Int.Value |> printf "%A") k