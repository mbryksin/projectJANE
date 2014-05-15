//namespace UnitTesting
module testInterpret

open FsUnit
open NUnit.Framework
open AST
open SA.Program
open LanguageParser
open Interpret

let programText = "
    class Man {
            
        Man(int a, int e, int h)
        {
            Age = a;
            Energy = e;
            Height = h;
        }
        int Age = 0;
        int Energy = 100;
        int Height = 100;

        int getAge()
        {
            return Age;
        }

        int getEnergy()
        {
            return Energy;
        }

        void work()
        {
            Energy = Energy - 10;
        }

        static Man reproduct(Man father, Man mother)
        {
            let son = new Man(0,0,40);
            return son;
        }
    }

    class Ariphmetic {
            
        static int increment(int arg)
        {
            int b = arg + 1;
            return (b);
        }

        static int decrement(int arg)
        {
            int b = arg - 1;
            return (b);
        }

        static int sum(int a, int b)
        {
            int s = a + b;
            return (s);
        }

    }
    class myClass {

        myClass() {
        }

        static void main() {
            Man teenager = new Man(13,98,150);
            int age = teenager.getAge();
            teenager.work();
            teenager.work();
            int energy = teenager.getEnergy();
            Man ded = new Man(100,0,100);
            Man son = Man.reproduct(ded, teenager);
            int a = 5;
            int[] array = {{1,2,3}, {3,2,3}};
            int b = array[0];
            int s = Ariphmetic.sum(a,a);
            string str = \"lalka\";
            char c = str[0];
            int inc = Ariphmetic.increment(a);
            int dec = Ariphmetic.decrement(a);
        }
    }"


let test2 = "
    class Tree
    {
        int value = 0;
        Tree left = null;
        Tree right = null;

        int getVal()
        {
            return value;
        }

        void insert(int val)
        {
            if (value == 0) 
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
                    left.insert(value);
                }
                else
                {
                    if (right == null)
                    {
                        right = new Tree();
                    }
                    right.insert(value);
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
            myTree.insert(1);
            myTree.insert(3);
            myTree.insert(2);
            myTree.insert(4);
        }
    }"