module TestFor

open AST
open StaticAnalysis
open Errors

//class myClass {
//
//    myClass() {
//    }
//
//    static int main() {
//        int a = 1;
//        for (int i = 1; i < 5; i++)
//        {
//            a = a + 2;
//        }
//        a = a + 2;
//    }
//
//}

let p              = new Position(0, 0, 0, 0)

let myInt          = new IntType(0, p)

let one            = new IntegerLiteral(1L, p)
let five           = new IntegerLiteral(5L, p)
let two            = new IntegerLiteral(2L, p)

let myDecl         = new DeclarationStatement(myInt, new ID("a", p), one, p, None)

let binOpPlus      = new BinaryOperation(new Identifier(new ID("a", p)), ADDITION, two, p)
let myAssign       = new AssignmentStatement(["a"], binOpPlus, p, None)

//for
let myDeclFor      = new DeclarationStatement(myInt, new ID("i", p), one, p, None)
let myBlockFor     = new Block([myAssign], p, [], None)
let binOpPlusFor   = new BinaryOperation(new Identifier(new ID("i", p)), ADDITION, one, p)
let myAssignFor    = new AssignmentStatement(["i"], binOpPlusFor, p, None)
let myCondition    = new BinaryOperation (new Identifier(new ID("i", p)), LESS, five, p)
let myFor          = new ForStatement(myDeclFor, myCondition , myAssignFor, myBlockFor, p, None)

let myBlock        = new Block([myDecl; myFor; myAssign], p, [], None)
let myMethod       = new ClassVoidMethod(true, new ID("main", p),[], myBlock, p)

let myClassMembers = List.map (fun a -> a :> ClassMember) [myMethod]
let myConstructor  = new ClassConstructor(new ID("myClass", p), [], new Block([], p, [], None), p)
let myClass        = new Class(new ID("myClass", p), None, [], Some myConstructor, myClassMembers, p)
let myClasses      = List.map (fun a -> a :> ProgramMember) [myClass]
let myProg         = new Program(myClasses, "myClass", p)

let err, main = SA_Program myProg

printfn "%A" myProg
myProg.Interpret()