module Test

open AST

//class myClass
//{
//    myClass()
//    {
//    }
//
//    int main()
//    {
//        string helloWorld = "Hello";
//        helloWorld = helloWorld + " World!"
//        stdout.print(helloWorld);
//    }
//}

/// p              - фиктивная нулевая позиция
let p              = new Position(0, 0, 0, 0)

let myTypeString   = new Type("string", 0, p)

let myHello        = new StringLiteral("Hello", p)
let myDecl         = new DeclarationStatement(myTypeString, "helloWorld", myHello, p)

let myWorld        = new StringLiteral(" World!", p)
let myMemberCall1  = new BinaryOperation(new Identifier("helloWorld", p), ADDITION, myWorld, p)
let myAssign       = new AssignmentStatement(["helloWorld"], myMemberCall1, p)
                           
let myMember       = new Member("print", new Arguments([new Identifier("helloWorld", p)], p), p)
let myMemberCall2  = new BinaryOperation(new Identifier("stdout", p), MEMBER_CALL, myMember, p)
let myPrint        = new MemberCallStatement(myMemberCall2, p)

let myBlock        = new Block([myDecl; myAssign; myPrint], p)
let myMethod       = new ClassMethod(true, new Type("int", 0), "main", [], myBlock, p)

let myClassMembers = List.map (fun a -> a :> ClassMember) [myMethod]
let myConstructor  = new ClassConstructor([], new Block([]))
let myClass        = new Class("myClass", None, [], myConstructor, myClassMembers)
let myClasses      = List.map (fun a -> a :> ProgramMember) [myClass]
let myProg         = new Program(myClasses)