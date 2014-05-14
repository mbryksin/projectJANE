module FullProgramTest


open FsUnit
open NUnit.Framework
open SA.Program
open LanguageParser
open Interpret
open SupportFunction

[<TestFixture>]
type TestingFullProgram() =
    [<Test>]
    member x. ``Interpret: Tree`` ()=
        let programText ="
            class Tree
            {
                int value = -1;
                Tree left = null;
                Tree right = null;

                static int height(Tree tree)
                {
                    if (tree == null) 
                    {           
                        return 0;
                    }         
                    else
                    {
                        int leftH  = Tree.height(tree.left);
                        int rightH = Tree.height(tree.right);
                        int max = 0;
                        if (leftH > rightH)
                        {
                            max = leftH;
                        }
                        else
                        {
                            max = rightH;
                        }
                        return max+1;
                    }   
            
                }
                int getVal()
                {
                    return value;
                }

                int setVal(int setVal)
                {
                    value = setVal;
                }

                void insert(int val)
                {
                    if (value == -1) 
                    {           
                        value = val;  
                    }         
                    else
                    {
                        if (value > val)
                        {
                            if (left == null)
                            {
                                left = new Tree();
                            }
                            left.insert(val);
                        }
                        else
                        {
                            if (right == null)
                            {
                                right = new Tree();
                            }
                            right.insert(val);
                        }            
                    }   
            
                }  
            }

            

            class mainClass 
            {
                static void main() 
                {
                    Tree myTree = new Tree();
                    myTree.insert(7);
                    myTree.insert(8);
                    myTree.insert(3);
                    myTree.insert(2);
                    myTree.insert(4);
                    Tree leftTree = myTree.left;
                    int leftVal = leftTree.getVal();
                    Console.Writeline(leftVal);
                    int h = Tree.height(myTree);
                    Console.Writeline(h);
                }
            }"            
        getResult(programText) = "33"  |> should be True