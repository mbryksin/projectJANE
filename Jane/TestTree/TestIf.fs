﻿module TestIf

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
//        if (a < 5)
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

//If
let myCondition    = new BinaryOperation (new Identifier(new ID("a", p)), LESS, one, p)
let IfBlock        = new Block([myAssign], p, [], None)
let myIf           = new IfStatement(myCondition, IfBlock, None, p, None)

let myBlock        = new Block([myDecl; myIf; myAssign], p, [], None)
let myMethod       = new ClassVoidMethod(true, new ID("main", p),[], myBlock, p)

let myClassMembers = List.map (fun a -> a :> ClassMember) [myMethod]
let myConstructor  = new ClassConstructor(new ID("myClass", p), [], new Block([], p, [], None), p)
let myClass        = new Class(new ID("myClass", p), None, [], Some myConstructor, myClassMembers, p)
let myClasses      = List.map (fun a -> a :> ProgramMember) [myClass]
let myProg         = new Program(myClasses, "myClass", p)

let err, main = SA_Program myProg

printfn "%A" myProg
myProg.Interpret()